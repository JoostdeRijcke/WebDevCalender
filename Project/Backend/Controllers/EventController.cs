using Microsoft.AspNetCore.Mvc;
using CalendifyApp.Models;
using CalendifyApp.Services;
using CalendifyApp.Filters;
using System.Collections.Generic;

namespace CalendifyApp.Controllers
{
    [ApiController]
    [Route("api/Events")]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult GetAllEvents()
        {
            var events = _eventService.GetAllEvents();
            if (events == null || !events.Any())
                return NotFound("No events available.");
            
            return Ok(events);
        }

        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
                return NotFound($"No event found with ID {id}.");

            return Ok(eventDetails);
        }


        [AdminFilter]
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] DTOEvent eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

                if (eventDto.Date < DateTime.UtcNow.Date)
                    return BadRequest("Event date cannot be in the past.");

                if (eventDto.StartTime >= eventDto.EndTime)
                    return BadRequest("End time must be after start time.");

            // Call the AddEvent method from the service
            var createdEvent = await _eventService.AddEvent(eventDto);

            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        [AdminFilter]
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, [FromBody] UpdateEventDTO updatedEvent)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEvent = _eventService.GetEventById(id);
            if (existingEvent == null)
                return NotFound($"Event with ID {id} not found.");

            var success = _eventService.UpdateEvent(id, updatedEvent);
            if (success)
                return Ok($"Event with ID {id} updated successfully.");

            return BadRequest("Failed to update the event.");
        }


        [AdminFilter]
        [HttpDelete("{id}")]
        public IActionResult DeleteEvent(int id)
        {
            var result = _eventService.DeleteEvent(id);
            if (result)
                return Ok($"Event with ID {id} deleted successfully.");
            
            return NotFound($"Event with ID {id} not found.");
        }

        [HttpGet("reviews")]
        public IActionResult GetAllReviews()
        {
            var reviews = _eventService.GetAllReviews();
            if (reviews == null || !reviews.Any())
                return NotFound("No reviews found.");
            
            return Ok(reviews);
        }

        [HttpPost("review")]
        public IActionResult AddReview([FromBody] ReviewDto reviewDto)
        {
            var review = new EventAttendance
            {
                UserId = reviewDto.UserId,
                EventId = reviewDto.EventId,
                Rating = reviewDto.Rating,
                Feedback = reviewDto.Feedback,
                AttendedAt = reviewDto.AttendedAt
            };

            var result = _eventService.AddReview(review);
            if (result)
                return Ok("Review added successfully.");

            return BadRequest("Failed to add the review.");
        }

        [HttpGet("search")]
        public IActionResult SearchEvents(string? title = null, string? location = null, DateTime? startDate = null, DateTime? endDate = null)
        {
            var events = _eventService.SearchEvents(title, location, startDate, endDate);
            if (events == null || !events.Any())
                return NotFound("No matching events found.");
            
            return Ok(events);
        }
    }

    public class ReviewDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int? Rating { get; set; }
        public string? Feedback { get; set; }
        public DateTime AttendedAt { get; set; }
    }
}
