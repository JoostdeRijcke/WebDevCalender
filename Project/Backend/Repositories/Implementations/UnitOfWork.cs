using CalendifyApp.Models;
using CalendifyApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace CalendifyApp.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyContext _context;
        private IDbContextTransaction? _transaction;

        private IEventRepository? _events;
        private IEventAttendanceRepository? _eventAttendances;
        private IUserRepository? _users;
        private IAdminRepository? _admins;
        private IAttendanceRepository? _attendances;

        public UnitOfWork(MyContext context)
        {
            _context = context;
        }

        public IEventRepository Events => _events ??= new EventRepository(_context);
        public IEventAttendanceRepository EventAttendances => _eventAttendances ??= new EventAttendanceRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);
        public IAdminRepository Admins => _admins ??= new AdminRepository(_context);
        public IAttendanceRepository Attendances => _attendances ??= new AttendanceRepository(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}
