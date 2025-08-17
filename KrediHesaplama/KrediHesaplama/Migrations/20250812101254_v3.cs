using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KrediHesaplama.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KrediHesaplamaKayitlari",
                columns: table => new
                {
                    KayitId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreditTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tutar = table.Column<double>(type: "REAL", nullable: false),
                    Vade = table.Column<int>(type: "INTEGER", nullable: false),
                    FaizOrani = table.Column<double>(type: "REAL", nullable: false),
                    AylikOdeme = table.Column<double>(type: "REAL", nullable: false),
                    ToplamOdeme = table.Column<double>(type: "REAL", nullable: false),
                    HesaplamaTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrediHesaplamaKayitlari", x => x.KayitId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KrediHesaplamaKayitlari");
        }
    }
}
