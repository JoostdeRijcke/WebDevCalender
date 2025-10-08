using CalendifyApp.Models;
using CalendifyApp.Services;
using CalendifyApp.Filters;
using Microsoft.AspNetCore.Mvc;
using System;


namespace CalendifyApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventAttendanceController : ControllerBase
    {
        private readonly IEventAttendanceService _eventAttendanceService;

        public EventAttendanceController(IEventAttendanceService eventAttendanceService)
        {
            _eventAttendanceService = eventAttendanceService;
        }

        [HttpPost("attend")]
        public IActionResult AttendEvent([FromBody] AttendanceDto attendance)
        {
            try
            {
                var result = _eventAttendanceService.AttendEvent(attendance);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("attendees/{eventId}")]
        public IActionResult GetEventAttendees(int eventId)
        {
            try
            {
                var attendees = _eventAttendanceService.GetEventAttendees(eventId);

                if (!attendees.Any())
                {
                    return Ok(new { Message = "No attendees found for this event.", Attendees = attendees });
                }

                return Ok(attendees);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }


        [HttpGet("user/{userId}/attended-events")]
        public IActionResult GetEventsByUser(int userId)
        {
            try
            {
                var events = _eventAttendanceService.GetEventsByUser(userId);
                return Ok(events);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("remove")]
        public IActionResult RemoveAttendance([FromBody] AttendanceDto attendance)
        {
            try
            {
                _eventAttendanceService.RemoveAttendance(attendance);
                return Ok("Registration successfully removed.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
