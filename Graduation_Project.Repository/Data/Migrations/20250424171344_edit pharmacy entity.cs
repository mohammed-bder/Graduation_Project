using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class editpharmacyentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pharmacies_Pharmacists_PharmacistId",
                table: "Pharmacies");

            migrationBuilder.DropTable(
                name: "MedicineMedicinePharmacy");

            migrationBuilder.DropTable(
                name: "MedicinePharmacyPharmacy");

            migrationBuilder.DropTable(
                name: "Pharmacists");

            migrationBuilder.DropIndex(
                name: "IX_Pharmacies_PharmacistId",
                table: "Pharmacies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicinePharmacies",
                table: "MedicinePharmacies");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "PharmacistId",
                table: "Pharmacies");

            migrationBuilder.RenameColumn(
                name: "PharmacyLicensePictureUrl",
                table: "Pharmacies",
                newName: "LicenseImageUrl");

            migrationBuilder.RenameColumn(
                name: "Longtude",
                table: "Pharmacies",
                newName: "Longitude");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeliverDate",
                table: "PharmacyOrders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeliveryAddress",
                table: "PharmacyOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "PharmacyOrders",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserID",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfilePictureUrl",
                table: "Pharmacies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MedicinePharmacies",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicinePharmacies",
                table: "MedicinePharmacies",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PharmacyContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PharmacyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PharmacyContacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PharmacyContacts_Pharmacies_PharmacyId",
                        column: x => x.PharmacyId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePharmacies_MedicineId",
                table: "MedicinePharmacies",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePharmacies_PharmacyId",
                table: "MedicinePharmacies",
                column: "PharmacyId");

            migrationBuilder.CreateIndex(
                name: "IX_PharmacyContacts_PharmacyId",
                table: "PharmacyContacts",
                column: "PharmacyId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePharmacies_Medicines_MedicineId",
                table: "MedicinePharmacies");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicinePharmacies_Pharmacies_PharmacyId",
                table: "MedicinePharmacies");

            migrationBuilder.DropTable(
                name: "PharmacyContacts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MedicinePharmacies",
                table: "MedicinePharmacies");

            migrationBuilder.DropIndex(
                name: "IX_MedicinePharmacies_MedicineId",
                table: "MedicinePharmacies");

            migrationBuilder.DropIndex(
                name: "IX_MedicinePharmacies_PharmacyId",
                table: "MedicinePharmacies");

            migrationBuilder.DropColumn(
                name: "DeliverDate",
                table: "PharmacyOrders");

            migrationBuilder.DropColumn(
                name: "DeliveryAddress",
                table: "PharmacyOrders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "PharmacyOrders");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "ApplicationUserID",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "ProfilePictureUrl",
                table: "Pharmacies");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MedicinePharmacies");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Pharmacies",
                newName: "Longtude");

            migrationBuilder.RenameColumn(
                name: "LicenseImageUrl",
                table: "Pharmacies",
                newName: "PharmacyLicensePictureUrl");

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Pharmacies",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PharmacistId",
                table: "Pharmacies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MedicinePharmacies",
                table: "MedicinePharmacies",
                columns: new[] { "MedicineId", "PharmacyId" });

            migrationBuilder.CreateTable(
                name: "MedicineMedicinePharmacy",
                columns: table => new
                {
                    MedicinesId = table.Column<int>(type: "int", nullable: false),
                    MedicinePharmaciesMedicineId = table.Column<int>(type: "int", nullable: false),
                    MedicinePharmaciesPharmacyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicineMedicinePharmacy", x => new { x.MedicinesId, x.MedicinePharmaciesMedicineId, x.MedicinePharmaciesPharmacyId });
                    table.ForeignKey(
                        name: "FK_MedicineMedicinePharmacy_MedicinePharmacies_MedicinePharmaciesMedicineId_MedicinePharmaciesPharmacyId",
                        columns: x => new { x.MedicinePharmaciesMedicineId, x.MedicinePharmaciesPharmacyId },
                        principalTable: "MedicinePharmacies",
                        principalColumns: new[] { "MedicineId", "PharmacyId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicineMedicinePharmacy_Medicines_MedicinesId",
                        column: x => x.MedicinesId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicinePharmacyPharmacy",
                columns: table => new
                {
                    PharmaciesId = table.Column<int>(type: "int", nullable: false),
                    MedicinePharmaciesMedicineId = table.Column<int>(type: "int", nullable: false),
                    MedicinePharmaciesPharmacyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicinePharmacyPharmacy", x => new { x.PharmaciesId, x.MedicinePharmaciesMedicineId, x.MedicinePharmaciesPharmacyId });
                    table.ForeignKey(
                        name: "FK_MedicinePharmacyPharmacy_MedicinePharmacies_MedicinePharmaciesMedicineId_MedicinePharmaciesPharmacyId",
                        columns: x => new { x.MedicinePharmaciesMedicineId, x.MedicinePharmaciesPharmacyId },
                        principalTable: "MedicinePharmacies",
                        principalColumns: new[] { "MedicineId", "PharmacyId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicinePharmacyPharmacy_Pharmacies_PharmaciesId",
                        column: x => x.PharmaciesId,
                        principalTable: "Pharmacies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pharmacists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NationalID = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    PharmacistLicensePictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pharmacists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pharmacies_PharmacistId",
                table: "Pharmacies",
                column: "PharmacistId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineMedicinePharmacy_MedicinePharmaciesMedicineId_MedicinePharmaciesPharmacyId",
                table: "MedicineMedicinePharmacy",
                columns: new[] { "MedicinePharmaciesMedicineId", "MedicinePharmaciesPharmacyId" });

            migrationBuilder.CreateIndex(
                name: "IX_MedicinePharmacyPharmacy_MedicinePharmaciesMedicineId_MedicinePharmaciesPharmacyId",
                table: "MedicinePharmacyPharmacy",
                columns: new[] { "MedicinePharmaciesMedicineId", "MedicinePharmaciesPharmacyId" });

            migrationBuilder.AddForeignKey(
                name: "FK_Pharmacies_Pharmacists_PharmacistId",
                table: "Pharmacies",
                column: "PharmacistId",
                principalTable: "Pharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
