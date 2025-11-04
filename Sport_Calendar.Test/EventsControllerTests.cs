using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Xunit;

using Sport_Calendar.Controllers;
using Sport_Calendar.Controllers.Api; // LookupsController
using Sport_Calendar.ViewModels;

using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;

using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Infrastructure.Repositories;
using Sport_Calendar.Application.Services;
using Sport_Calendar.Application.Dtos;
using System.Linq;

namespace Prueba.Test;

public class EventsControllerTests
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
    public async Task Index_Filters_By_Sport()
    {
        using var db = CreateDb();

        // seed
        var football = new Sport { Name = "Football" };
        var tennis = new Sport { Name = "Tennis" };
        var place = new Place { Name = "Arena" };
        db.AddRange(football, tennis, place);
        await db.SaveChangesAsync();

        db.Events.AddRange(
            new Event { EventDate = new DateOnly(2025, 1, 1), EventTime = new TimeOnly(18, 0), SportId = football.Id, PlaceId = place.Id },
            new Event { EventDate = new DateOnly(2025, 1, 2), EventTime = new TimeOnly(18, 0), SportId = tennis.Id, PlaceId = place.Id }
        );
        await db.SaveChangesAsync();

        // wiring repos/services
        IEventRepository evRepo = new EfEventRepository(db);
        ISportRepository spRepo = new EfSportRepository(db);
        IPlaceRepository plRepo = new EfPlaceRepository(db);
        ITeamRepository tmRepo = new EfTeamRepository(db);

        IEventService evSvc = new EventService(evRepo);
        ILookupService lookupSvc = new LookupService(spRepo, plRepo, tmRepo);

        var controller = new EventsController(evSvc, lookupSvc);
        var filter = new EventFilterVm { SportId = football.Id };

        var result = await controller.Index(filter);
        var view = Assert.IsType<ViewResult>(result);

        var list = Assert.IsAssignableFrom<List<Event>>(view.ViewData["Events"]);
        Assert.All(list, e => Assert.Equal(football.Id, e.SportId));
    }

    [Fact]
    public async Task Api_Lookups_Teams_Returns_Only_Requested_Sport()
    {
        using var db = CreateDb();

        var football = new Sport { Name = "Football" };
        var tennis = new Sport { Name = "Tennis" };
        db.AddRange(football, tennis);
        await db.SaveChangesAsync();

        db.Teams.AddRange(
            new Team { Name = "Sevilla", SportId = football.Id },
            new Team { Name = "Madrid", SportId = football.Id },
            new Team { Name = "Nadal", SportId = tennis.Id }
        );
        await db.SaveChangesAsync();

        // wiring repos/services
        ITeamRepository teamRepo = new EfTeamRepository(db);
        ISportRepository sportRepo = new EfSportRepository(db);
        IPlaceRepository placeRepo = new EfPlaceRepository(db);
        ILookupService lookups = new LookupService(sportRepo, placeRepo, teamRepo);

        // test the API controller that serves teams by sport
        var api = new LookupsController(lookups);
        var result = await api.GetTeams(football.Id);

        var list = Assert.IsAssignableFrom<IEnumerable<LookupItemDto>>(result);
        Assert.Equal(2, list.Count()); // only the 2 football teams
    }
}
