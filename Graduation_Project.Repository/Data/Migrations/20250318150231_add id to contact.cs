using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class addidtocontact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "contactNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "contactNumbers");
        }
    }
}
