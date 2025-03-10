using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migratoins
{
    /// <inheritdoc />
    public partial class handleNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_Doctors_DoctorId",
                table: "notificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_Patients_patientId",
                table: "notificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_Pharmacists_pharmacistId",
                table: "notificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_notificationRecipients_secretaries_secretaryId",
                table: "notificationRecipients");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "notifications",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "secretaryId",
                table: "notificationRecipients",
                newName: "SecretaryId");

            migrationBuilder.RenameColumn(
                name: "pharmacistId",
                table: "notificationRecipients",
                newName: "PharmacistId");

            migrationBuilder.RenameColumn(
                name: "patientId",
                table: "notificationRecipients",
                newName: "PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_notificationRecipients_secretaryId",
                table: "notificationRecipients",
                newName: "IX_notificationRecipients_SecretaryId");

            migrationBuilder.RenameIndex(
                name: "IX_notificationRecipients_pharmacistId",
                table: "notificationRecipients",
                newName: "IX_notificationRecipients_PharmacistId");

            migrationBuilder.RenameIndex(
                name: "IX_notificationRecipients_patientId",
                table: "notificationRecipients",
                newName: "IX_notificationRecipients_PatientId");

            migrationBuilder.AlterColumn<int>(
                name: "SecretaryId",
                table: "notificationRecipients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PharmacistId",
                table: "notificationRecipients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PatientId",
                table: "notificationRecipients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "notificationRecipients",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "notificationRecipients",
                type: "nvarchar(max)",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "notificationRecipients");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "notifications",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "SecretaryId",
                table: "notificationRecipients",
                newName: "secretaryId");

            migrationBuilder.RenameColumn(
                name: "PharmacistId",
                table: "notificationRecipients",
                newName: "pharmacistId");

            migrationBuilder.RenameColumn(
                name: "PatientId",
                table: "notificationRecipients",
                newName: "patientId");

            migrationBuilder.RenameIndex(
                name: "IX_notificationRecipients_SecretaryId",
                table: "notificationRecipients",
                newName: "IX_notificationRecipients_secretaryId");

            migrationBuilder.RenameIndex(
                name: "IX_notificationRecipients_PharmacistId",
                table: "notificationRecipients",
                newName: "IX_notificationRecipients_pharmacistId");

            migrationBuilder.RenameIndex(
                name: "IX_notificationRecipients_PatientId",
                table: "notificationRecipients",
                newName: "IX_notificationRecipients_patientId");

            migrationBuilder.AlterColumn<int>(
                name: "secretaryId",
                table: "notificationRecipients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "pharmacistId",
                table: "notificationRecipients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "patientId",
                table: "notificationRecipients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DoctorId",
                table: "notificationRecipients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_Doctors_DoctorId",
                table: "notificationRecipients",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_Patients_patientId",
                table: "notificationRecipients",
                column: "patientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_Pharmacists_pharmacistId",
                table: "notificationRecipients",
                column: "pharmacistId",
                principalTable: "Pharmacists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notificationRecipients_secretaries_secretaryId",
                table: "notificationRecipients",
                column: "secretaryId",
                principalTable: "secretaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
