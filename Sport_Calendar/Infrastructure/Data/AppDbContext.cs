using Microsoft.EntityFrameworkCore;
using Sport_Calendar.Domain.Models;

namespace Sport_Calendar.Infrastructure.Data;
//Here we make database
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Place> Places => Set<Place>();
    public DbSet<Sport> Sports => Set<Sport>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<Event> Events => Set<Event>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        //Defiene where the sport event will take place
        //id_place: primary key 
        //place: Name of the venue where the event takes place
        b.Entity<Place>(e => {
            e.ToTable("Place");
            e.Property(x => x.Id).HasColumnName("id_place").IsRequired();
            e.Property(x => x.Name).HasColumnName("place").IsRequired();
            e.Property(x => x.City).HasColumnName("city");
            e.Property(x => x.Capacity).HasColumnName("capacity");
            e.Property(x => x.TicketPrice).HasColumnName("ticket_price");
            e.Property(x => x.Vip).HasColumnName("vip");
        });
        //Sport table
        b.Entity<Sport>(e => {
            e.ToTable("Sport");
            e.Property(x => x.Id).HasColumnName("id_sport");
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.Individual).HasColumnName("individual");
        });
        //Team table
        b.Entity<Team>(e => {
            e.ToTable("Team");
            e.Property(x => x.Id).HasColumnName("id_team");
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.SportId).HasColumnName("id_sport");
            e.Property(x => x.PlaceId).HasColumnName("id_place");
            e.HasOne(x => x.Sport).WithMany(s => s.Teams).HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Place).WithMany(p => p.Teams).HasForeignKey(x => x.PlaceId)
                .OnDelete(DeleteBehavior.SetNull);
        });
        //Event table
        b.Entity<Event>(e => {
            e.ToTable("Event");
            e.Property(x => x.Id).HasColumnName("id_event");
            e.Property(x => x.EventDate).HasColumnName("event_date");
            e.Property(x => x.EventTime).HasColumnName("event_time");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.SportId).HasColumnName("sport_id");
            e.Property(x => x.PlaceId).HasColumnName("place_id");
            e.Property(x => x.LocalTeamId).HasColumnName("local_team");
            e.Property(x => x.VisitTeamId).HasColumnName("visit_team");

            e.HasOne(x => x.Sport).WithMany(s => s.Events).HasForeignKey(x => x.SportId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Place).WithMany(p => p.Events).HasForeignKey(x => x.PlaceId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.LocalTeam).WithMany().HasForeignKey(x => x.LocalTeamId)
                .OnDelete(DeleteBehavior.SetNull);
            e.HasOne(x => x.VisitTeam).WithMany().HasForeignKey(x => x.VisitTeamId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
