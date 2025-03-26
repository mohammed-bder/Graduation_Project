using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class GovernorateIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
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

            migrationBuilder.AlterColumn<int>(
                name: "GovernorateId",
                table: "clinics",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_clinics_governorates_GovernorateId",
                table: "clinics",
                column: "GovernorateId",
                principalTable: "governorates",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
