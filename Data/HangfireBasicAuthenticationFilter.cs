using Hangfire.Dashboard;
using System;
using System.Text;
using Microsoft.AspNetCore.Http;

public class HangfireBasicAuthenticationFilter : IDashboardAuthorizationFilter
{
    public string Username { get; set; }
    public string Password { get; set; }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var header = httpContext.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(header))
        {
            Challenge(httpContext);
            return false;
        }

        var authHeader = header.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase) ? header.Substring("Basic ".Length).Trim() : string.Empty;

        if (string.IsNullOrEmpty(authHeader))
        {
            Challenge(httpContext);
            return false;
        }

        var credentialBytes = Convert.FromBase64String(authHeader);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

        if (credentials.Length != 2 || credentials[0] != Username || credentials[1] != Password)
        {
            Challenge(httpContext);
            return false;
        }

        return true;
    }

    private void Challenge(HttpContext httpContext)
    {
        httpContext.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Hangfire Dashboard\"";
        httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
    }
}
