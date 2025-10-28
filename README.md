# Nike Clone - ASP.NET Core MVC Project

## 📋 Mô tả dự án
Dự án clone website Nike sử dụng ASP.NET Core MVC với các tính năng frontend hiện đại.

## 🎯 Tính năng đã triển khai

### Frontend
- ✅ Bootstrap 5 cho responsive design
- ✅ jQuery cho AJAX và interactive features
- ✅ CSS tùy chỉnh theo phong cách Nike
- ✅ Bootstrap Icons cho icons
- ✅ Responsive design cho mobile, tablet và desktop

### Layout và Components
- ✅ Top Bar với search và cart icon
- ✅ Main Header với navigation menu
- ✅ Hero Section với call-to-action buttons
- ✅ Feature Section (2x2 grid)
- ✅ Special Deals Section với product cards
- ✅ Shop by Icons Section với category icons
- ✅ Footer với links và social media

### Backend/API
- ✅ RESTful Web API endpoints
  - `GET /Home/GetProducts` - Lấy danh sách sản phẩm
  - `GET /Home/GetFeaturedProducts` - Lấy sản phẩm nổi bật
  - `GET /Home/GetSpecialDeals` - Lấy deals đặc biệt
  - `POST /Home/SearchProducts` - Tìm kiếm sản phẩm qua AJAX

### Models
- ✅ Product Model
- ✅ ShoppingCartItem Model
- ✅ SearchRequest Model

### JavaScript/AJAX
- ✅ Search functionality với AJAX
- ✅ Cart management với cookies
- ✅ Smooth scroll navigation
- ✅ Filter và sort products (demo)
- ✅ Add to cart functionality

## 📁 Cấu trúc dự án

```
BTL_WEBDEV2025/
├── Controllers/
│   └── HomeController.cs (với API endpoints)
├── Models/
│   ├── Product.cs
│   ├── ShoppingCartItem.cs
│   └── ErrorViewModel.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── Partials/
│   │       ├── _TopBar.cshtml
│   │       ├── _MainHeader.cshtml
│   │       ├── _HeroSection.cshtml
│   │       ├── _FeatureSection.cshtml
│   │       ├── _SpecialDealsSection.cshtml
│   │       ├── _ShopByIconsSection.cshtml
│   │       └── _Footer.cshtml
│   ├── Home/
│   │   └── Index.cshtml
│   └── Products/
│       └── Index.cshtml
├── wwwroot/
│   ├── css/
│   │   ├── site.css
│   │   └── nike-style.css
│   └── js/
│       └── site.js
└── Program.cs
```

## 🚀 Hướng dẫn chạy dự án

1. Mở terminal trong thư mục `BTL_WEBDEV2025`
2. Chạy lệnh:
   ```bash
   dotnet run
   ```
3. Truy cập: `https://localhost:5001` hoặc `http://localhost:5000`

## 📝 Ghi chú

### Phù hợp với MVC Pattern
- **Models**: `Product.cs`, `ShoppingCartItem.cs` - Quản lý dữ liệu
- **Views**: Các file `.cshtml` trong thư mục Views - Hiển thị UI
- **Controllers**: `HomeController.cs` - Xử lý logic và trả về views/JSON

### Partial Views
Các partial views được tách riêng để dễ quản lý và tái sử dụng:
- `_TopBar.cshtml` - Thanh trên cùng
- `_MainHeader.cshtml` - Header và navigation
- `_HeroSection.cshtml` - Khu vực hero banner
- `_FeatureSection.cshtml` - Khu vực features (2x2 grid)
- `_SpecialDealsSection.cshtml` - Khu vực deals
- `_ShopByIconsSection.cshtml` - Khu vực icons/categories
- `_Footer.cshtml` - Footer

### Responsive Design
- Mobile-first approach
- Bootstrap grid system (col-12, col-md-6, col-lg-3)
- Media queries trong `nike-style.css`

### Security & Internationalization
- Có thể mở rộng với authentication và authorization
- Session và cookies đã được thiết lập sẵn
- Ready for i18n implementation

## 🔄 Tính năng có thể mở rộng

1. **Database Integration**: Kết nối với SQL Server/SQLite
2. **Authentication**: Login/Register với ASP.NET Identity
3. **Shopping Cart**: Database-backed cart thay vì cookies
4. **Product Details Page**: Trang chi tiết sản phẩm
5. **Pagination**: Server-side pagination cho products
6. **Image Upload**: Upload và lưu trữ hình ảnh
7. **Admin Panel**: CRUD operations cho products

## 👨‍💻 Tác giả
BTL_WEBDEV2025 - ASP.NET Core MVC Project

