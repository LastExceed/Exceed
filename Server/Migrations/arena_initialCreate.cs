using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Server.Migrations
{
    public partial class initialArenaCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {          
            migrationBuilder.CreateTable(
                name: "Arenas",
                columns: table => new
                {
                    Id = table.Column<uint>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ArenaId = table.Column<uint>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    SpawnPosition1 = table.Column<byte[]>(nullable: false), // Spawn position of Player 1
                    SpawnPosition2 = table.Column<byte[]>(nullable: false), // Spawn position of Player 2
                    SpawnPosition3 = table.Column<byte[]>(nullable: false) // Spawn position of Spectator
                },
                constraints: table =>
                   {
                       table.PrimaryKey("PK_Arenas", x => x.Id);
                   });             
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arenas");
        }
    }
}
