# 🚀 Bedaya Learning Platform
### *Enterprise Grade E-Learning System (.NET 8 MVC)*

[![Framework](https://img.shields.io/badge/.NET-8.0-512bd4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-N--Tier-blue?style=for-the-badge)](https://en.wikipedia.org/wiki/Multitier_architecture)
[![Database](https://img.shields.io/badge/DB-SQL_Server-red?style=for-the-badge&logo=microsoft-sql-server)](https://www.microsoft.com/en-us/sql-server/)

A clean, well-structured learning platform built with **ASP.NET MVC (.NET 8)** using an **N-Tier architecture** (Presentation ▶︎ BLL ▶︎ DAL ▶︎ Database). The platform includes three roles — **Admin, Instructor, and Student** — each with dedicated dashboards and role-specific permissions.

---

## 🌟 Quick Highlights

* 🏗️ **N‑Tier Architecture** — Presentation / BLL / DAL / Database.
* 👤 **Roles** — Admin, Instructor, Student (separate dashboards & permissions).
* 💳 **Payments** — **PayMob** integrated; easy to add PayPal, Fawry, or wallets.
* 🔎 **Search & Filters** — Advanced search for courses, instructors, and categories.
* 📦 **Subscription System** — Single subscription type with admin-controlled limits.
* 🎬 **Media** — Upload video lessons (on-disk or cloud storage).
* 🔒 **Security** — Role-based authorization and best practices.

---

## 🏗️ Architecture (N‑Tier) — Overview

**Presentation (ASP.NET MVC)** &nbsp;&nbsp;&nbsp;&nbsp;⇅  
**Business Logic Layer (Services, Validation, DTOs)** &nbsp;&nbsp;&nbsp;&nbsp;⇅  
**Data Access Layer (Repositories, EF Core DbContext)** &nbsp;&nbsp;&nbsp;&nbsp;⇅  
**Database (SQL Server)**

### Layers — Responsibilities

* **Presentation Layer:** MVC Controllers, Views, ViewModels. Thin controllers — delegate logic to BLL.
* **Business Logic Layer (BLL):** Core application rules, business validation, interfaces for payment/storage, and DTOs mapping (AutoMapper).
* **Data Access Layer (DAL):** EF Core repositories, unit-of-work, migrations, and query optimizations.
* **Database:** SQL Server with migrations & seed data.

---

## 🔑 Seed Admin (Development)
> [!CAUTION]
> Use this seeded admin account for local development/testing ONLY.
* **Email:** `admin@gmail.com`
* **Password:** `Admin@123`

---

## ⚙️ Getting Started (Developer)

### Prerequisites
* .NET 8 SDK
* SQL Server / LocalDB
* Visual Studio 2022+ or VS Code

### Quick Setup
1. **Clone repo:**
   ```bash
   git clone [https://github.com/Ma7moudMo7med/bedaya-app.git](https://github.com/Ma7moudMo7med/bedaya-app.git)
   cd bedaya-app
