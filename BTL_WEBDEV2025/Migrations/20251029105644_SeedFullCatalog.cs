using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BTL_WEBDEV2025.Migrations
{
    /// <inheritdoc />
    public partial class SeedFullCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Color", "ProductId", "Size", "StockQuantity" },
                values: new object[,]
                {
                    { 3, "Black", 1, "42", 10 },
                    { 4, "Red/Black", 2, "42", 12 },
                    { 5, "Blue/White", 2, "43", 8 },
                    { 6, "Panda", 3, "40", 30 },
                    { 7, "Panda", 3, "41", 25 },
                    { 8, "Black", 4, "41", 25 },
                    { 9, "Black", 4, "42", 20 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "BrandId", "Category", "CategoryId", "Description", "DiscountPrice", "ImageUrl", "IsFeatured", "IsSpecialDeal", "Name", "Price" },
                values: new object[,]
                {
                    { 5, 1, "", 4, "Retro basketball-inspired design.", null, "/images/products/nike_blazer.jpg", false, false, "Nike Blazer Mid '77", 110m },
                    { 6, 1, "", 1, "Training stability and grip.", null, "/images/products/nike_metcon9.jpg", false, false, "Nike Metcon 9", 140m },
                    { 7, 1, "", 2, "Natural feel with lightweight support.", null, "/images/products/nike_freerun.jpg", false, false, "Nike Free Run 5.0", 120m },
                    { 8, 1, "", 1, "Giannis signature shoe.", null, "/images/products/nike_freak5.jpg", false, false, "Nike Zoom Freak 5", 135m },
                    { 9, 1, "", 4, "Inspired by '90s streetwear.", null, "/images/products/nike_huarache.jpg", false, false, "Nike Air Huarache", 125m },
                    { 10, 1, "", 1, "Super fast race-day running shoes.", null, "/images/products/nike_vaporfly3.jpg", false, false, "Nike Vaporfly 3", 250m },
                    { 11, 2, "", 2, "Comfort and energy return.", null, "/images/products/adidas_ultraboost1.jpg", false, false, "Adidas Ultraboost 1.0", 180m },
                    { 12, 2, "", 4, "Timeless tennis silhouette.", null, "/images/products/adidas_stansmith.jpg", false, false, "Adidas Stan Smith", 95m },
                    { 13, 2, "", 4, "Iconic shell-toe sneaker.", null, "/images/products/adidas_superstar.jpg", false, false, "Adidas Superstar", 90m },
                    { 14, 2, "", 1, "Urban comfort meets modern tech.", null, "/images/products/adidas_nmdr1.jpg", false, false, "Adidas NMD_R1", 150m },
                    { 15, 2, "", 4, "Retro indoor soccer classic.", null, "/images/products/adidas_samba.jpg", false, false, "Adidas Samba OG", 100m },
                    { 16, 3, "", 4, "Chunky sneaker trendsetter.", null, "/images/products/balenciaga_triples.jpg", false, false, "Balenciaga Triple S", 950m },
                    { 17, 3, "", 2, "Sock-like futuristic silhouette.", null, "/images/products/balenciaga_speed.jpg", false, false, "Balenciaga Speed 2.0", 850m },
                    { 18, 3, "", 4, "Technical, layered trail sneaker.", null, "/images/products/balenciaga_track.jpg", false, false, "Balenciaga Track", 895m },
                    { 19, 3, "", 1, "Oversized sole futuristic design.", null, "/images/products/balenciaga_defender.jpg", false, false, "Balenciaga Defender", 1100m },
                    { 20, 4, "", 2, "Signature red sole and spikes.", null, "/images/products/louboutin_vieira.jpg", false, false, "Louboutin Vieira Spikes", 850m },
                    { 21, 4, "", 1, "Luxury leather low-top sneakers.", null, "/images/products/louboutin_louis.jpg", false, false, "Louboutin Louis Junior", 995m },
                    { 22, 5, "", 4, "Retro-futuristic design.", null, "/images/products/puma_rsx.jpg", false, false, "Puma RS-X", 120m },
                    { 23, 5, "", 4, "Timeless streetwear sneaker.", null, "/images/products/puma_suede.jpg", false, false, "Puma Suede Classic", 80m },
                    { 24, 5, "", 1, "Comfort with vintage look.", null, "/images/products/puma_futurerider.jpg", false, false, "Puma Future Rider", 90m },
                    { 25, 5, "", 2, "Bold sole, soft leather upper.", null, "/images/products/puma_cali.jpg", false, false, "Puma Cali Dream", 100m },
                    { 26, 6, "", 4, "Revived '80s basketball icon.", null, "/images/products/nb_550.jpg", false, false, "New Balance 550", 110m },
                    { 27, 6, "", 1, "Premium running performance.", null, "/images/products/nb_990v6.jpg", false, false, "New Balance 990v6", 200m },
                    { 28, 6, "", 1, "Max comfort daily runner.", null, "/images/products/nb_1080v13.jpg", false, false, "New Balance Fresh Foam X 1080v13", 160m },
                    { 29, 6, "", 4, "Retro running-inspired lifestyle shoe.", null, "/images/products/nb_327.jpg", false, false, "New Balance 327", 120m },
                    { 30, 7, "", 1, "Stability with premium comfort.", null, "/images/products/asics_kayano30.jpg", false, false, "ASICS Gel-Kayano 30", 160m },
                    { 31, 7, "", 2, "Cushioning and cloud-like ride.", null, "/images/products/asics_nimbus26.jpg", false, false, "ASICS Gel-Nimbus 26", 170m },
                    { 32, 7, "", 1, "Everyday running reliability.", null, "/images/products/asics_gt2000.jpg", false, false, "ASICS GT-2000 12", 150m },
                    { 33, 8, "", 4, "Clean retro court sneaker.", null, "/images/products/reebok_clubc85.jpg", false, false, "Reebok Club C 85", 85m },
                    { 34, 8, "", 4, "Heritage style and soft leather.", null, "/images/products/reebok_classic.jpg", false, false, "Reebok Classic Leather", 90m },
                    { 35, 8, "", 1, "Functional training shoe.", null, "/images/products/reebok_nanox4.jpg", false, false, "Reebok Nano X4", 140m },
                    { 36, 8, "", 2, "Energy-return performance sneaker.", null, "/images/products/reebok_zig.jpg", false, false, "Reebok Zig Kinetica 2.5", 130m }
                });

            migrationBuilder.InsertData(
                table: "ProductVariants",
                columns: new[] { "Id", "Color", "ProductId", "Size", "StockQuantity" },
                values: new object[,]
                {
                    { 10, "White/Red", 5, "42", 15 },
                    { 11, "White/Red", 5, "43", 10 },
                    { 12, "Grey/Volt", 6, "43", 8 },
                    { 13, "Pink Foam", 7, "38", 12 },
                    { 14, "Pink Foam", 7, "39", 10 },
                    { 15, "Blue/Gold", 8, "44", 10 },
                    { 16, "Black/White", 9, "40", 30 },
                    { 17, "Triple Black", 9, "41", 15 },
                    { 18, "White/Red", 10, "42", 7 },
                    { 19, "Cloud White", 11, "37", 18 },
                    { 20, "Cloud White", 11, "38", 15 },
                    { 21, "White/Green", 12, "41", 40 },
                    { 22, "White/Green", 12, "42", 35 },
                    { 23, "White/Black", 13, "40", 50 },
                    { 24, "White/Black", 13, "41", 45 },
                    { 25, "Core Black", 14, "42", 20 },
                    { 26, "White/Black", 15, "39", 40 },
                    { 27, "White/Black", 15, "40", 38 },
                    { 28, "Beige", 16, "41", 5 },
                    { 29, "Black", 16, "42", 4 },
                    { 30, "Black/White", 17, "38", 7 },
                    { 31, "Grey/Blue", 18, "42", 6 },
                    { 32, "Black", 19, "43", 3 },
                    { 33, "White/Silver", 20, "37", 4 },
                    { 34, "Black", 21, "42", 5 },
                    { 35, "Navy", 21, "43", 3 },
                    { 36, "Grey/Yellow", 22, "41", 12 },
                    { 37, "Black/White", 23, "40", 30 },
                    { 38, "Red/White", 23, "41", 15 },
                    { 39, "Blue/Orange", 24, "42", 18 },
                    { 40, "White/Pink", 25, "38", 20 },
                    { 41, "White/Green", 26, "41", 20 },
                    { 42, "White/Grey", 26, "42", 15 },
                    { 43, "Grey", 27, "42.5", 9 },
                    { 44, "Grey", 27, "43", 7 },
                    { 45, "White/Blue", 28, "42", 14 },
                    { 46, "Multi", 29, "39", 16 },
                    { 47, "Navy/Orange", 29, "40", 14 },
                    { 48, "Black/Lime", 30, "42", 14 },
                    { 49, "Black/Lime", 30, "43", 11 },
                    { 50, "Pink/Blue", 31, "38", 10 },
                    { 51, "Blue/Orange", 32, "41.5", 13 },
                    { 52, "White/Green", 33, "40", 22 },
                    { 53, "White/Green", 33, "41", 20 },
                    { 54, "White/Gum", 34, "42", 18 },
                    { 55, "Black/Red", 35, "43", 9 },
                    { 56, "Purple/Pink", 36, "37", 11 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 50);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 51);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 52);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 53);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 54);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 55);

            migrationBuilder.DeleteData(
                table: "ProductVariants",
                keyColumn: "Id",
                keyValue: 56);

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

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 36);
        }
    }
}
