using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace CalendifyApp.Filters
{
    public class AdminFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check if the admin is logged in by verifying the session key
            if (context.HttpContext.Session.GetString("AdminLoggedIn") is null)
            {
                // Set response status to 401 (Unauthorized) and provide a plain text message
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("You are not authorized. Admin access is required.");
                return;
            }

            // Proceed to the next action if authorized
            await next();
        }
    }
}
