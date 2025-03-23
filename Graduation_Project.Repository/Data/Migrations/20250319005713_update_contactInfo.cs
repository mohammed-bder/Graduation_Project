using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_contactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_contactNumbers",
                table: "contactNumbers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "contactNumbers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactNumbers",
                table: "contactNumbers",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_contactNumbers_ClinicId_PhoneNumber",
                table: "contactNumbers",
                columns: new[] { "ClinicId", "PhoneNumber" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_contactNumbers",
                table: "contactNumbers");

            migrationBuilder.DropIndex(
                name: "IX_contactNumbers_ClinicId_PhoneNumber",
                table: "contactNumbers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "contactNumbers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_contactNumbers",
                table: "contactNumbers",
                columns: new[] { "ClinicId", "PhoneNumber" });
        }
    }
}
