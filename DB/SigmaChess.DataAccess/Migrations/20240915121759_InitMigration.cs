using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SigmaChess.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DbUserAuth",
                columns: table => new
                {
                    TgId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    TgUsername = table.Column<string>(type: "text", nullable: false),
                    AuthDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Avatar = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbUserAuth", x => x.TgId);
                });

            migrationBuilder.CreateTable(
                name: "DbGameSession",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InviteLink = table.Column<string>(type: "text", nullable: false),
                    Player1TgId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    Player2TgId = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DbGameSession", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DbGameSession_DbUserAuth_Player1TgId",
                        column: x => x.Player1TgId,
                        principalTable: "DbUserAuth",
                        principalColumn: "TgId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DbGameSession_DbUserAuth_Player2TgId",
                        column: x => x.Player2TgId,
                        principalTable: "DbUserAuth",
                        principalColumn: "TgId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DbGameSession_Player1TgId",
                table: "DbGameSession",
                column: "Player1TgId");

            migrationBuilder.CreateIndex(
                name: "IX_DbGameSession_Player2TgId",
                table: "DbGameSession",
                column: "Player2TgId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DbGameSession");

            migrationBuilder.DropTable(
                name: "DbUserAuth");
        }
    }
}
