// Bootstraps the ASP.NET Core MVC app: registers services, configures routing,
// migrates the DB, and starts the server.

using Microsoft.EntityFrameworkCore;
using Sport_Calendar.Infrastructure.Data;

using Sport_Calendar.Application.Repositories;
using Sport_Calendar.Application.Services;
using Sport_Calendar.Infrastructure.Repositories;
 
 

var builder = WebApplication.CreateBuilder(args);

// Add MVC
builder.Services.AddControllersWithViews();

// EF Core (SQLite)
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"))
);

// Repositories
builder.Services.AddScoped<ITeamRepository, EfTeamRepository>();
builder.Services.AddScoped<IEventRepository, EfEventRepository>();
builder.Services.AddScoped<ISportRepository, EfSportRepository>();
builder.Services.AddScoped<IPlaceRepository, EfPlaceRepository>();

// Domain/Application services
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ILookupService, LookupService>();

var app = builder.Build();

// Error pages & HSTS only in Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// HTTP pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Conventional routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Events}/{action=Index}/{id?}"
);

// Attribute-routed controllers 
app.MapControllers();

// Apply pending EF Core migrations at startup 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    // await Seed.RunAsync(db)
}

app.Run();
