using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class addclinicpicture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PictureUrl",
                table: "clinics");

            migrationBuilder.CreateTable(
                name: "clinicPictures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clinicPictures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_clinicPictures_clinics_ClinicId",
                        column: x => x.ClinicId,
                        principalTable: "clinics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_clinicPictures_ClinicId",
                table: "clinicPictures",
                column: "ClinicId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "clinicPictures");

            migrationBuilder.AddColumn<string>(
                name: "PictureUrl",
                table: "clinics",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
