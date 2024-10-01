using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PasteNest.Areas.Identity.Data;
using PasteNest.Data;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

namespace PasteNest.Middleware
{
    public class AuthMiddleware
    {
        private readonly ILogger<AuthMiddleware> _logger;
        private readonly RequestDelegate _next;

        public AuthMiddleware(ILogger<AuthMiddleware> logger, RequestDelegate next)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, PasteNestContext dbContext)
        {
            SetUsernameSession(context, dbContext);
            await _next.Invoke(context);
        }

        private async void SetUsernameSession(HttpContext context, PasteNestContext dbContext)
        {
            if (context.User.Identity is null) return;

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Session.Remove("Username");
                return;
            }

            if (!context.Session.Get("Username").IsNullOrEmpty()) return;

            // Get Username from database
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return;

            var user = await dbContext.Users.Where(u => u.Id == userId)
                .Select(u => u.Nickname)
                .FirstOrDefaultAsync();

            if (user == null) return;

            context.Session.SetString("Username", user);
        }
    }
}

