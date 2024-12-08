using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Migrations
{
    /// <inheritdoc />
    public partial class SubSpecialityNullAble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubSpecialities_Specialties_SpecialtyId",
                table: "SubSpecialities");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialtyId",
                table: "SubSpecialities",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SubSpecialities_Specialties_SpecialtyId",
                table: "SubSpecialities",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubSpecialities_Specialties_SpecialtyId",
                table: "SubSpecialities");

            migrationBuilder.AlterColumn<int>(
                name: "SpecialtyId",
                table: "SubSpecialities",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubSpecialities_Specialties_SpecialtyId",
                table: "SubSpecialities",
                column: "SpecialtyId",
                principalTable: "Specialties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
