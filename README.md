# 🚀 Bedaya Learning Platform
### *Enterprise Grade E-Learning System (.NET 8 MVC)*

[![Framework](https://img.shields.io/badge/.NET-8.0-512bd4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-N--Tier-blue?style=for-the-badge)](https://en.wikipedia.org/wiki/Multitier_architecture)
[![Database](https://img.shields.io/badge/DB-SQL_Server-red?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server/)

A clean, well-structured learning platform built with **ASP.NET MVC (.NET 8)** using an **N-Tier architecture** (Presentation ▶︎ BLL ▶︎ DAL ▶︎ Database).

---

## 🚀 Quick Highlights
* 🏗️ **N‑Tier Architecture** — Separation of concerns (Presentation / BLL / DAL / Database).
* 👤 **Roles & Permissions** — Admin, Instructor, and Student with dedicated dashboards.
* 💳 **Payments Integration** — **PayMob** integrated (Extensible for PayPal, Fawry, and Wallets).
* 🔎 **Smart Filters** — Advanced search for courses, instructors, and categories.
* 📦 **Subscriptions** — Admin-controlled limits for content access.
* 🎬 **Media Management** — Lesson uploads with support for local and cloud storage.
  
---

## 🏗️ Architecture Layers
1. **Presentation:** MVC Controllers, Views, ViewModels.
2. **Business Logic (BLL):** Services, Validations, DTOs (AutoMapper).
3. **Data Access (DAL):** EF Core repositories, Unit of Work, Migrations.

---

## 🔑 Seed Admin (Development)
> [!IMPORTANT]
> Use these for local testing only:
> * **Email:** `admin@gmail.com`
> * **Password:** `Admin@123`

---
## ⚙️ How to Run
1. Clone the repo: `git clone https://github.com/Ma7moudMo7med/bedaya-app.git`
2. Update connection string in `appsettings.Development.json`.
3. Run `dotnet ef database update`.
4. Run the project!

---
💡 Developed by **Mahmoud Mohammed**
