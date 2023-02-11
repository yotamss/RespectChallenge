using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Respect.Server.Migrations
{
    /// <inheritdoc />
    public partial class _0002owneremail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "Petitions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "Petitions");
        }
    }
}
