using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RoomBookingApi.Data;

namespace RoomBookingApi.Tests;

public class RoomBookingIntegrationTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
{
    private readonly RoomApiContext _context;
    private readonly HttpClient _client;

    public RoomBookingIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();

        using var scope = factory.Services.CreateScope();
        _context = scope.ServiceProvider.GetRequiredService<RoomApiContext>();
        _context.Database.EnsureDeleted();
        _context.Database.Migrate();
    }

    [Fact]
    public async Task GetRoomsShouldReturnAllRooms()
    {
        var response = await _client.GetAsync("/api/v1.0/room");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
            return;

        _client.Dispose();
        _context.Dispose();
    }
}