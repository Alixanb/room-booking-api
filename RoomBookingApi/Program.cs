using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using RoomBookingApi.Data;
using RoomBookingApi.Middlewares;
using RoomBookingApi.Models;
using Serilog;

namespace RoomBookingApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

            try
            {
                Log.Information("Starting app");

                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Room API V1"
                    });
                    opt.SwaggerDoc("v2", new OpenApiInfo
                    {
                        Version = "v2",
                        Title = "Room API V2"
                    });
                });
                builder.Services.AddControllers();
                builder.Services.AddDbContext<RoomApiContext>(options =>
                    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                );
                builder.Services.Configure<ApplicationSettings>(
                    builder.Configuration.GetSection("ApplicationSettings")
                );
                builder.Services.AddApiVersioning(options =>
                {
                    options.DefaultApiVersion = new ApiVersion(1, 0);
                    options.AssumeDefaultVersionWhenUnspecified = true;
                    options.ReportApiVersions = true;
                }).AddApiExplorer(options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    options.SubstituteApiVersionInUrl = true;
                }).AddMvc();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(setup =>
                    {
                        setup.SwaggerEndpoint("/swagger/v1/swagger.json", "Room API V1");
                        setup.SwaggerEndpoint("/swagger/v2/swagger.json", "Room API V2");
                    });
                }

                app.UseHttpsRedirection();
                app.UseMiddleware<ExceptionMiddleware>();
                app.MapControllers();
                // app.MapGet("/", () => "Hello World!");

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application failed unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
