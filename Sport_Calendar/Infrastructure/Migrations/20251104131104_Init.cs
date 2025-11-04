using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sport_Calendar.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Place",
                columns: table => new
                {
                    id_place = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    place = table.Column<string>(type: "TEXT", nullable: false),
                    city = table.Column<string>(type: "TEXT", nullable: true),
                    capacity = table.Column<int>(type: "INTEGER", nullable: true),
                    ticket_price = table.Column<decimal>(type: "TEXT", nullable: true),
                    vip = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Place", x => x.id_place);
                });

            migrationBuilder.CreateTable(
                name: "Sport",
                columns: table => new
                {
                    id_sport = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    individual = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sport", x => x.id_sport);
                });

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    id_team = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    id_sport = table.Column<int>(type: "INTEGER", nullable: false),
                    id_place = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.id_team);
                    table.ForeignKey(
                        name: "FK_Team_Place_id_place",
                        column: x => x.id_place,
                        principalTable: "Place",
                        principalColumn: "id_place",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Team_Sport_id_sport",
                        column: x => x.id_sport,
                        principalTable: "Sport",
                        principalColumn: "id_sport",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    id_event = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    event_date = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    event_time = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    sport_id = table.Column<int>(type: "INTEGER", nullable: false),
                    place_id = table.Column<int>(type: "INTEGER", nullable: false),
                    local_team = table.Column<int>(type: "INTEGER", nullable: true),
                    visit_team = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.id_event);
                    table.ForeignKey(
                        name: "FK_Event_Place_place_id",
                        column: x => x.place_id,
                        principalTable: "Place",
                        principalColumn: "id_place",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Sport_sport_id",
                        column: x => x.sport_id,
                        principalTable: "Sport",
                        principalColumn: "id_sport",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Team_local_team",
                        column: x => x.local_team,
                        principalTable: "Team",
                        principalColumn: "id_team",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Event_Team_visit_team",
                        column: x => x.visit_team,
                        principalTable: "Team",
                        principalColumn: "id_team",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_local_team",
                table: "Event",
                column: "local_team");

            migrationBuilder.CreateIndex(
                name: "IX_Event_place_id",
                table: "Event",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_sport_id",
                table: "Event",
                column: "sport_id");

            migrationBuilder.CreateIndex(
                name: "IX_Event_visit_team",
                table: "Event",
                column: "visit_team");

            migrationBuilder.CreateIndex(
                name: "IX_Team_id_place",
                table: "Team",
                column: "id_place");

            migrationBuilder.CreateIndex(
                name: "IX_Team_id_sport",
                table: "Team",
                column: "id_sport");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Place");

            migrationBuilder.DropTable(
                name: "Sport");
        }
    }
}
