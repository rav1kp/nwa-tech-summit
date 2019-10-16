using Microsoft.AspNetCore.Builder;
using System.Collections.Generic;

namespace Helium
{
    public static class HomePageMiddlewareExtensions
    {
        static readonly HashSet<string> validPaths = new HashSet<string> { "/", "/index.html", "/index.htm", "/default.html", "/default.htm" };
        static readonly byte[] responseBytes = System.Text.Encoding.UTF8.GetBytes("Under construction ...");

        /// <summary>
        /// Middleware extension method to handle home page request
        /// </summary>
        /// <param name="builder">this IApplicationBuilder</param>
        /// <returns></returns>
        public static IApplicationBuilder UseHomePage(this IApplicationBuilder builder)
        {

            // create the middleware
            builder.Use(async (context, next) =>
            {
                // matches / or index.htm[l] or default.htm[l]
                if (validPaths.Contains(context.Request.Path.Value.ToLower()))
                {
                    // return the content
                    context.Response.ContentType = "text/html";
                    await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
                else
                {
                    // not a match, so call next middleware handler
                    await next();
                }
            });

            return builder;
        }
    }
}
