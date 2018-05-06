using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Server.Migrations
{
    public partial class initialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clans",
                columns: table => new
                {
                    id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(nullable: true),
                    tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PasswordHash = table.Column<byte[]>(maxLength: 64, nullable: false),
                    clanId = table.Column<uint>(nullable: true),
                    email = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: false),
                    permission = table.Column<byte>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_clans_clanId",
                        column: x => x.clanId,
                        principalTable: "clans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "bans",
                columns: table => new
                {
                    id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ip = table.Column<int>(nullable: false),
                    mac = table.Column<string>(nullable: true),
                    reason = table.Column<string>(nullable: true),
                    userId = table.Column<uint>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bans", x => x.id);
                    table.ForeignKey(
                        name: "FK_bans_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "logins",
                columns: table => new
                {
                    userId = table.Column<uint>(nullable: false),
                    ip = table.Column<int>(nullable: false),
                    mac = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logins", x => x.userId);
                    table.ForeignKey(
                        name: "FK_logins_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bans_userId",
                table: "bans",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_users_clanId",
                table: "users",
                column: "clanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bans");

            migrationBuilder.DropTable(
                name: "logins");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "clans");
        }
    }
}
