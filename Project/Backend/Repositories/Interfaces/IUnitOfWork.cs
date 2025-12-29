namespace CalendifyApp.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Repository properties
        IEventRepository Events { get; }
        IEventAttendanceRepository EventAttendances { get; }
        IUserRepository Users { get; }
        IAdminRepository Admins { get; }
        IAttendanceRepository Attendances { get; }

        // Transactional operations
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
