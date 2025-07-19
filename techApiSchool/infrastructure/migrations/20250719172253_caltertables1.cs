using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace techApiSchool.infrastructure.migrations
{
    /// <inheritdoc />
    public partial class caltertables1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "audit");

            migrationBuilder.DropColumn(
                name: "FieldName",
                table: "audit");

            migrationBuilder.DropColumn(
                name: "NewValue",
                table: "audit");

            migrationBuilder.RenameColumn(
                name: "OldValue",
                table: "audit",
                newName: "Infovalue");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "students",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "students");

            migrationBuilder.RenameColumn(
                name: "Infovalue",
                table: "audit",
                newName: "OldValue");

            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "audit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FieldName",
                table: "audit",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NewValue",
                table: "audit",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
