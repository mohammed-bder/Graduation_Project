using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class MedicinePrescriptiondetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dosage",
                table: "Prescription");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Prescription");

            migrationBuilder.RenameColumn(
                name: "MedicationDetails",
                table: "Prescription",
                newName: "Diagnoses");

            migrationBuilder.AddColumn<int>(
                name: "Details",
                table: "MedicinePrescription",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Details",
                table: "MedicinePrescription");

            migrationBuilder.RenameColumn(
                name: "Diagnoses",
                table: "Prescription",
                newName: "MedicationDetails");

            migrationBuilder.AddColumn<string>(
                name: "Dosage",
                table: "Prescription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Prescription",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
