using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherAppCore.Migrations
{
    public partial class MyWeatherMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Time = table.Column<TimeSpan>(type: "time", nullable: false),
                    T = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Humidity = table.Column<int>(nullable: false),
                    Td = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Pressure = table.Column<int>(nullable: false),
                    Direction = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Speed = table.Column<int>(nullable: false),
                    Cloudiness = table.Column<int>(nullable: false),
                    h = table.Column<int>(nullable: false),
                    VV = table.Column<int>(nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherData", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherData");
        }
    }
}
