using Microsoft.AspNetCore.Mvc;
using CalendifyApp.Models;
using CalendifyApp.Services;
using CalendifyApp.Filters;

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
        public IActionResult GetAllEvents([FromQuery] bool onlyUpcoming = false)
        {
            var events = _eventService.GetAllEvents();

            if (events == null || !events.Any())
                return NotFound("No events available.");

            if (onlyUpcoming)
            {
                var now = DateTime.UtcNow;
                events = events.Where(e =>
                {
                    DateTime endDateTime = e.Date.Date.Add(e.EndTime);

                    // Behandel als local time en converteer naar UTC
                    if (endDateTime.Kind == DateTimeKind.Unspecified)
                    {
                        endDateTime = DateTime.SpecifyKind(endDateTime, DateTimeKind.Local).ToUniversalTime();
                    }

                    return endDateTime >= now;
                }).ToList();
            }

            return Ok(events);
        }
        [HttpGet("upcoming")]
        public IActionResult GetUpcoming()
        {
            var events = _eventService.GetAllEvents();
            if (events == null || !events.Any()) return NotFound("No events available.");

            var now = DateTime.UtcNow;
            var upcoming = events
                .Where(e => GetEndDateTimeUtc(e) >= now)
                .OrderBy(e => GetEndDateTimeUtc(e))
                .ToList();

            return Ok(upcoming);
        }


        [HttpGet("{id}")]
        public IActionResult GetEventById(int id)
        {
            var eventDetails = _eventService.GetEventById(id);
            if (eventDetails == null)
                return NotFound($"No event found with ID {id}.");

            return Ok(eventDetails);
        }

        //[AdminFilter]
        [HttpPost]
        public async Task<IActionResult> AddEvent([FromBody] DTOEvent eventDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (eventDto.Date < DateTime.UtcNow.Date)
                return BadRequest("Event date cannot be in the past.");

            if (eventDto.StartTime >= eventDto.EndTime)
                return BadRequest("End time must be after start time.");

            var createdEvent = await _eventService.AddEvent(eventDto);
            return CreatedAtAction(nameof(GetEventById), new { id = createdEvent.Id }, createdEvent);
        }

        //[AdminFilter]
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

        //[AdminFilter]
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
        public IActionResult AddReview([FromBody] EventAttendance review)
        {
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

        // ---------- Helpers (werken met DetailedEventDTO) ----------

        /// <summary>
        /// Bepaalt de eind-datum/tijd van een event in UTC op basis van Date + EndTime (fallback: StartTime, anders 23:59:59).
        /// Werkt met TimeSpan of string ("HH:mm:ss").
        /// Pas aan als jouw DTO andere property-namen/types heeft.
        /// </summary>
        private static DateTime GetEndDateTimeUtc(DetailedEventDTO e)
        {
            // Aanname: e.Date is DateTime (lokale/UTC onbekend). We normaliseren naar UTC als UTC is bedoeld:
            var date = DateTime.SpecifyKind(e.Date.Date, DateTimeKind.Utc);

            var end = TryParseTime(e.EndTime);
            var start = TryParseTime(e.StartTime);
            var endTime = end ?? start ?? new TimeSpan(23, 59, 59);

            return date.Add(endTime);
        }

        private static TimeSpan? TryParseTime(object? t)
        {
            if (t is null) return null;
            if (t is TimeSpan ts) return ts;
            if (t is string s && TimeSpan.TryParse(s, out var parsed)) return parsed;
            return null;
        }
    }
}
