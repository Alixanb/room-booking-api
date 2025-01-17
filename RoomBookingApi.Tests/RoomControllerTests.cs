using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using RoomBookingApi.Controllers;
using RoomBookingApi.Data;
using RoomBookingApi.Models;

namespace RoomBookingApi.Tests;

public class RoomControllerTests
{
    [Fact]
    public void GetRoomsShouldReturnAllRooms()
    {
        // Arrange / Given
        var mockContext = new Mock<RoomApiContext>();
        mockContext.Setup(context => context.Rooms)
            .ReturnsDbSet(new List<Room>
            {
                new()
                {
                    Area = 12.3m,
                    Capacity = 3,
                    Id = 1,
                    IsAccessible = false,
                    Name = "Room 1"
                }
            });

        var mockLogger = new Mock<ILogger<RoomController>>();
        var roomController = new RoomController(mockContext.Object,
        mockLogger.Object);

        // Act / When
        var allRooms = roomController.GetRooms();

        // Assert / Then
        var result = GetObjectResultContent(allRooms);
        Assert.Equal(1, result?.Count());
    }

    private static T? GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)((ObjectResult)result.Result)?.Value;
    }
}
