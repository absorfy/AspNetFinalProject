using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AspNetFinalProject.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConnectUserProfileToIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_AspNetUsers_IdentityId",
                table: "UserProfiles",
                column: "IdentityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_AspNetUsers_IdentityId",
                table: "UserProfiles");
        }
    }
}
