using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FabricPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFabricInspectionInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "observation",
                table: "fabric_inspections",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "roll_length",
                table: "fabric_inspections",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "shade_match",
                table: "fabric_inspections",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "weave_condition",
                table: "fabric_inspections",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "observation",
                table: "fabric_inspections");

            migrationBuilder.DropColumn(
                name: "roll_length",
                table: "fabric_inspections");

            migrationBuilder.DropColumn(
                name: "shade_match",
                table: "fabric_inspections");

            migrationBuilder.DropColumn(
                name: "weave_condition",
                table: "fabric_inspections");
        }
    }
}
