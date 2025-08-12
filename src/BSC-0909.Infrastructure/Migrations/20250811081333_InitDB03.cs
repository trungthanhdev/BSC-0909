using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BSC_0909.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDB03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cryptoCurrencyM15Entities",
                columns: table => new
                {
                    ccid = table.Column<string>(type: "text", nullable: false),
                    currencyName = table.Column<string>(type: "text", nullable: false),
                    OpenPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    HighPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    LowPrice = table.Column<decimal>(type: "numeric", nullable: false),
                    ClosePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    timeStamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cryptoCurrencyM15Entities", x => x.ccid);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cryptoCurrencyM15Entities");
        }
    }
}
