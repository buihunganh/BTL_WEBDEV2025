using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_WEBDEV2025.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsSpecialDeal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Category", "Description", "DiscountPrice", "ImageUrl", "IsFeatured", "IsSpecialDeal", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Men", "Premium running shoes with Air Max technology", null, "https://via.placeholder.com/300", true, false, "Air Max 270", 150m },
                    { 2, "Unisex", "Classic lifestyle shoes", 70m, "https://via.placeholder.com/300", true, true, "Air Force 1", 90m },
                    { 3, "Men", "High-performance running shoes", null, "https://via.placeholder.com/300", true, false, "Zoom Pegasus", 120m },
                    { 4, "Women", "Everyday running for women", null, "https://via.placeholder.com/300", true, false, "Revolution 6", 60m },
                    { 5, "Men", "Basketball lifestyle shoes", 45m, "https://via.placeholder.com/300", false, true, "Court Vision", 65m },
                    { 6, "Unisex", "Futuristic design sneakers", null, "https://via.placeholder.com/300", true, false, "React Element", 130m },
                    { 7, "Women", "Natural motion running shoes", 60m, "https://via.placeholder.com/300", false, true, "Free RN", 80m },
                    { 8, "Unisex", "Skateboarding classic", null, "https://via.placeholder.com/300", true, false, "Dunk Low", 100m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
