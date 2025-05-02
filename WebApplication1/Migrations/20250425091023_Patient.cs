using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class Patient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Login",
                newName: "PasswordHash");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Registration",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Registration");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "Login",
                newName: "Password");
        }
    }
}
