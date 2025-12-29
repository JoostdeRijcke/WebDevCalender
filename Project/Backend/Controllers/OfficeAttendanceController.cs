using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CalendifyApp.Models;
using CalendifyApp.Filters;
using CalendifyApp.Services;


namespace CalendifyApp.Controllers
{
    [AuthorizationFilter]
    [ApiController]
    [Route("[controller]")]
    public class OfficeAttendanceController : ControllerBase
    {
        private readonly IOfficeAttendanceService _officeAttendanceService;

        public OfficeAttendanceController(IOfficeAttendanceService officeAttendanceService)
        {
            _officeAttendanceService = officeAttendanceService;
        }

        [HttpPost]
        public async Task<IActionResult> AttendOffice([FromBody] Attendance attendance)
        {
            // Check if the user has already booked the selected date
            bool attendanceExists = await _officeAttendanceService.IsDateBookedAsync(attendance.UserId, attendance.Date);

            if (attendanceExists)
            {
                return Conflict("The selected time is already booked. Please choose another time.");
            }

            // Add new booking for the date
            await _officeAttendanceService.AddAttendanceAsync(attendance);

            return Ok($"Time booked successfully. BookingId = {attendance.Id}");
        }   

        [HttpDelete("{userId}/{date}")]
        public async Task<IActionResult> UnAttend(int userId, DateOnly date)
        {
            DateTime dateTime = date.ToDateTime(TimeOnly.MinValue);
            bool removed = await _officeAttendanceService.RemoveAttendanceAsync(userId, dateTime);

            if (removed)
            {
                return Ok("Booking has been cancelled");
            }

            return NotFound("Attendance record not found for the given user and date.");
        }
    }
}
