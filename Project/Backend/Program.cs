using CalendifyApp.Models;
using CalendifyApp.Seeders; // Import the seeder namespace
using CalendifyApp.Services;
using CalendifyApp.Repositories.Interfaces;
using CalendifyApp.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

// Register Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventAttendanceRepository, EventAttendanceRepository>();
builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();

// Register Services
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IEventAttendanceService, EventAttendanceService>();
builder.Services.AddScoped<IOfficeAttendanceService, OfficeAttendanceService>();
builder.Logging.AddConsole();
builder.Services.AddDistributedMemoryCache(); // For session storage
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Calendify.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(10); // Set session timeout
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None; // Allow cookies across origins
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Force the Secure attribute
});

builder.Services.AddDbContext<MyContext>(options =>
    options.UseSqlite("Data Source=calendify.db"));


var app = builder.Build();

// Seed the database during application startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MyContext>();
    context.Database.EnsureCreated(); // Ensure the database is created
    DatabaseSeeder.Seed(context);    // Call the seeder to populate the database
}

app.UseCors();

app.UseSession();

app.MapControllers();

app.Urls.Add("http://localhost:5001");

app.MapGet("/hi", () => "Hello pleps!");

// Run the application
app.Run();
