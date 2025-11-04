using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Xunit;

using Sport_Calendar.Controllers;
using Sport_Calendar.Domain.Models;
using Sport_Calendar.Infrastructure.Data;
using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Infrastructure.Repositories;

namespace Sport_Calendar.Test
{
    public class SportsControllerTests
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
        public async Task Create_Saves_Sport_And_Redirects_To_Events_Create()
        {
            using var db = CreateDb();

            ISportRepository repo = new EfSportRepository(db);
            var controller = new SportsController(repo);

            //  
            var http = new DefaultHttpContext();
            controller.ControllerContext = new ControllerContext { HttpContext = http };
            controller.TempData = new TempDataDictionary(http, new FakeTempDataProvider());
             

            var sport = new Sport { Name = "Tennis", Description = "Singles", Individual = true };

            var result = await controller.Create(sport);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Create", redirect.ActionName);
            Assert.Equal("Events", redirect.ControllerName);

            Assert.True(await db.Sports.AnyAsync(s => s.Name == "Tennis"));
        }
    }
}
