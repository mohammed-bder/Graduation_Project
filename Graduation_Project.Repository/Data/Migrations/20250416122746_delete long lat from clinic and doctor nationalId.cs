using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class deletelonglatfromclinicanddoctornationalId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "NationalID",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "clinics");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "clinics");

            migrationBuilder.AlterColumn<int>(
                name: "GovernorateId",
                table: "clinics",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics");

            migrationBuilder.AddColumn<string>(
                name: "NationalID",
                table: "Doctors",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GovernorateId",
                table: "clinics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "clinics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "clinics",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
