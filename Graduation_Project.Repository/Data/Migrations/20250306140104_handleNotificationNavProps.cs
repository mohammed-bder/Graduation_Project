using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migratoins
{
    /// <inheritdoc />
    public partial class handleNotificationNavProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_Doctors_DoctorId",
                table: "notificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_Patients_PatientId",
                table: "notificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_Pharmacists_PharmacistId",
                table: "notificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_secretaries_SecretaryId",
                table: "notificationRecipients");

            migrationBuilder.DropIndex(
                name: "IX_notificationRecipients_DoctorId",
                table: "notificationRecipients");

            migrationBuilder.DropIndex(
                name: "IX_notificationRecipients_PatientId",
                table: "notificationRecipients");

            migrationBuilder.DropIndex(
                name: "IX_notificationRecipients_PharmacistId",
                table: "notificationRecipients");

            migrationBuilder.DropIndex(
                name: "IX_notificationRecipients_SecretaryId",
                table: "notificationRecipients");

            migrationBuilder.DropColumn(
                name: "DoctorId",
                table: "notificationRecipients");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "notificationRecipients");

            migrationBuilder.DropColumn(
                name: "PharmacistId",
                table: "notificationRecipients");

            migrationBuilder.DropColumn(
                name: "SecretaryId",
                table: "notificationRecipients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DoctorId",
                table: "notificationRecipients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "notificationRecipients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PharmacistId",
                table: "notificationRecipients",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SecretaryId",
                table: "notificationRecipients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_notificationRecipients_DoctorId",
                table: "notificationRecipients",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_notificationRecipients_PatientId",
                table: "notificationRecipients",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_notificationRecipients_PharmacistId",
                table: "notificationRecipients",
                column: "PharmacistId");

            migrationBuilder.CreateIndex(
                name: "IX_notificationRecipients_SecretaryId",
                table: "notificationRecipients",
                column: "SecretaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_Doctors_DoctorId",
                table: "notificationRecipients",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_Patients_PatientId",
                table: "notificationRecipients",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_Pharmacists_PharmacistId",
                table: "notificationRecipients",
                column: "PharmacistId",
                principalTable: "Pharmacists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_secretaries_SecretaryId",
                table: "notificationRecipients",
                column: "SecretaryId",
                principalTable: "secretaries",
                principalColumn: "Id");
        }
    }
}
