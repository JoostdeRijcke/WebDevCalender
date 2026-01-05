using CalendifyApp.Models;
using CalendifyApp.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CalendifyApp.Seeders
{
    public static class DatabaseSeeder
    {
        public static void Seed(MyContext context)
        {
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Password = EncryptionHelper.EncryptPassword("password1"), RecurringDays = 1 },
                    new User { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", Password = EncryptionHelper.EncryptPassword("password2"), RecurringDays = 2 },
                    new User { FirstName = "Alice", LastName = "Johnson", Email = "alice.johnson@example.com", Password = EncryptionHelper.EncryptPassword("password3"), RecurringDays = 0 },
                    new User { FirstName = "Bob", LastName = "Brown", Email = "bob.brown@example.com", Password = EncryptionHelper.EncryptPassword("password4"), RecurringDays = 1 },
                    new User { FirstName = "Charlie", LastName = "Davis", Email = "charlie.davis@example.com", Password = EncryptionHelper.EncryptPassword("password5"), RecurringDays = 4 }
                );
                context.SaveChanges();
            }

            if (!context.Events.Any())
            {
                context.Events.AddRange(
                    new Event { Title = "Meeting", Description = "Team meeting", Date = DateTime.Now.AddDays(1), StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(12), Location = "Room 101", AdminApproval = true },
                    new Event { Title = "Workshop", Description = "Coding workshop", Date = DateTime.Now.AddDays(2), StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(16), Location = "Room 202", AdminApproval = true },
                    new Event { Title = "Conference", Description = "Tech conference", Date = DateTime.Now.AddDays(3), StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Location = "Auditorium", AdminApproval = true },
                    new Event { Title = "Presentation", Description = "Project presentation", Date = DateTime.Now.AddDays(4), StartTime = TimeSpan.FromHours(11), EndTime = TimeSpan.FromHours(13), Location = "Room 303", AdminApproval = true },
                    new Event { Title = "Training", Description = "Skill training", Date = DateTime.Now.AddDays(5), StartTime = TimeSpan.FromHours(15), EndTime = TimeSpan.FromHours(17), Location = "Training Hall", AdminApproval = true },
                    new Event { Title = "Workshop", Description = "Advanced techniques", Date = DateTime.Now.AddDays(10), StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(12), Location = "Community Center", AdminApproval = true },
                    new Event { Title = "Meeting", Description = "Quarterly review", Date = DateTime.Now.AddDays(3), StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(16), Location = "Conference Room A", AdminApproval = true },
                    new Event { Title = "Lecture", Description = "Guest speaker session", Date = DateTime.Now.AddDays(7), StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(11), Location = "Auditorium", AdminApproval = true },
                    new Event { Title = "Seminar", Description = "Career development", Date = DateTime.Now.AddDays(15), StartTime = TimeSpan.FromHours(13), EndTime = TimeSpan.FromHours(15), Location = "Library Hall", AdminApproval = true },
                    new Event { Title = "Networking", Description = "Professional networking event", Date = DateTime.Now.AddDays(20), StartTime = TimeSpan.FromHours(18), EndTime = TimeSpan.FromHours(20), Location = "Banquet Hall", AdminApproval = true },
                    new Event { Title = "Yoga Class", Description = "Beginner yoga session", Date = DateTime.Now.AddDays(1), StartTime = TimeSpan.FromHours(8), EndTime = TimeSpan.FromHours(9), Location = "Fitness Studio", AdminApproval = true },
                    new Event { Title = "Hackathon", Description = "24-hour coding competition", Date = DateTime.Now.AddDays(12), StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(17), Location = "Innovation Lab", AdminApproval = true },
                    new Event { Title = "Webinar", Description = "Online marketing strategies", Date = DateTime.Now.AddDays(6), StartTime = TimeSpan.FromHours(16), EndTime = TimeSpan.FromHours(17), Location = "Virtual (Zoom)", AdminApproval = true },
                    new Event { Title = "Art Exhibition", Description = "Local artists showcase", Date = DateTime.Now.AddDays(8), StartTime = TimeSpan.FromHours(11), EndTime = TimeSpan.FromHours(16), Location = "Gallery Room", AdminApproval = true },
                    new Event { Title = "Book Club", Description = "Monthly discussion", Date = DateTime.Now.AddDays(4), StartTime = TimeSpan.FromHours(19), EndTime = TimeSpan.FromHours(20), Location = "Library Meeting Room", AdminApproval = true },
                    new Event { Title = "Cooking Class", Description = "Italian cuisine basics", Date = DateTime.Now.AddDays(9), StartTime = TimeSpan.FromHours(18), EndTime = TimeSpan.FromHours(20), Location = "Kitchen Studio", AdminApproval = true },
                    new Event { Title = "Music Recital", Description = "Student performances", Date = DateTime.Now.AddDays(11), StartTime = TimeSpan.FromHours(17), EndTime = TimeSpan.FromHours(19), Location = "Concert Hall", AdminApproval = true },
                    new Event { Title = "Charity Run", Description = "5K marathon for a cause", Date = DateTime.Now.AddDays(14), StartTime = TimeSpan.FromHours(7), EndTime = TimeSpan.FromHours(10), Location = "City Park", AdminApproval = true },
                    new Event { Title = "Tech Talk", Description = "Future of AI", Date = DateTime.Now.AddDays(5), StartTime = TimeSpan.FromHours(14), EndTime = TimeSpan.FromHours(15), Location = "Tech Hub", AdminApproval = true },
                    new Event { Title = "Game Night", Description = "Board games and fun", Date = DateTime.Now.AddDays(2), StartTime = TimeSpan.FromHours(18), EndTime = TimeSpan.FromHours(21), Location = "Recreation Hall", AdminApproval = true },
                    new Event { Title = "Dance Class", Description = "Salsa for beginners", Date = DateTime.Now.AddDays(13), StartTime = TimeSpan.FromHours(19), EndTime = TimeSpan.FromHours(20), Location = "Dance Studio", AdminApproval = true },
                    new Event { Title = "Volunteer Meetup", Description = "Planning community cleanup", Date = DateTime.Now.AddDays(16), StartTime = TimeSpan.FromHours(10), EndTime = TimeSpan.FromHours(11), Location = "City Office", AdminApproval = true },
                    new Event { Title = "Film Screening", Description = "Classic cinema night", Date = DateTime.Now.AddDays(18), StartTime = TimeSpan.FromHours(20), EndTime = TimeSpan.FromHours(22), Location = "Outdoor Theater", AdminApproval = true },
                    new Event { Title = "Science Fair", Description = "Student projects showcase", Date = DateTime.Now.AddDays(21), StartTime = TimeSpan.FromHours(9), EndTime = TimeSpan.FromHours(13), Location = "Exhibition Hall", AdminApproval = true },
                    new Event { Title = "Photography Walk", Description = "Nature photography tips", Date = DateTime.Now.AddDays(17), StartTime = TimeSpan.FromHours(6), EndTime = TimeSpan.FromHours(8), Location = "Nature Trail", AdminApproval = true }

                );
                context.SaveChanges();
            }

            if (!context.Attendance.Any())
            {
                context.Attendance.AddRange(
                    new Attendance { UserId = context.Users.First(u => u.FirstName == "John").Id, Date = DateTime.Now.AddDays(1) },
                    new Attendance { UserId = context.Users.First(u => u.FirstName == "Jane").Id, Date = DateTime.Now.AddDays(2) },
                    new Attendance { UserId = context.Users.First(u => u.FirstName == "Alice").Id, Date = DateTime.Now.AddDays(3) },
                    new Attendance { UserId = context.Users.First(u => u.FirstName == "Bob").Id, Date = DateTime.Now.AddDays(4) },
                    new Attendance { UserId = context.Users.First(u => u.FirstName == "Charlie").Id, Date = DateTime.Now.AddDays(5) }
                );
                context.SaveChanges();
            }

            if (!context.EventAttendances.Any())
            {
                context.EventAttendances.AddRange(
                    new EventAttendance { UserId = context.Users.First(u => u.FirstName == "John").Id, EventId = context.Events.First(e => e.Title == "Meeting").Id, Rating = 5, Feedback = "Great event!" },
                    new EventAttendance { UserId = context.Users.First(u => u.FirstName == "Jane").Id, EventId = context.Events.First(e => e.Title == "Workshop").Id, Rating = 4, Feedback = "Informative session." },
                    new EventAttendance { UserId = context.Users.First(u => u.FirstName == "Alice").Id, EventId = context.Events.First(e => e.Title == "Conference").Id, Rating = 5, Feedback = "Very engaging." },
                    new EventAttendance { UserId = context.Users.First(u => u.FirstName == "Bob").Id, EventId = context.Events.First(e => e.Title == "Presentation").Id, Rating = 3, Feedback = "Could be better." },
                    new EventAttendance { UserId = context.Users.First(u => u.FirstName == "Charlie").Id, EventId = context.Events.First(e => e.Title == "Training").Id, Rating = 4, Feedback = "Well organized." }
                );
                context.SaveChanges();
            }

            if (!context.Admin.Any())
            {
                context.Admin.AddRange(
                    new Admin { Username = "admin1", Password = EncryptionHelper.EncryptPassword("adminpass1"), Email = "admin1@example.com" },
                    new Admin { Username = "admin2", Password = EncryptionHelper.EncryptPassword("adminpass2"), Email = "admin2@example.com" },
                    new Admin { Username = "admin3", Password = EncryptionHelper.EncryptPassword("adminpass3"), Email = "admin3@example.com" },
                    new Admin { Username = "admin4", Password = EncryptionHelper.EncryptPassword("adminpass4"), Email = "admin4@example.com" },
                    new Admin { Username = "admin5", Password = EncryptionHelper.EncryptPassword("adminpass5"), Email = "admin5@example.com" }
                );
                context.SaveChanges();
            }
        }
    }
}
