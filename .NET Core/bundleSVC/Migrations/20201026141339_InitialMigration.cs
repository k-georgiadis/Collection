using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bundleSVC.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bundles",
                columns: table => new
                {
                    B_code = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    B_name = table.Column<string>(maxLength: 100, nullable: false),
                    B_price = table.Column<double>(nullable: false),
                    B_expdate = table.Column<DateTime>(nullable: false),
                    B_availdate = table.Column<DateTime>(nullable: false),
                    B_active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bundles", x => x.B_code);
                });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "B_code", "B_active", "B_availdate", "B_expdate", "B_name", "B_price" },
                values: new object[] { 1, true, new DateTime(2020, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 12, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "test1", 7.5 });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "B_code", "B_active", "B_availdate", "B_expdate", "B_name", "B_price" },
                values: new object[] { 2, true, new DateTime(2020, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2020, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "test2", 12.0 });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "B_code", "B_active", "B_availdate", "B_expdate", "B_name", "B_price" },
                values: new object[] { 3, true, new DateTime(2020, 12, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "test3", 11.0 });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "B_code", "B_active", "B_availdate", "B_expdate", "B_name", "B_price" },
                values: new object[] { 4, true, new DateTime(2020, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "test4", 13.0 });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "B_code", "B_active", "B_availdate", "B_expdate", "B_name", "B_price" },
                values: new object[] { 5, true, new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 2, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "test5", 5.0 });

            migrationBuilder.InsertData(
                table: "Bundles",
                columns: new[] { "B_code", "B_active", "B_availdate", "B_expdate", "B_name", "B_price" },
                values: new object[] { 6, true, new DateTime(2021, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2021, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test6", 8.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bundles");
        }
    }
}
