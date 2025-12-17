# 🚀 Bedaya Learning Platform
### *Enterprise Grade E-Learning System (.NET 8 MVC)*

[![Framework](https://img.shields.io/badge/.NET-8.0-512bd4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-N--Tier-blue?style=for-the-badge)](https://en.wikipedia.org/wiki/Multitier_architecture)
[![Database](https://img.shields.io/badge/DB-SQL_Server-red?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server/)

A clean, well-structured learning platform built with **ASP.NET MVC (.NET 8)** using an **N-Tier architecture** (Presentation ▶︎ BLL ▶︎ DAL ▶︎ Database).

---

## 🌟 Quick Highlights

* 🏗️ **N‑Tier Architecture** — Separation of concerns (Presentation / BLL / DAL / Database).
* 👤 **Roles & Permissions** — Admin, Instructor, and Student with dedicated dashboards.
* 💳 **Payments Integration** — **PayMob** integrated (Extensible for PayPal, Fawry, and Wallets).
* 🔎 **Smart Filters** — Advanced search for courses, instructors, and categories.
* 📦 **Subscriptions** — Admin-controlled limits for content access.
* 🎬 **Media Management** — Lesson uploads with support for local and cloud storage.

---

## 🏗️ Technical Architecture

| Layer | Responsibility |
| :--- | :--- |
| **Presentation** | MVC Controllers, Razor Views, and ViewModels. |
| **Business Logic (BLL)** | Core rules, Services, Business Validation, and AutoMapper DTOs. |
| **Data Access (DAL)** | EF Core, Repositories, Unit of Work, and Seeding logic. |
| **Database** | SQL Server with optimized schema and migrations. |

---

## 🔑 Development Access (Seeded)
> [!CAUTION]
> Use these credentials for **Local Development ONLY**. Change them before production.

* 📧 **Email:** `admin@gmail.com`
* 🔑 **Password:** `Admin@123`

---

## ⚙️ Installation & Setup

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/Ma7moudMo7med/bedaya-app.git](https://github.com/Ma7moudMo7med/bedaya-app.git)
