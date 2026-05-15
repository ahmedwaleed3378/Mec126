using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mec126.Migrations
{
    /// <inheritdoc />
    public partial class SeedProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Electronics", "High performance laptop for gaming and development.", "Gaming Laptop", 1200.99m },
                    { 2, "Electronics", "Noise cancelling over-ear wireless headphones.", "Wireless Headphones", 149.50m },
                    { 3, "Furniture", "Ergonomic office chair with lumbar support.", "Office Chair", 320.00m },
                    { 4, "Accessories", "RGB mechanical keyboard with blue switches.", "Mechanical Keyboard", 89.99m },
                    { 5, "Wearables", "Fitness tracking smart watch with AMOLED display.", "Smart Watch", 210.75m },
                    { 6, "Home Appliances", "Automatic coffee maker with timer feature.", "Coffee Maker", 65.25m },
                    { 7, "Sports", "Lightweight running shoes for daily workouts.", "Running Shoes", 95.00m },
                    { 8, "Electronics", "Portable waterproof Bluetooth speaker.", "Bluetooth Speaker", 55.49m },
                    { 9, "Bags", "Durable backpack suitable for travel and work.", "Backpack", 40.00m },
                    { 10, "Furniture", "LED desk lamp with adjustable brightness.", "Desk Lamp", 27.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
