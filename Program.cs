using ServiceAnalyzer.Application.Interfaces;
using ServiceAnalyzer.Application.Services;
using ServiceAnalyzer.Logging;
using ServiceAnalyzer.Infrastructure.Clients;

namespace ServiceAnalyzer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Logging.ClearProviders();
            builder.Logging.AddProvider(new FileLoggerProvider("/app/out.log"));

            // Add services to container
            builder.Services.AddControllers();

            // Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // HttpClient для Reddit
            builder.Services.AddHttpClient();

            // DI реєстрації
            builder.Services.AddScoped<RedditClient>();
            builder.Services.AddScoped<IRedditService, RedditService>();
            builder.Logging.AddProvider(
                new FileLoggerProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "logs", "out.log")
                )
            );

            builder.Services.AddHttpClient<RedditClient>(client =>
            {
                client.BaseAddress = new Uri("https://www.reddit.com/");
                client.DefaultRequestHeaders.Add("User-Agent", "ServiceAnalyzer/1.0 (Windows; .NET 8)");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            var app = builder.Build();

            app.UseExceptionHandler("/error");
            app.Map("/error", (HttpContext context) =>
            {
                return Results.Problem("Something went wrong");
            });

            // Middleware pipeline
            app.UseSwagger();
            app.UseSwaggerUI();

            //app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();

            app.Urls.Add("http://0.0.0.0:8080");
            app.Run();
        }
    }
}
