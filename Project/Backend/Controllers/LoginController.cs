using System.Text;
using Microsoft.AspNetCore.Mvc;
using CalendifyApp.Services;
using CalendifyApp.Models;
using CalendifyApp.Filters;

namespace CalendifyApp.Controllers;

[Route("api")]
public class LoginController : Controller
{
    private readonly ILoginService _loginService;

    public LoginController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpGet("SimulateAdminLogin")]
    public IActionResult SimulateAdminLogin()
    {
        HttpContext.Session.SetString("AdminLoggedIn", "admin1");
        return Ok("Simulated admin login successful.");
    }

    [HttpGet("SimulateUserLogin")]
    public IActionResult SimulateUserLogin()
    {
        HttpContext.Session.SetString("UserLoggedIn", "John");
        return Ok("Simulated user login successful.");
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginBody loginBody)
    {
        if (loginBody.Password == null || (loginBody.Username == null && loginBody.Email == null))
            return BadRequest("Invalid input");

        // Admin login
        if (!string.IsNullOrEmpty(loginBody.Email) &&
            _loginService.CheckPassword(loginBody.Email, loginBody.Password) == LoginStatus.Success)
        {
            HttpContext.Session.SetString("AdminLoggedIn", $"{loginBody.Username}");
            return Ok("Successfully logged in as admin.");
        }

        // User login
        if (!string.IsNullOrEmpty(loginBody.Email) &&
            _loginService.CheckUserPassword(loginBody.Email, loginBody.Password) == LoginStatus.Success)
        {
            HttpContext.Session.SetString("UserLoggedIn", $"{loginBody.Email}");
            return Ok("Successfully logged in as user.");
        }

        return Unauthorized("Invalid email, username, or password.");
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] User user)
    {
        if (user is null) return BadRequest("Invallid input");
        switch (_loginService.Register(user))
        {
            case 3:
                return BadRequest("Email already in use by another account, try 'forgot password'.");
            case 2:
                return BadRequest("Password must be at least 6 characters long.");
            case 1:
                return BadRequest("Please use a valid Email.");
            case 0:
                return Ok($"Succesfully Registerd {user}");
            default:
                return BadRequest("Something went wrong");
        }

    }

    [HttpGet("IsAdminLoggedIn")]
    public IActionResult IsAdminLoggedIn()
    {
        if (HttpContext.Session.GetString("AdminLoggedIn") is null)
        {
            return Unauthorized("You are not logged in as admin.");
        }
        return Ok($"{HttpContext.Session.GetString("AdminLoggedIn")}");
    }

    [HttpGet("IsUserLoggedIn")]
    public IActionResult IsUserLoggedIn()
    {
        var user = HttpContext.Session.GetString("UserLoggedIn");
        if (string.IsNullOrEmpty(user))
        {
            return Unauthorized("You are not logged in.");
        }
        return Ok($"Logged in as {user}");
    }

    [HttpGet("ForgotPassword")]
    public IActionResult ForgotPassword()
    {
        return Ok(_loginService.ForgotPassword());
    }


    [HttpPut("Password")]
    public IActionResult Password([FromBody] string email, [FromBody] string password)
    {
        if (_loginService.Password(email, password)) return Ok();
        return BadRequest();
    }

    [AuthorizationFilter]
    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserLoggedIn");
        return Ok("Logged out.");
    }

    [AdminFilter]
    [HttpGet("AdminLogout")]
    public IActionResult AdminLogout()
    {
        HttpContext.Session.Remove("AdminLoggedIn");
        return Ok("Logged out.");
    }
}

public class LoginBody
{
    public string? Username { get; set; }
    public string? Email { get; set; } // Added email for user login
    public string? Password { get; set; }
}
