using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SigmaChess.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GameSession_GameStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "DbGameSession",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "DbGameSession");
        }
    }
}
