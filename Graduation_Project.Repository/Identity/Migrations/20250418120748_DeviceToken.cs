using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Graduation_Project.Repository.Identity.Migrations
{
    /// <inheritdoc />
    public partial class DeviceToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FcmToken",
                table: "AspNetUsers",
                newName: "DeviceToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeviceToken",
                table: "AspNetUsers",
                newName: "FcmToken");
        }
    }
}
