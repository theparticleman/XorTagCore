using System.Text;
using XorTag.Domain;

namespace XorTag;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException)
        {
            Console.WriteLine("Resource not found " + context.Request.Path);
            context.Response.StatusCode = 404;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("Resource not found"));
        }
        catch(Exception ex)
        {
            Console.WriteLine("An unexpected exeption was thrown");
            Console.WriteLine(ex);
            context.Response.StatusCode = 500;
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes("An unexpected error occurred"));
        }
    }
}