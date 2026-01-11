using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRental.Migrations
{
    /// <inheritdoc />
    public partial class RemovePropertiesRentalTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentAmout",
                table: "Rentals");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Rentals");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PaymentAmout",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "TransactionId",
                table: "Rentals",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
