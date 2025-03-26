using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveGovernorateId2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics");

            migrationBuilder.DropIndex(
                name: "IX_clinics_GovernorateId",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "GovernorateId",
                table: "clinics");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GovernorateId",
                table: "clinics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_clinics_GovernorateId",
                table: "clinics",
                column: "GovernorateId");

            migrationBuilder.AddForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id");
        }
    }
}
