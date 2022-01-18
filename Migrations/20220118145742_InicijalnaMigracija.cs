using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebProgramiranjeProjekat.Migrations
{
    public partial class InicijalnaMigracija : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bolnica",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    BrMesta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bolnica", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Lekar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lekar", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Pacijent",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Prezime = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    JMBG = table.Column<string>(type: "nvarchar(13)", maxLength: 13, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacijent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BolnicaLekar",
                columns: table => new
                {
                    BolniceID = table.Column<int>(type: "int", nullable: false),
                    LekariID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BolnicaLekar", x => new { x.BolniceID, x.LekariID });
                    table.ForeignKey(
                        name: "FK_BolnicaLekar_Bolnica_BolniceID",
                        column: x => x.BolniceID,
                        principalTable: "Bolnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BolnicaLekar_Lekar_LekariID",
                        column: x => x.LekariID,
                        principalTable: "Lekar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecenje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pocetak = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Kraj = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SobaID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BolnicaID = table.Column<int>(type: "int", nullable: true),
                    PacijentID = table.Column<int>(type: "int", nullable: true),
                    LekarID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecenje", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Lecenje_Bolnica_BolnicaID",
                        column: x => x.BolnicaID,
                        principalTable: "Bolnica",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lecenje_Lekar_LekarID",
                        column: x => x.LekarID,
                        principalTable: "Lekar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Lecenje_Pacijent_PacijentID",
                        column: x => x.PacijentID,
                        principalTable: "Pacijent",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BolnicaLekar_LekariID",
                table: "BolnicaLekar",
                column: "LekariID");

            migrationBuilder.CreateIndex(
                name: "IX_Lecenje_BolnicaID",
                table: "Lecenje",
                column: "BolnicaID");

            migrationBuilder.CreateIndex(
                name: "IX_Lecenje_LekarID",
                table: "Lecenje",
                column: "LekarID");

            migrationBuilder.CreateIndex(
                name: "IX_Lecenje_PacijentID",
                table: "Lecenje",
                column: "PacijentID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BolnicaLekar");

            migrationBuilder.DropTable(
                name: "Lecenje");

            migrationBuilder.DropTable(
                name: "Bolnica");

            migrationBuilder.DropTable(
                name: "Lekar");

            migrationBuilder.DropTable(
                name: "Pacijent");
        }
    }
}
