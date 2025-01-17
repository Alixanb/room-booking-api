using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApi.Data;
using RoomBookingApi.Models;

namespace RoomBookingApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RoomApiContext _context;
        private readonly ILogger<RoomController> _logger;

        public RoomController(RoomApiContext context,
            ILogger<RoomController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        public ActionResult<IEnumerable<Room>?> GetRooms()
        {
            _logger.LogInformation("Get all rooms");
            return Ok(_context.Rooms);
        }

        [HttpGet]
        [MapToApiVersion("2.0")]
        public ActionResult<IEnumerable<Room>> GetRoomsV2()
        {
            _logger.LogInformation("Get all rooms");
            return Ok(_context.Rooms.FirstOrDefault());
        }

        [HttpGet("{id:int}")]
        public ActionResult<Room> GetRoomById(int id)
        {
            _logger.LogInformation($"Get room {id}");
            return Ok(_context.Rooms.FirstOrDefault(room => room.Id == id));
        }

        [HttpPost]
        public ActionResult<Room> AddRoom(Room room)
        {
            _logger.LogInformation($"Add room {room.Name}");
            _context.Rooms.Add(room);
            _context.SaveChanges();
            return Created(nameof(AddRoom), room);
        }

        [HttpPut]
        public ActionResult<Room> UpdateRoom(Room newRoom)
        {
            _logger.LogInformation($"Update room {newRoom.Name}");
            var oldRoom = _context.Rooms.FirstOrDefault(room => room.Id == newRoom.Id);
            if (oldRoom == null)
                return NotFound(newRoom.Id);
            oldRoom.Area = newRoom.Area;
            oldRoom.Capacity = newRoom.Capacity;
            oldRoom.Name = newRoom.Name;
            oldRoom.IsAccessible = newRoom.IsAccessible;
            _context.SaveChanges();
            return Accepted(newRoom);
        }

        [HttpDelete]
        public ActionResult<Room> DeleteRoom(int id)
        {
            _logger.LogInformation($"Delete room {id}");
            var room = _context.Rooms.FirstOrDefault(room => room.Id == id);
            if (room == null)
                return NotFound(id);
            _context.Rooms.Remove(room);
            _context.SaveChanges();
            return Accepted();
        }
    }
}