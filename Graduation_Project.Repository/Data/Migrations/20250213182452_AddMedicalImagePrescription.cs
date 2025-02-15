using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicalImagePrescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MedicalImage",
                table: "MedicalHistories",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MedicalImage",
                table: "MedicalHistories");
        }
    }
}
