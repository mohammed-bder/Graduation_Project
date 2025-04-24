using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class editpharmacyentity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePharmacies_Medicines_MedicineId",
                table: "MedicinePharmacies");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePharmacies_Pharmacies_PharmacyId",
                table: "MedicinePharmacies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicinePharmacies",
                table: "MedicinePharmacies");

            migrationBuilder.RenameTable(
                name: "MedicinePharmacies",
                newName: "PharmacyMedicineStocks");

            migrationBuilder.RenameIndex(
                name: "IX_MedicinePharmacies_PharmacyId",
                table: "PharmacyMedicineStocks",
                newName: "IX_PharmacyMedicineStocks_PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_MedicinePharmacies_MedicineId",
                table: "PharmacyMedicineStocks",
                newName: "IX_PharmacyMedicineStocks_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PharmacyMedicineStocks",
                table: "PharmacyMedicineStocks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyMedicineStocks_Medicines_MedicineId",
                table: "PharmacyMedicineStocks",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PharmacyMedicineStocks_Pharmacies_PharmacyId",
                table: "PharmacyMedicineStocks",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyMedicineStocks_Medicines_MedicineId",
                table: "PharmacyMedicineStocks");

            migrationBuilder.DropForeignKey(
                name: "FK_PharmacyMedicineStocks_Pharmacies_PharmacyId",
                table: "PharmacyMedicineStocks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PharmacyMedicineStocks",
                table: "PharmacyMedicineStocks");

            migrationBuilder.RenameTable(
                name: "PharmacyMedicineStocks",
                newName: "MedicinePharmacies");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyMedicineStocks_PharmacyId",
                table: "MedicinePharmacies",
                newName: "IX_MedicinePharmacies_PharmacyId");

            migrationBuilder.RenameIndex(
                name: "IX_PharmacyMedicineStocks_MedicineId",
                table: "MedicinePharmacies",
                newName: "IX_MedicinePharmacies_MedicineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicinePharmacies",
                table: "MedicinePharmacies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MedicinePharmacies_Medicines_MedicineId",
                table: "MedicinePharmacies",
                column: "MedicineId",
                principalTable: "Medicines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicinePharmacies_Pharmacies_PharmacyId",
                table: "MedicinePharmacies",
                column: "PharmacyId",
                principalTable: "Pharmacies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
