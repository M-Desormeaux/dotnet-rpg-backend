using Microsoft.EntityFrameworkCore.Migrations;

namespace NetRPG.Migrations
{
    public partial class SkillSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "ID", "Damage", "Name" },
                values: new object[,]
                {
                    { 1, 25, "Fireball" },
                    { 2, 15, "Acid Splash" },
                    { 3, 50, "Ice Storm" },
                    { 4, 90, "Meteor" },
                    { 5, 5, "Echo" },
                    { 6, 35, "Madness" },
                    { 7, 20, "Shock" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "ID",
                keyValue: 7);
        }
    }
}
