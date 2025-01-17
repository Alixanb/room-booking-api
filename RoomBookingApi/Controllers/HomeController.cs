using Microsoft.AspNetCore.Mvc;

namespace RoomBookingApi.Controllers
{
    [ApiController]
    [Route("/home")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string GetHome()
        // public IActionResult GetHome()
        {
            return "Hello World !";
            // return Ok("Hello World !");
        }
    }
}