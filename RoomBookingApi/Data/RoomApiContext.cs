using Microsoft.EntityFrameworkCore;
using RoomBookingApi.Models;

namespace RoomBookingApi.Data
{
    public class RoomApiContext : DbContext
    {
        public RoomApiContext()
        {
        }

        public RoomApiContext(DbContextOptions<RoomApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Room> Rooms { get; set; }
    }
}