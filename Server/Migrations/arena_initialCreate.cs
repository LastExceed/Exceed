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
                    X = table.Column<long>(nullable: false),
                    Y = table.Column<long>(nullable: false),
                    Z = table.Column<long>(nullable: false)
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
