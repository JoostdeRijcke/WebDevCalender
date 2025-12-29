using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalendifyApp.Services
{
    public class OfficeAttendanceService : IOfficeAttendanceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OfficeAttendanceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> IsDateBookedAsync(int userId, DateTime date)
        {
            return await _unitOfWork.Attendances.IsDateBookedAsync(userId, date);
        }

        public async Task AddAttendanceAsync(Attendance attendance)
        {
            await _unitOfWork.Attendances.AddAsync(attendance);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> RemoveAttendanceAsync(int userId, DateTime date)
        {
            var existingAttendance = await _unitOfWork.Attendances
                .GetAttendanceByUserAndDateAsync(userId, date);

            if (existingAttendance != null)
            {
                await _unitOfWork.Attendances.DeleteAsync(existingAttendance);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<int>> GetUserIdsByDateAsync(DateTime date)
        {
            var result = await _unitOfWork.Attendances.GetUserIdsByDateAsync(date);
            return result.ToList();
        }

        // public async Task<List<DateOnly>> GetAttendanceDatesByUserAsync(int userId)
        // {
        //     return await _context.Attendance
        //         .Where(a => a.UserId == userId)
        //         .Select(a => a.Date)
        //         .ToListAsync();
        // }
    }
}
