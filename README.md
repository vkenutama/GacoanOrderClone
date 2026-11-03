# 🍜 Mie Gacoan Backend API

Backend RESTful API untuk aplikasi **Mie Gacoan Clone**, dibangun menggunakan **.NET 8 Web API** dengan arsitektur bersih (*Clean/Layered Architecture*), Entity Framework Core (SQLite), Microsoft Identity, autentikasi JWT, FluentValidation, dan AutoMapper.

---

## 🛠️ Tech Stack & Libraries

- **Framework**: [.NET 8 Web API](https://dotnet.microsoft.com/download/dotnet/8.0) (C# 12)
- **Database**: [SQLite](https://www.sqlite.org/) via Entity Framework Core (`Microsoft.EntityFrameworkCore.Sqlite`)
- **Authentication & Identity**: Microsoft ASP.NET Core Identity (`IdentityUser<Guid>`, `IdentityRole<Guid>`) dengan JWT Bearer Tokens (`System.IdentityModel.Tokens.Jwt`)
- **Validation**: [FluentValidation](https://docs.fluentvalidation.net/) dengan validasi otomatis pada request pipeline (`FluentValidation.AspNetCore`)
- **Object Mapping**: [AutoMapper](https://automapper.org/) v14 untuk transformasi Entity ↔ DTO
- **API Documentation**: [Swagger / OpenAPI 3.0](https://swagger.io/) via Swashbuckle dengan integrasi otorisasi JWT Bearer
- **Architecture Pattern**: Repository Pattern, Service Layer Pattern, dan `ServiceResult<T>` Response Wrapper Pattern

---

## 📁 Struktur Direktori

```text
be/Gacoan.Backend/
├── Common/
│   ├── Mappings/            # AutoMapper Profile (MappingProfile.cs)
│   └── Models/              # ServiceResult<T> pattern & generic response
├── Configurations/          # Konstanta & konfigurasi bisnis (User, Product, Jwt)
├── Controllers/             # API Endpoints (Auth, Product, Restaurant, Cart)
│   └── BaseApiController.cs # Base controller dengan claim helper (CurrentUserId, Role)
├── Database/
│   ├── AppDbContext.cs      # EF Core DbContext & relasi tabel
│   └── Seeder.cs            # Data seeder otomatis saat aplikasi start
├── Dtos/                    # Data Transfer Objects (Auth, Product, Restaurant, Cart)
├── Models/                  # Database Entities (User, Product, Restaurant, Cart, dll)
├── Repository/              # Generic Repository Pattern (IRepository<T>, Repository<T>)
├── Services/                # Business Logic Services (Auth, Product, Restaurant, Cart)
├── Validators/              # FluentValidation rules untuk setiap DTO
├── appsettings.json         # Konfigurasi koneksi database, JWT, dan logging
└── Program.cs               # Entry point & Dependency Injection container
```

---

## 🗄️ Model Data & Relasi

1. **User & Role** (`Microsoft.AspNetCore.Identity`):
   - Primary key menggunakan `Guid`.
   - Mendukung 2 role bawaan: `Admin` dan `Customer`.
   - User memiliki relasi *one-to-one* opsional dengan `OrderCart`.
2. **Product & ProductType**:
   - `Product` memiliki relasi *many-to-one* ke `ProductType` (kategori seperti "Mie", "Beverage", dll).
3. **Restaurant & OperationalHour**:
   - `Restaurant` memiliki relasi *one-to-many* ke `OperationalHour` (jadwal buka/tutup per hari dalam seminggu).
4. **OrderCart & OrderItem**:
   - Satu `OrderCart` dimiliki oleh satu `User` dan terikat pada satu cabang `Restaurant`.
   - `OrderCart` memiliki banyak `OrderItem` (produk, kuantitas, catatan khusus/level pedas).

---

## 🚀 Memulai (Getting Started)

### Prasyarat
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) terpasang di komputer.
- Database SQLite tidak membutuhkan instalasi server database eksternal (file `database.db` akan dibuat otomatis di root project backend).

### Menjalankan Aplikasi

1. Buka terminal dan masuk ke direktori backend:
   ```bash
   cd be/Gacoan.Backend
   ```

2. Restore dependensi NuGet:
   ```bash
   dotnet restore
   ```

3. Jalankan migrasi database (opsional jika database belum ada):
   ```bash
   dotnet ef database update
   ```

4. Jalankan server:
   ```bash
   dotnet run
   ```

5. Server akan aktif pada:
   - **HTTP**: `http://localhost:5232`
   - **HTTPS**: `https://localhost:7234`
   - **Swagger UI**: [http://localhost:5232/swagger](http://localhost:5232/swagger)

---

## 🌱 Data Otomatis (Database Seeder)

Setiap kali aplikasi dijalankan, sistem otomatis melakukan pengecekan dan inisialisasi data (*data seeding*) jika belum tersedia:
- **Roles**: Otomatis membuat role `Admin` dan `Customer`.
- **Kategori Produk**: Menambahkan kategori `"Mie"`.
- **Produk**: Menambahkan produk `"Gacoan lv1"` hingga `"Gacoan lv4"` dengan harga default `Rp 12.000`.
- **Restoran**: Menambahkan cabang `"Mie Gacoan Merr Surabaya"` lengkap dengan jam operasional 7 hari seminggu (09:00 - 22:00).

---

## 🔐 Autentikasi & Otorisasi (JWT)

Backend menggunakan **JSON Web Token (JWT)** untuk autentikasi endpoint yang dilindungi.

### Cara Penggunaan di Swagger UI:
1. Panggil endpoint `POST /api/Auth/login` atau `POST /api/Auth/register`.
2. Salin token dari response (`data.token`).
3. Klik tombol **Authorize** (ikon gembok hijau) di pojok kanan atas Swagger UI.
4. Masukkan format: `Bearer {token_anda}` (contoh: `Bearer eyJhbGciOi...`).
5. Klik **Authorize** dan selesai.

---

## 📡 Daftar Endpoint API

### 1. Autentikasi (`/api/Auth`)
| Method | Endpoint | Akses | Deskripsi |
| :--- | :--- | :--- | :--- |
| `POST` | `/api/Auth/register` | Publik | Mendaftarkan akun baru (Role: `Customer` / `Admin`) |
| `POST` | `/api/Auth/login` | Publik | Login menggunakan username dan password |
| `GET` | `/api/Auth/me` | Logged In | Mengambil data lengkap profil user yang sedang login |

### 2. Produk & Kategori (`/api/Product`)
| Method | Endpoint | Akses | Deskripsi |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Product` | Publik | Mengambil semua daftar menu produk |
| `GET` | `/api/Product/{id}` | Publik | Mengambil detail satu produk berdasarkan ID |
| `GET` | `/api/Product/types` | Publik | Mengambil semua daftar kategori produk |
| `POST` | `/api/Product` | `Admin` | Menambahkan produk baru |
| `PUT` | `/api/Product/{id}` | `Admin` | Memperbarui data produk |
| `DELETE` | `/api/Product/{id}` | `Admin` | Menghapus produk |

### 3. Restoran & Cabang (`/api/Restaurant`)
| Method | Endpoint | Akses | Deskripsi |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Restaurant` | Publik | Mengambil seluruh cabang restoran Mie Gacoan |
| `GET` | `/api/Restaurant/{id}` | Publik | Mengambil detail satu restoran |
| `GET` | `/api/Restaurant/{id}/operational-hours` | Publik | Mengambil jadwal jam buka/tutup cabang restoran |
| `POST` | `/api/Restaurant` | `Admin` | Menambahkan cabang restoran baru |
| `PUT` | `/api/Restaurant/{id}` | `Admin` | Memperbarui data cabang restoran |
| `DELETE` | `/api/Restaurant/{id}` | `Admin` | Menghapus data cabang restoran |

### 4. Keranjang Belanja (`/api/Cart`)
*Seluruh endpoint Cart mewajibkan user login (`[Authorize]`)*
| Method | Endpoint | Akses | Deskripsi |
| :--- | :--- | :--- | :--- |
| `GET` | `/api/Cart` | User Login | Mengambil keranjang aktif milik user yang sedang login |
| `POST` | `/api/Cart/items` | User Login | Menambahkan produk ke keranjang belanja |
| `PUT` | `/api/Cart/items/{itemId}` | User Login | Mengubah kuantitas atau catatan menu pada keranjang |
| `DELETE` | `/api/Cart/items/{itemId}` | User Login | Menghapus satu item dari keranjang belanja |
| `DELETE` | `/api/Cart` | User Login | Mengosongkan seluruh isi keranjang belanja |

---

## 📦 Standar Format Response (`ServiceResult<T>`)

Semua endpoint mengembalikan format response standar yang konsisten:

**Berhasil (200 OK):**
```json
{
  "statusCode": 200,
  "success": true,
  "message": "Registrasi berhasil",
  "data": {
    "token": "eyJhbGciOi...",
    "userName": "vincent",
    "email": "vincent@example.com",
    "role": "Customer"
  }
}
```

**Gagal / Validasi Error (400 Bad Request):**
```json
{
  "statusCode": 400,
  "success": false,
  "message": "Password minimal 6 karakter.",
  "data": null
}
```

---

## ⚙️ Pengaturan Environment (`appsettings.json`)

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=database.db"
  },
  "Jwt": {
    "Key": "KunciRahasiaMinimal32KarakterHarusKuatDanAman!",
    "Issuer": "GacoanBackend",
    "Audience": "GacoanApp",
    "ExpireDays": 7
  }
}
```
