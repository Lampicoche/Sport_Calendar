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
        //city: City where the venue is located
        //capacity: Maximum number of spectators the venue can accommodate
        //ticket_price: Price of the ticket to attend the event at the venue
        //vip: Indicates whether the venue has VIP areas or services
        b.Entity<Place>(e => {
            e.ToTable("Place");
            e.Property(x => x.Id).HasColumnName("id_place").IsRequired();
            e.Property(x => x.Name).HasColumnName("place").IsRequired();
            e.Property(x => x.City).HasColumnName("city");
            e.Property(x => x.Capacity).HasColumnName("capacity");
            e.Property(x => x.TicketPrice).HasColumnName("ticket_price");
            e.Property(x => x.Vip).HasColumnName("vip");
        });
        //Define different sports of the events
        //id_sport: primary key
        //name: Name of the sport
        //description: Description of the sport, rules, characteristics, etc.
        //individual: Indicates whether the sport is individual or team-based

        b.Entity<Sport>(e => {
            e.ToTable("Sport");
            e.Property(x => x.Id).HasColumnName("id_sport");
            e.Property(x => x.Name).HasColumnName("name").IsRequired();
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.Individual).HasColumnName("individual");
        });
        //Define different teams participating in the events
        //id_team: primary key
        //name: Name of the team or a individual athlete
        //id_sport: foreign key referencing the Sport table
        //id_place: foreign key referencing the Place table 
        
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
        //Define the sports events
        //id_event: primary key
        //event_date: Date when the event takes place
        //event_time: Time when the event takes place
        //description: Description of the event, details, type of competition, etc.
        //id_sport: foreign key referencing the Sport table
        //id_place: foreign key referencing the Place table
        //local_team: foreign key referencing the Team table for the home team
        //visit_team: foreign key referencing the Team table for the visiting team
        

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
