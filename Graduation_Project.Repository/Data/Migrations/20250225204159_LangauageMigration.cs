using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class LangauageMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "regions",
                newName: "Name_en");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "governorates",
                newName: "Name_en");

            migrationBuilder.AddColumn<string>(
                name: "Name_ar",
                table: "regions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "MedicinePrescription",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Name_ar",
                table: "governorates",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name_ar",
                table: "regions");

            migrationBuilder.DropColumn(
                name: "Name_ar",
                table: "governorates");

            migrationBuilder.RenameColumn(
                name: "Name_en",
                table: "regions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Name_en",
                table: "governorates",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "MedicinePrescription",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
