using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace techApiSchool.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class calterteacher1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Teachers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Teachers");
        }
    }
}
