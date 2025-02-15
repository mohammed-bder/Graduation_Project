using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExprianceForDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperianceYears",
                table: "Doctors",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperianceYears",
                table: "Doctors");
        }
    }
}
