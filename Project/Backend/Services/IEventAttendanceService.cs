using CalendifyApp.Models;
using System.Collections.Generic;

namespace CalendifyApp.Services
{
    public interface IEventAttendanceService
    {
        string AttendEvent(AttendanceDto attendance);
        IEnumerable<object> GetEventAttendees(int eventId);
        IEnumerable<object> GetEventsByUser(int userId);
        void RemoveAttendance(AttendanceDto attendance);
    }
}
