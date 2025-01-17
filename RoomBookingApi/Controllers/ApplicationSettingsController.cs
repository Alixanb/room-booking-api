using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RoomBookingApi.Models;

namespace RoomBookingApi.Controllers
{
    [ApiController]
    [Route("appsettings")]
    public class ApplicationSettingsController : ControllerBase
    {
        private readonly ILogger<ApplicationSettingsController> _logger;
        private readonly ApplicationSettings _options;

        public ApplicationSettingsController(ILogger<ApplicationSettingsController> logger,
            IOptions<ApplicationSettings> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        [HttpGet]
        public IActionResult GetAppSettings()
        {
            _logger.LogInformation("Get application settings");
            return Ok(new {
                _options?.ApiName,
                _options?.Version
            });
        }
    }
}