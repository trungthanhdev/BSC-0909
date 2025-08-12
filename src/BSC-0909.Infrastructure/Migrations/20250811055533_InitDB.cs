using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSC_0909.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cryptoCurrencyEntities",
                columns: table => new
                {
                    ccid = table.Column<string>(type: "text", nullable: false),
                    currencyName = table.Column<string>(type: "text", nullable: false),
                    OpenPrice = table.Column<double>(type: "double precision", nullable: false),
                    HighPrice = table.Column<double>(type: "double precision", nullable: false),
                    LowPrice = table.Column<double>(type: "double precision", nullable: false),
                    ClosePrice = table.Column<double>(type: "double precision", nullable: false),
                    timeStamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cryptoCurrencyEntities", x => x.ccid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cryptoCurrencyEntities");
        }
    }
}
