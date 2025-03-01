using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class SpecialtyAndSubMigratoin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "SubSpecialities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Specialties");

            migrationBuilder.AddColumn<string>(
                name: "Name_ar",
                table: "SubSpecialities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_en",
                table: "SubSpecialities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_ar",
                table: "Specialties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name_en",
                table: "Specialties",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_ar",
                table: "SubSpecialities");

            migrationBuilder.DropColumn(
                name: "Name_en",
                table: "SubSpecialities");

            migrationBuilder.DropColumn(
                name: "Name_ar",
                table: "Specialties");

            migrationBuilder.DropColumn(
                name: "Name_en",
                table: "Specialties");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "SubSpecialities",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Specialties",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
