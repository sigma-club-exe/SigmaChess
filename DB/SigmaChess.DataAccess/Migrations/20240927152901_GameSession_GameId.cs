using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SigmaChess.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class GameSession_GameId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "InviteLink",
                table: "DbGameSession",
                newName: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameId",
                table: "DbGameSession",
                newName: "InviteLink");
        }
    }
}
