using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KrediHesaplama.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BasvuruLoglari",
                columns: table => new
                {
                    basvurulogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tutar = table.Column<double>(type: "REAL", nullable: false),
                    Vade = table.Column<int>(type: "INTEGER", nullable: false),
                    isApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    isCancelled = table.Column<bool>(type: "INTEGER", nullable: false),
                    basvuruTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasvuruLoglari", x => x.basvurulogId);
                });

            migrationBuilder.CreateTable(
                name: "HesaplamaLoglari",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    table.PrimaryKey("PK_HesaplamaLoglari", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "KrediUrunleri",
                columns: table => new
                {
                    CreditTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    urunAdi = table.Column<string>(type: "TEXT", nullable: false),
                    faizOrani = table.Column<double>(type: "REAL", nullable: false),
                    minTutar = table.Column<double>(type: "REAL", nullable: false),
                    maxTutar = table.Column<double>(type: "REAL", nullable: false),
                    minVade = table.Column<int>(type: "INTEGER", nullable: false),
                    maxVade = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrediUrunleri", x => x.CreditTypeId);
                });

            migrationBuilder.CreateTable(
                name: "LoginLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    islemTuru = table.Column<string>(type: "TEXT", nullable: true),
                    Seviye = table.Column<string>(type: "TEXT", nullable: true),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginLogs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "SignupLogs",
                columns: table => new
                {
                    LogId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    TCKN = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignupLogs", x => x.LogId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TCKN = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false),
                    Sifre = table.Column<string>(type: "TEXT", nullable: true),
                    adSoyad = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "KrediBasvurulari",
                columns: table => new
                {
                    basvuruId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Tutar = table.Column<double>(type: "REAL", nullable: false),
                    Vade = table.Column<int>(type: "INTEGER", nullable: false),
                    IsApproved = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCancelled = table.Column<bool>(type: "INTEGER", nullable: false),
                    basvuruTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KrediBasvurulari", x => x.basvuruId);
                    table.ForeignKey(
                        name: "FK_KrediBasvurulari_KrediUrunleri_CreditTypeId",
                        column: x => x.CreditTypeId,
                        principalTable: "KrediUrunleri",
                        principalColumn: "CreditTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KrediBasvurulari_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "KrediUrunleri",
                columns: new[] { "CreditTypeId", "faizOrani", "maxTutar", "maxVade", "minTutar", "minVade", "urunAdi" },
                values: new object[,]
                {
                    { 1, 4.9900000000000002, 100000.0, 36, 10000.0, 3, "İhtiyaç" },
                    { 2, 2.9900000000000002, 4000000.0, 120, 300000.0, 3, "Konut" },
                    { 3, 3.9900000000000002, 1000000.0, 60, 100000.0, 3, "Taşıt" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_KrediBasvurulari_CreditTypeId",
                table: "KrediBasvurulari",
                column: "CreditTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_KrediBasvurulari_UserId",
                table: "KrediBasvurulari",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TCKN",
                table: "Users",
                column: "TCKN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasvuruLoglari");

            migrationBuilder.DropTable(
                name: "HesaplamaLoglari");

            migrationBuilder.DropTable(
                name: "KrediBasvurulari");

            migrationBuilder.DropTable(
                name: "LoginLogs");

            migrationBuilder.DropTable(
                name: "SignupLogs");

            migrationBuilder.DropTable(
                name: "KrediUrunleri");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
