using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_contactInfo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_contactNumbers_ClinicId_PhoneNumber",
                table: "contactNumbers");

            migrationBuilder.CreateIndex(
                name: "IX_contactNumbers_ClinicId",
                table: "contactNumbers",
                column: "ClinicId");

            migrationBuilder.CreateIndex(
                name: "IX_contactNumbers_PhoneNumber",
                table: "contactNumbers",
                column: "PhoneNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_contactNumbers_ClinicId",
                table: "contactNumbers");

            migrationBuilder.DropIndex(
                name: "IX_contactNumbers_PhoneNumber",
                table: "contactNumbers");

            migrationBuilder.CreateIndex(
                name: "IX_contactNumbers_ClinicId_PhoneNumber",
                table: "contactNumbers",
                columns: new[] { "ClinicId", "PhoneNumber" },
                unique: true);
        }
    }
}
