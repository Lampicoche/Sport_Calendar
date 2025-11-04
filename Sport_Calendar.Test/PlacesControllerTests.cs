using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

using Sport_Calendar.Controllers;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Infrastructure.Repositories;

namespace Prueba.Test;

public class PlacesControllerTests
{
    private static AppDbContext CreateDb()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(conn).Options;
        var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task Create_Saves_Place_And_Redirects_To_Events()
    {
        using var db = CreateDb();
        IPlaceRepository repo = new EfPlaceRepository(db);
        var controller = new PlacesController(repo);

        var place = new Place { Name = "City Arena", City = "Seville", Capacity = 20000, TicketPrice = 25, Vip = false };
        var result = await controller.Create(place);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Create", redirect.ActionName);
        Assert.Equal("Events", redirect.ControllerName);

        Assert.True(await db.Places.AnyAsync(p => p.Name == "City Arena"));
    }
}
