using BTL_WEBDEV2025.Models;
using Microsoft.EntityFrameworkCore;

namespace BTL_WEBDEV2025.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<Brand> Brands => Set<Brand>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure currency precision to avoid truncation warnings and rounding issues
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Product>()
                .Property(p => p.DiscountPrice)
                .HasPrecision(18, 2);

            // precision
            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<OrderDetail>()
                .Property(d => d.UnitPrice)
                .HasPrecision(18, 2);

            // seed roles
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Customer" }
            );

            // seed categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Men" },
                new Category { Id = 2, Name = "Women" },
                new Category { Id = 3, Name = "Kid" },
                new Category { Id = 4, Name = "Unisex" }
            );

            // seed user admin
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Email = "admin@nike.com", PasswordHash = "admin123", FullName = "Admin Account", PhoneNumber = "123456", RoleId = 1 }
            );

            // seed brands
            modelBuilder.Entity<Brand>().HasData(
                new Brand { Id = 1, Name = "Nike" },
                new Brand { Id = 2, Name = "Adidas" },
                new Brand { Id = 3, Name = "Balenciaga" },
                new Brand { Id = 4, Name = "Louboutin" },
                new Brand { Id = 5, Name = "Puma" },
                new Brand { Id = 6, Name = "New Balance" },
                new Brand { Id = 7, Name = "ASICS" },
                new Brand { Id = 8, Name = "Reebok" }
            );

            // seed products (full 36 items)
            modelBuilder.Entity<Product>().HasData(
                // Nike (1-10)
                new Product { Id = 1, Name = "Nike Air Force 1 '07", Description = "Classic leather and heritage style.", Price = 110m, ImageUrl = "/images/products/nike_airforce1.jpg", CategoryId = 4, BrandId = 1, IsFeatured = true },
                new Product { Id = 2, Name = "Nike Air Max 270", Description = "Big Air unit comfort.", Price = 150m, DiscountPrice = 120m, ImageUrl = "/images/products/nike_airmax270.jpg", CategoryId = 1, BrandId = 1, IsSpecialDeal = true },
                new Product { Id = 3, Name = "Nike Dunk Low Retro", Description = "Street icon returns with retro colors.", Price = 100m, ImageUrl = "/images/products/nike_dunklow.jpg", CategoryId = 4, BrandId = 1 },
                new Product { Id = 4, Name = "Nike Air Zoom Pegasus 40", Description = "Everyday responsive running shoes.", Price = 130m, ImageUrl = "/images/products/nike_pegasus40.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 5, Name = "Nike Blazer Mid '77", Description = "Retro basketball-inspired design.", Price = 110m, ImageUrl = "/images/products/nike_blazer.jpg", CategoryId = 4, BrandId = 1 },
                new Product { Id = 6, Name = "Nike Metcon 9", Description = "Training stability and grip.", Price = 140m, ImageUrl = "/images/products/nike_metcon9.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 7, Name = "Nike Free Run 5.0", Description = "Natural feel with lightweight support.", Price = 120m, ImageUrl = "/images/products/nike_freerun.jpg", CategoryId = 2, BrandId = 1 },
                new Product { Id = 8, Name = "Nike Zoom Freak 5", Description = "Giannis signature shoe.", Price = 135m, ImageUrl = "/images/products/nike_freak5.jpg", CategoryId = 1, BrandId = 1 },
                new Product { Id = 9, Name = "Nike Air Huarache", Description = "Inspired by '90s streetwear.", Price = 125m, ImageUrl = "/images/products/nike_huarache.jpg", CategoryId = 4, BrandId = 1 },
                new Product { Id = 10, Name = "Nike Vaporfly 3", Description = "Super fast race-day running shoes.", Price = 250m, ImageUrl = "/images/products/nike_vaporfly3.jpg", CategoryId = 1, BrandId = 1 },

                // Adidas (11-15)
                new Product { Id = 11, Name = "Adidas Ultraboost 1.0", Description = "Comfort and energy return.", Price = 180m, ImageUrl = "/images/products/adidas_ultraboost1.jpg", CategoryId = 2, BrandId = 2 },
                new Product { Id = 12, Name = "Adidas Stan Smith", Description = "Timeless tennis silhouette.", Price = 95m, ImageUrl = "/images/products/adidas_stansmith.jpg", CategoryId = 4, BrandId = 2 },
                new Product { Id = 13, Name = "Adidas Superstar", Description = "Iconic shell-toe sneaker.", Price = 90m, ImageUrl = "/images/products/adidas_superstar.jpg", CategoryId = 4, BrandId = 2 },
                new Product { Id = 14, Name = "Adidas NMD_R1", Description = "Urban comfort meets modern tech.", Price = 150m, ImageUrl = "/images/products/adidas_nmdr1.jpg", CategoryId = 1, BrandId = 2 },
                new Product { Id = 15, Name = "Adidas Samba OG", Description = "Retro indoor soccer classic.", Price = 100m, ImageUrl = "/images/products/adidas_samba.jpg", CategoryId = 4, BrandId = 2 },

                // Balenciaga (16-19)
                new Product { Id = 16, Name = "Balenciaga Triple S", Description = "Chunky sneaker trendsetter.", Price = 950m, ImageUrl = "/images/products/balenciaga_triples.jpg", CategoryId = 4, BrandId = 3 },
                new Product { Id = 17, Name = "Balenciaga Speed 2.0", Description = "Sock-like futuristic silhouette.", Price = 850m, ImageUrl = "/images/products/balenciaga_speed.jpg", CategoryId = 2, BrandId = 3 },
                new Product { Id = 18, Name = "Balenciaga Track", Description = "Technical, layered trail sneaker.", Price = 895m, ImageUrl = "/images/products/balenciaga_track.jpg", CategoryId = 4, BrandId = 3 },
                new Product { Id = 19, Name = "Balenciaga Defender", Description = "Oversized sole futuristic design.", Price = 1100m, ImageUrl = "/images/products/balenciaga_defender.jpg", CategoryId = 1, BrandId = 3 },

                // Louboutin (20-21)
                new Product { Id = 20, Name = "Louboutin Vieira Spikes", Description = "Signature red sole and spikes.", Price = 850m, ImageUrl = "/images/products/louboutin_vieira.jpg", CategoryId = 2, BrandId = 4 },
                new Product { Id = 21, Name = "Louboutin Louis Junior", Description = "Luxury leather low-top sneakers.", Price = 995m, ImageUrl = "/images/products/louboutin_louis.jpg", CategoryId = 1, BrandId = 4 },

                // Puma (22-25)
                new Product { Id = 22, Name = "Puma RS-X", Description = "Retro-futuristic design.", Price = 120m, ImageUrl = "/images/products/puma_rsx.jpg", CategoryId = 4, BrandId = 5 },
                new Product { Id = 23, Name = "Puma Suede Classic", Description = "Timeless streetwear sneaker.", Price = 80m, ImageUrl = "/images/products/puma_suede.jpg", CategoryId = 4, BrandId = 5 },
                new Product { Id = 24, Name = "Puma Future Rider", Description = "Comfort with vintage look.", Price = 90m, ImageUrl = "/images/products/puma_futurerider.jpg", CategoryId = 1, BrandId = 5 },
                new Product { Id = 25, Name = "Puma Cali Dream", Description = "Bold sole, soft leather upper.", Price = 100m, ImageUrl = "/images/products/puma_cali.jpg", CategoryId = 2, BrandId = 5 },

                // New Balance (26-29)
                new Product { Id = 26, Name = "New Balance 550", Description = "Revived '80s basketball icon.", Price = 110m, ImageUrl = "/images/products/nb_550.jpg", CategoryId = 4, BrandId = 6 },
                new Product { Id = 27, Name = "New Balance 990v6", Description = "Premium running performance.", Price = 200m, ImageUrl = "/images/products/nb_990v6.jpg", CategoryId = 1, BrandId = 6 },
                new Product { Id = 28, Name = "New Balance Fresh Foam X 1080v13", Description = "Max comfort daily runner.", Price = 160m, ImageUrl = "/images/products/nb_1080v13.jpg", CategoryId = 1, BrandId = 6 },
                new Product { Id = 29, Name = "New Balance 327", Description = "Retro running-inspired lifestyle shoe.", Price = 120m, ImageUrl = "/images/products/nb_327.jpg", CategoryId = 4, BrandId = 6 },

                // ASICS (30-32)
                new Product { Id = 30, Name = "ASICS Gel-Kayano 30", Description = "Stability with premium comfort.", Price = 160m, ImageUrl = "/images/products/asics_kayano30.jpg", CategoryId = 1, BrandId = 7 },
                new Product { Id = 31, Name = "ASICS Gel-Nimbus 26", Description = "Cushioning and cloud-like ride.", Price = 170m, ImageUrl = "/images/products/asics_nimbus26.jpg", CategoryId = 2, BrandId = 7 },
                new Product { Id = 32, Name = "ASICS GT-2000 12", Description = "Everyday running reliability.", Price = 150m, ImageUrl = "/images/products/asics_gt2000.jpg", CategoryId = 1, BrandId = 7 },

                // Reebok (33-36)
                new Product { Id = 33, Name = "Reebok Club C 85", Description = "Clean retro court sneaker.", Price = 85m, ImageUrl = "/images/products/reebok_clubc85.jpg", CategoryId = 4, BrandId = 8 },
                new Product { Id = 34, Name = "Reebok Classic Leather", Description = "Heritage style and soft leather.", Price = 90m, ImageUrl = "/images/products/reebok_classic.jpg", CategoryId = 4, BrandId = 8 },
                new Product { Id = 35, Name = "Reebok Nano X4", Description = "Functional training shoe.", Price = 140m, ImageUrl = "/images/products/reebok_nanox4.jpg", CategoryId = 1, BrandId = 8 },
                new Product { Id = 36, Name = "Reebok Zig Kinetica 2.5", Description = "Energy-return performance sneaker.", Price = 130m, ImageUrl = "/images/products/reebok_zig.jpg", CategoryId = 2, BrandId = 8 }
            );

            // seed product variants (as provided subset and extended)
            modelBuilder.Entity<ProductVariant>().HasData(
                // Nike variants (1-10)
                new ProductVariant { Id = 1, ProductId = 1, Size = "41", Color = "White", StockQuantity = 20 },
                new ProductVariant { Id = 2, ProductId = 1, Size = "42", Color = "White", StockQuantity = 15 },
                new ProductVariant { Id = 3, ProductId = 1, Size = "42", Color = "Black", StockQuantity = 10 },
                new ProductVariant { Id = 4, ProductId = 2, Size = "42", Color = "Red/Black", StockQuantity = 12 },
                new ProductVariant { Id = 5, ProductId = 2, Size = "43", Color = "Blue/White", StockQuantity = 8 },
                new ProductVariant { Id = 6, ProductId = 3, Size = "40", Color = "Panda", StockQuantity = 30 },
                new ProductVariant { Id = 7, ProductId = 3, Size = "41", Color = "Panda", StockQuantity = 25 },
                new ProductVariant { Id = 8, ProductId = 4, Size = "41", Color = "Black", StockQuantity = 25 },
                new ProductVariant { Id = 9, ProductId = 4, Size = "42", Color = "Black", StockQuantity = 20 },
                new ProductVariant { Id = 10, ProductId = 5, Size = "42", Color = "White/Red", StockQuantity = 15 },
                new ProductVariant { Id = 11, ProductId = 5, Size = "43", Color = "White/Red", StockQuantity = 10 },
                new ProductVariant { Id = 12, ProductId = 6, Size = "43", Color = "Grey/Volt", StockQuantity = 8 },
                new ProductVariant { Id = 13, ProductId = 7, Size = "38", Color = "Pink Foam", StockQuantity = 12 },
                new ProductVariant { Id = 14, ProductId = 7, Size = "39", Color = "Pink Foam", StockQuantity = 10 },
                new ProductVariant { Id = 15, ProductId = 8, Size = "44", Color = "Blue/Gold", StockQuantity = 10 },
                new ProductVariant { Id = 16, ProductId = 9, Size = "40", Color = "Black/White", StockQuantity = 30 },
                new ProductVariant { Id = 17, ProductId = 9, Size = "41", Color = "Triple Black", StockQuantity = 15 },
                new ProductVariant { Id = 18, ProductId = 10, Size = "42", Color = "White/Red", StockQuantity = 7 },

                // Adidas (11-15)
                new ProductVariant { Id = 19, ProductId = 11, Size = "37", Color = "Cloud White", StockQuantity = 18 },
                new ProductVariant { Id = 20, ProductId = 11, Size = "38", Color = "Cloud White", StockQuantity = 15 },
                new ProductVariant { Id = 21, ProductId = 12, Size = "41", Color = "White/Green", StockQuantity = 40 },
                new ProductVariant { Id = 22, ProductId = 12, Size = "42", Color = "White/Green", StockQuantity = 35 },
                new ProductVariant { Id = 23, ProductId = 13, Size = "40", Color = "White/Black", StockQuantity = 50 },
                new ProductVariant { Id = 24, ProductId = 13, Size = "41", Color = "White/Black", StockQuantity = 45 },
                new ProductVariant { Id = 25, ProductId = 14, Size = "42", Color = "Core Black", StockQuantity = 20 },
                new ProductVariant { Id = 26, ProductId = 15, Size = "39", Color = "White/Black", StockQuantity = 40 },
                new ProductVariant { Id = 27, ProductId = 15, Size = "40", Color = "White/Black", StockQuantity = 38 },

                // Balenciaga (16-19)
                new ProductVariant { Id = 28, ProductId = 16, Size = "41", Color = "Beige", StockQuantity = 5 },
                new ProductVariant { Id = 29, ProductId = 16, Size = "42", Color = "Black", StockQuantity = 4 },
                new ProductVariant { Id = 30, ProductId = 17, Size = "38", Color = "Black/White", StockQuantity = 7 },
                new ProductVariant { Id = 31, ProductId = 18, Size = "42", Color = "Grey/Blue", StockQuantity = 6 },
                new ProductVariant { Id = 32, ProductId = 19, Size = "43", Color = "Black", StockQuantity = 3 },

                // Louboutin (20-21)
                new ProductVariant { Id = 33, ProductId = 20, Size = "37", Color = "White/Silver", StockQuantity = 4 },
                new ProductVariant { Id = 34, ProductId = 21, Size = "42", Color = "Black", StockQuantity = 5 },
                new ProductVariant { Id = 35, ProductId = 21, Size = "43", Color = "Navy", StockQuantity = 3 },

                // Puma (22-25)
                new ProductVariant { Id = 36, ProductId = 22, Size = "41", Color = "Grey/Yellow", StockQuantity = 12 },
                new ProductVariant { Id = 37, ProductId = 23, Size = "40", Color = "Black/White", StockQuantity = 30 },
                new ProductVariant { Id = 38, ProductId = 23, Size = "41", Color = "Red/White", StockQuantity = 15 },
                new ProductVariant { Id = 39, ProductId = 24, Size = "42", Color = "Blue/Orange", StockQuantity = 18 },
                new ProductVariant { Id = 40, ProductId = 25, Size = "38", Color = "White/Pink", StockQuantity = 20 },

                // New Balance (26-29)
                new ProductVariant { Id = 41, ProductId = 26, Size = "41", Color = "White/Green", StockQuantity = 20 },
                new ProductVariant { Id = 42, ProductId = 26, Size = "42", Color = "White/Grey", StockQuantity = 15 },
                new ProductVariant { Id = 43, ProductId = 27, Size = "42.5", Color = "Grey", StockQuantity = 9 },
                new ProductVariant { Id = 44, ProductId = 27, Size = "43", Color = "Grey", StockQuantity = 7 },
                new ProductVariant { Id = 45, ProductId = 28, Size = "42", Color = "White/Blue", StockQuantity = 14 },
                new ProductVariant { Id = 46, ProductId = 29, Size = "39", Color = "Multi", StockQuantity = 16 },
                new ProductVariant { Id = 47, ProductId = 29, Size = "40", Color = "Navy/Orange", StockQuantity = 14 },

                // ASICS (30-32)
                new ProductVariant { Id = 48, ProductId = 30, Size = "42", Color = "Black/Lime", StockQuantity = 14 },
                new ProductVariant { Id = 49, ProductId = 30, Size = "43", Color = "Black/Lime", StockQuantity = 11 },
                new ProductVariant { Id = 50, ProductId = 31, Size = "38", Color = "Pink/Blue", StockQuantity = 10 },
                new ProductVariant { Id = 51, ProductId = 32, Size = "41.5", Color = "Blue/Orange", StockQuantity = 13 },

                // Reebok (33-36)
                new ProductVariant { Id = 52, ProductId = 33, Size = "40", Color = "White/Green", StockQuantity = 22 },
                new ProductVariant { Id = 53, ProductId = 33, Size = "41", Color = "White/Green", StockQuantity = 20 },
                new ProductVariant { Id = 54, ProductId = 34, Size = "42", Color = "White/Gum", StockQuantity = 18 },
                new ProductVariant { Id = 55, ProductId = 35, Size = "43", Color = "Black/Red", StockQuantity = 9 },
                new ProductVariant { Id = 56, ProductId = 36, Size = "37", Color = "Purple/Pink", StockQuantity = 11 }
            );
        }
    }
}


