using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditAppointmentLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctors_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment");

            migrationBuilder.AddColumn<int>(
                name: "ActivePolicyId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SlotDurationMinutes",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PolicyId",
                table: "Appointment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RescheduleCount",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Appointment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DoctorPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: true),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    AllowPatientCancellation = table.Column<bool>(type: "bit", nullable: false),
                    MinCancellationHours = table.Column<int>(type: "int", nullable: false),
                    AllowLateCancellationReschedule = table.Column<bool>(type: "bit", nullable: false),
                    MaxRescheduleAttempts = table.Column<int>(type: "int", nullable: false),
                    AllowRescheduling = table.Column<bool>(type: "bit", nullable: false),
                    MinRescheduleHours = table.Column<int>(type: "int", nullable: false),
                    AllowFullRefund = table.Column<bool>(type: "bit", nullable: false),
                    AllowPartialRefund = table.Column<bool>(type: "bit", nullable: false),
                    PartialRefundPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    RequirePrePayment = table.Column<bool>(type: "bit", nullable: false),
                    UnpaidReservationTimeoutMinutes = table.Column<int>(type: "int", nullable: false),
                    AllowMultipleBookingsPerDay = table.Column<bool>(type: "bit", nullable: false),
                    MaxBookingsPerPatientPerDay = table.Column<int>(type: "int", nullable: false),
                    AllowLastMinuteBooking = table.Column<bool>(type: "bit", nullable: false),
                    MinBookingAdvanceHours = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorPolicies_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scheduleExceptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scheduleExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_scheduleExceptions_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "DoctorPolicies",
                columns: new[] { "Id", "AllowFullRefund", "AllowLastMinuteBooking", "AllowLateCancellationReschedule", "AllowMultipleBookingsPerDay", "AllowPartialRefund", "AllowPatientCancellation", "AllowRescheduling", "CreatedAt", "DoctorId", "IsDefault", "MaxBookingsPerPatientPerDay", "MaxRescheduleAttempts", "MinBookingAdvanceHours", "MinCancellationHours", "MinRescheduleHours", "PartialRefundPercentage", "RequirePrePayment", "UnpaidReservationTimeoutMinutes" },
                values: new object[] { 1, true, true, true, false, true, true, true, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, true, 1, 1, 2, 24, 12, 50m, true, 30 });

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_ActivePolicyId",
                table: "Doctors",
                column: "ActivePolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PolicyId",
                table: "Appointment",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorPolicies_DoctorId",
                table: "DoctorPolicies",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_scheduleExceptions_DoctorId",
                table: "scheduleExceptions",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_DoctorPolicies_PolicyId",
                table: "Appointment",
                column: "PolicyId",
                principalTable: "DoctorPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctors_DoctorId",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_DoctorPolicies_ActivePolicyId",
                table: "Doctors",
                column: "ActivePolicyId",
                principalTable: "DoctorPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_DoctorPolicies_PolicyId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Doctors_DoctorId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment");

            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_DoctorPolicies_ActivePolicyId",
                table: "Doctors");

            migrationBuilder.DropTable(
                name: "DoctorPolicies");

            migrationBuilder.DropTable(
                name: "scheduleExceptions");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_ActivePolicyId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Appointment_PolicyId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "ActivePolicyId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "SlotDurationMinutes",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "PolicyId",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "RescheduleCount",
                table: "Appointment");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointment");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Doctors_DoctorId",
                table: "Appointment",
                column: "DoctorId",
                principalTable: "Doctors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointment_Patients_PatientId",
                table: "Appointment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
