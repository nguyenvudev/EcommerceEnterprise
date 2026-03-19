# EcommerceEnterprise

> **Headless E-commerce Platform** — ASP.NET Core 8 · Angular 17 · Clean Architecture · CQRS

---

## Giới thiệu

EcommerceEnterprise là nền tảng thương mại điện tử đa kênh được xây dựng theo kiến trúc Clean Architecture. Hệ thống tách biệt hoàn toàn Backend API và Frontend, giải quyết sự gò bó của các platform có sẵn như Odoo hay WordPress.

### Tính năng chính

- Quản lý sản phẩm đa biến thể (size, màu, SKU riêng)
- Đơn hàng với vòng đời đầy đủ (Pending → Shipped → Delivered)
- Thanh toán VNPay và MoMo với HMAC verification
- Vận chuyển GHN và GHTK
- Kho hàng đa chi nhánh, đa kho
- Flash Sale với giới hạn số lượng và thời gian thực
- Hệ thống Coupon (% và giá cố định)
- Ví điện tử và hoàn tiền
- Đánh giá sản phẩm (chỉ người đã mua)
- Analytics Dashboard
- Real-time order tracking (SignalR)
- JWT Authentication + Refresh Token

---

## Tech Stack

| Layer           | Technology                | Version |
| --------------- | ------------------------- | ------- |
| Backend         | ASP.NET Core              | 8.0     |
| Architecture    | Clean Architecture + CQRS |         |
| ORM             | Entity Framework Core     | 8.0     |
| Database        | SQL Server                | 2022    |
| Cache           | Redis                     | 7.0     |
| Message Bus     | MediatR                   | 12.2    |
| Validation      | FluentValidation          | 11.9    |
| Real-time       | SignalR                   | 8.0     |
| Background Jobs | Hangfire                  | 1.8     |
| Frontend        | Angular                   | 17      |
| Container       | Docker                    |         |
| CI/CD           | GitHub Actions            |         |

---

## Kiến trúc — Clean Architecture

```
Domain Layer          ← Không phụ thuộc gì
    ↑
Application Layer     ← Phụ thuộc Domain
    ↑
Infrastructure Layer  ← Phụ thuộc Application + Domain
    ↑
API Layer             ← Phụ thuộc Application + Infrastructure
```

---

## Cấu trúc thư mục

```
EcommerceEnterprise/
├── src/
│   ├── EcommerceEnterprise.Domain/
│   │   ├── Common/          BaseEntity, AggregateRoot
│   │   ├── Entities/        Product, Order, User, Wallet...
│   │   ├── Enums/           OrderStatus, PaymentMethod...
│   │   ├── Events/          Domain Events
│   │   ├── Exceptions/      DomainException...
│   │   ├── Interfaces/      IRepository, IUnitOfWork
│   │   └── ValueObjects/    Money, Address
│   │
│   ├── EcommerceEnterprise.Application/
│   │   ├── Common/          Interfaces, Models, Behaviors
│   │   └── Features/
│   │       ├── Auth/        Commands: Login, Register, Refresh
│   │       ├── Products/    Commands + Queries
│   │       ├── Orders/      Commands + Queries
│   │       ├── Payments/    VNPay, MoMo handlers
│   │       ├── Promotions/  Coupon, FlashSale
│   │       ├── Reviews/     CreateReview
│   │       ├── Warehouses/  Stock management
│   │       ├── Analytics/   Reports
│   │       └── Wallet/      Credit, Debit
│   │
│   ├── EcommerceEnterprise.Infrastructure/
│   │   ├── Persistence/     DbContext, Repositories, Migrations
│   │   ├── Identity/        JwtTokenService, CurrentUserService
│   │   ├── Caching/         RedisCacheService
│   │   └── ExternalServices/
│   │       ├── Payment/     VNPayService
│   │       ├── Shipping/    GHNService, GHTKService
│   │       └── Email/       ConsoleEmailService
│   │
│   └── EcommerceEnterprise.API/
│       ├── Controllers/v1/  Auth, Products, Orders, Payments...
│       ├── Middleware/      ExceptionMiddleware
│       ├── Hubs/            OrderTrackingHub (SignalR)
│       └── Program.cs
│
├── tests/
│   ├── EcommerceEnterprise.UnitTests/
│   └── EcommerceEnterprise.IntegrationTests/
│
├── .github/workflows/
│   ├── ci.yml               CI pipeline
│   └── cd.yml               CD pipeline
│
├── docker-compose.yml
└── EcommerceEnterprise.sln
```

---

## Cài đặt

### Yêu cầu

- .NET 8 SDK
- Node.js 18+ LTS
- Docker Desktop
- Angular CLI 17: `npm install -g @angular/cli@17`
- EF Core CLI: `dotnet tool install --global dotnet-ef`

### Backend

```bash
# 1. Clone
git clone https://github.com/your-username/EcommerceEnterprise.git
cd EcommerceEnterprise

# 2. Start SQL Server + Redis
docker-compose up -d sqlserver redis

# 3. Tạo database
cd src/EcommerceEnterprise.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../EcommerceEnterprise.API
dotnet ef database update --startup-project ../EcommerceEnterprise.API

# 4. Chạy API
cd ../EcommerceEnterprise.API
dotnet run
```

API: `http://localhost:5000`
Swagger: `http://localhost:5000/swagger`

### Frontend

```bash
cd ecommerce-frontend
npm install
ng serve
```

Frontend: `http://localhost:4200`

### Docker (tất cả cùng lúc)

```bash
docker-compose up -d
```

---

## API Endpoints

### Auth

| Method | Endpoint              | Mô tả             |
| ------ | --------------------- | ----------------- |
| POST   | /api/v1/auth/register | Đăng ký tài khoản |
| POST   | /api/v1/auth/login    | Đăng nhập         |
| POST   | /api/v1/auth/refresh  | Refresh Token     |

### Products

| Method | Endpoint                | Mô tả                                |
| ------ | ----------------------- | ------------------------------------ |
| GET    | /api/v1/products        | Danh sách (filter, sort, phân trang) |
| GET    | /api/v1/products/{slug} | Chi tiết sản phẩm                    |
| POST   | /api/v1/products        | Tạo mới (Admin)                      |

### Orders

| Method | Endpoint                   | Mô tả            |
| ------ | -------------------------- | ---------------- |
| POST   | /api/v1/orders             | Đặt hàng         |
| GET    | /api/v1/orders/my          | Đơn hàng của tôi |
| POST   | /api/v1/orders/{id}/cancel | Hủy đơn          |

### Payments

| Method | Endpoint                      | Mô tả          |
| ------ | ----------------------------- | -------------- |
| POST   | /api/v1/payments/vnpay/create | Tạo URL VNPay  |
| POST   | /api/v1/payments/momo/create  | Tạo URL MoMo   |
| GET    | /api/v1/webhook/vnpay-return  | VNPay callback |

---

## Cấu hình

Chỉnh sửa `src/EcommerceEnterprise.API/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EcommerceEnterpriseDb;...",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "SecretKey": "YourSecretKey_AtLeast32Characters!",
    "Issuer": "EcommerceEnterprise",
    "Audience": "EcommerceEnterpriseClient"
  },
  "VNPay": {
    "TmnCode": "YOUR_TMN_CODE",
    "HashSecret": "YOUR_HASH_SECRET"
  }
}
```

**VNPay Sandbox:** https://sandbox.vnpayment.vn/devreg

Thẻ test:

```
Số thẻ: 9704198526191432198
Tên: NGUYEN VAN A  |  Ngày PH: 07/15  |  OTP: 123456
```

---

## Testing

```bash
# Tất cả tests
dotnet test

# Unit tests
dotnet test tests/EcommerceEnterprise.UnitTests

# Integration tests
dotnet test tests/EcommerceEnterprise.IntegrationTests

# Kèm coverage
dotnet test --collect:"XPlat Code Coverage"
```

---

## Docker

```bash
docker-compose up -d              # Start tất cả
docker-compose up -d --build      # Rebuild và start
docker-compose down               # Dừng, giữ data
docker-compose down -v            # Dừng, xóa data
docker logs ecommerce_api -f      # Xem log realtime
docker stats                      # Xem CPU/RAM
```

---

## CI/CD

| Workflow | Kích hoạt           | Làm gì                                        |
| -------- | ------------------- | --------------------------------------------- |
| ci.yml   | Push / Pull Request | Build + Test                                  |
| cd.yml   | Merge vào main      | Docker Build → Deploy SIT → Deploy Production |

### Luồng deploy

```
Push code → CI (Build + Test) → Merge PR
    → Build Docker Image
    → Push GitHub Container Registry
    → Deploy SIT tự động
    → Smoke Tests
    → Approve thủ công
    → Deploy Production
```

---

## Tài khoản mặc định

| Role       | Email               | Password  |
| ---------- | ------------------- | --------- |
| SuperAdmin | admin@ecommerce.com | Admin@123 |

> Đổi mật khẩu ngay sau khi deploy production.

---

## License

MIT License

---

_Xây dựng với ASP.NET Core 8 + Angular 17 + Clean Architecture_
