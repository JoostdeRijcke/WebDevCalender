using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace CalendifyApp.Filters
{
    public class AuthorizationFilter : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {   
            var context = actionContext.HttpContext;

            // Check if the user is logged in by verifying the "UserLoggedIn" session key
            if (context.Session.GetString("UserLoggedIn") is null)
            {
                // Set response status to 401 (Unauthorized) and provide a plain text message
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("You are not logged in. Please log in to access this resource.");
                return;
            }

            // Proceed to the next action if the user is logged in
            await next();
        }
    }
}