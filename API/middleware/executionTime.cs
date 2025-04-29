using System.Text;
using System.Text.Json;

namespace API.middleware;

public class ExecutionTime
{
    private readonly RequestDelegate _next;

    public ExecutionTime(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.Now;
        var originalBodyStream = context.Response.Body;

        try
        {
            using var memoryStream = new MemoryStream();
            context.Response.Body = memoryStream;

            // Execute the rest of the pipeline
            await _next(context);

            var endTime = DateTime.Now;
            var executionTime = (endTime - startTime).TotalSeconds;

            memoryStream.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(memoryStream).ReadToEndAsync();

            if (!string.IsNullOrEmpty(responseBody) && 
                context.Response.ContentType?.ToLower().Contains("application/json") == true)
            {
                var jsonDocument = JsonDocument.Parse(responseBody);
                var outputStream = new MemoryStream();
                
                using (var jsonWriter = new Utf8JsonWriter(outputStream))
                {
                    jsonWriter.WriteStartObject();

                    // Copy existing properties
                    foreach (var element in jsonDocument.RootElement.EnumerateObject())
                    {
                        element.WriteTo(jsonWriter);
                    }

                    // Add execution time
                    jsonWriter.WriteNumber("executionTimeSeconds", executionTime);
                    jsonWriter.WriteEndObject();
                }

                var modifiedJson = Encoding.UTF8.GetString(outputStream.ToArray());
                
                memoryStream.SetLength(0);
                await using var writer = new StreamWriter(memoryStream, leaveOpen: true);
                await writer.WriteAsync(modifiedJson);
                await writer.FlushAsync();
            }

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(originalBodyStream);
        }
        finally
        {
            context.Response.Body = originalBodyStream;
        }
    }
}

// Extension method to make it easier to add the middleware
public static class ExecutionTimeExtensions
{
    public static IApplicationBuilder UseExecutionTime(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExecutionTime>();
    }
}
