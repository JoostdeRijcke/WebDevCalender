using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using CalendifyApp.Models;
using CalendifyApp.Filters; // Import the filter


namespace CalendifyApp.Controllers
{   
    [AuthorizationFilter]
    [ApiController]
    [Route("[controller]")]
    public class OfficeAttendanceController : ControllerBase
    {
        private readonly MyContext _context;
        //constructor to inject the databse
        public OfficeAttendanceController(MyContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AttendOffice([FromBody] Attendance attendance)
        {
            // Check if the user has already booked the selected date
            bool attendanceExists = await _context.Attendance // await pauses execution here until the database query completes
                .AnyAsync(a => a.UserId == attendance.UserId && a.Date == attendance.Date);

            if (attendanceExists)
            {
                //return Conflict(new { message = "The selected time is already booked. Please choose another time." });
                return Conflict("The selected time is already booked. Please choose another time.");

            }

            // Add new booking for the date
            await _context.Attendance.AddAsync(attendance);
            await _context.SaveChangesAsync();

            // Return a created response with the ID of the newly created booking
            //return CreatedAtAction(nameof(AttendOffice), new { id = attendance.Id }, new { message = $"Time booked successfully. BookingId = {attendance.Id}"});
           // return Ok(new { message = $"Time booked successfully. BookingId = {attendance.Id}" });
            return Ok($"Time booked successfully. BookingId = {attendance.Id}");


        }   

        [HttpDelete("{userId}/{date}")]
        public IActionResult UnAttend(int userId, DateOnly date) 
        {
            // Find the existing attendance record for the user on the specified date
            var existingAttendance = _context.Attendance
                .FirstOrDefault(a => a.UserId == userId && DateOnly.FromDateTime(a.Date) == date);

            if (existingAttendance != null)
            {
                _context.Attendance.Remove(existingAttendance); // Remove the record
                _context.SaveChanges(); // Save changes to the database
                return Ok("Booking has been cancelled"); // Return success message
            }

            return NotFound("Attendance record not found for the given user and date.");
        }
    }
}
