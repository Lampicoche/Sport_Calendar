using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

using Sport_Calendar.Controllers;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;

using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Infrastructure.Repositories;
using Sport_Calendar.Application.Services;

namespace Sport_Calendar.Test;

// It ensures that team creation is saved to the database
// and correctly redirects to the event creation screen.
public class TeamsControllerTests
{
    private static AppDbContext CreateDb()
    {
        var conn = new SqliteConnection("DataSource=:memory:");
        conn.Open();
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(conn)
            .Options;

        var db = new AppDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    [Fact]
    public async Task Create_InsertsTeam_AndRedirectsToEvents()
    {
        using var db = CreateDb();

        // Seed minimal data
        db.Sports.Add(new Sport { Name = "Football" });
        db.Places.Add(new Place { Name = "Stadium" });
        await db.SaveChangesAsync();

        // Wiring repositories and services
        ITeamRepository teamRepo = new EfTeamRepository(db);
        ISportRepository sportRepo = new EfSportRepository(db);
        IPlaceRepository placeRepo = new EfPlaceRepository(db);

        ITeamService teamSvc = new TeamService(teamRepo);
        ILookupService lookups = new LookupService(sportRepo, placeRepo, teamRepo);

        var controller = new TeamsController(teamSvc, lookups);

        var team = new Team { Name = "Sevilla FC", SportId = 1, PlaceId = 1 };

        var result = await controller.Create(team);

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Create", redirect.ActionName);
        Assert.Equal("Events", redirect.ControllerName);

        Assert.True(await db.Teams.AnyAsync(t => t.Name == "Sevilla FC"));
    }
}
