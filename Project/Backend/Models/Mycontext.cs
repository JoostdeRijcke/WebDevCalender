using Microsoft.EntityFrameworkCore;

namespace CalendifyApp.Models
{
    public class MyContext : DbContext
    {
        private readonly IConfiguration configuration;

        public MyContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<EventAttendance> EventAttendances { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // EventAttendance: User FK
            modelBuilder.Entity<EventAttendance>()
                .HasOne(ea => ea.User)
                .WithMany(u => u.EventAttendances)
                .HasForeignKey(ea => ea.UserId);

            // EventAttendance: Event FK
            modelBuilder.Entity<EventAttendance>()
                .HasOne(ea => ea.Event)
                .WithMany(e => e.EventAttendances)
                .HasForeignKey(ea => ea.EventId);
        }
    }
}
