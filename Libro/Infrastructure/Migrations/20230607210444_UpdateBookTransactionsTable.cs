using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookTransactionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Fine",
                table: "BookTransactions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.UpdateData(
                table: "BookTransactions",
                keyColumn: "BookTransactionId",
                keyValue: 1,
                column: "Fine",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "BookTransactions",
                keyColumn: "BookTransactionId",
                keyValue: 2,
                column: "Fine",
                value: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Fine",
                table: "BookTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldDefaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "BookTransactions",
                keyColumn: "BookTransactionId",
                keyValue: 1,
                column: "Fine",
                value: 0);

            migrationBuilder.UpdateData(
                table: "BookTransactions",
                keyColumn: "BookTransactionId",
                keyValue: 2,
                column: "Fine",
                value: 0);
        }
    }
}
