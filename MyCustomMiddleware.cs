using System.Diagnostics;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace NETDAY1; 
public class MyCustomMiddleware
{
    private readonly RequestDelegate _next;

    public MyCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // var scheme = context.Request.Scheme;
        // var host = context.Request.Host;
        // var path = context.Request.Path;
        // var queryString = context.Request.QueryString;
        var headers = new Dictionary<string, string>();
        foreach (var item in context.Request.Headers)
        {
            headers.Add(item.Key, item.Value.ToString());
        }
        var reader = new StreamReader(context.Request.Body);
        var body = reader.ReadToEnd();
        var requestData = new 
        {
            Scheme = context.Request.Scheme,
            Host = context.Request.Host.ToString(),
            Path = context.Request.Path.ToString(),
            QueryString = context.Request.QueryString.ToString(),
            Body =  body,
            Headers =headers
        };
        
        using (StreamWriter writter = File.AppendText("file.text"))
        {
            var data = JsonSerializer.Serialize(requestData);
            await writter.WriteLineAsync(data);
        }
        await _next(context);
        // Call the next delegate/middleware in the pipeline.
    }
} 
public static class MyCustomMiddlewareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder builder) 
    {
        return builder.UseMiddleware<MyCustomMiddleware>();
    }
}