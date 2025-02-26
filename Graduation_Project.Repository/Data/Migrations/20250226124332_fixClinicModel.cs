using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixClinicModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location",
                table: "clinics",
                newName: "Address");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "clinics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "clinics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "clinics");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "clinics",
                newName: "Location");
        }
    }
}
