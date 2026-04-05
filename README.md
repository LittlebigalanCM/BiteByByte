# BiteByByte

A full-stack food ordering web application built with ASP.NET Core 9. Customers can browse a menu, manage a shopping cart, and pay via Stripe — while admins manage the menu, categories, and user roles through a dedicated dashboard.

## Features

**Customer**
- Browse menu items by category with images and descriptions
- Add/remove items from cart with quantity controls
- Automatic 8.25% sales tax calculation at checkout
- Secure payment processing via Stripe
- Order confirmation and status tracking

**Admin**
- Full CRUD for menu items, categories, and food types
- User and role management (Admin, Customer, Driver, Kitchen)
- Menu item image uploads

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core 9, Razor Pages |
| Language | C# |
| ORM | Entity Framework Core 9 |
| Database | SQL Server / SQLite |
| Auth | ASP.NET Core Identity |
| Payments | Stripe.NET SDK |

## Architecture

Follows a clean 4-project layered architecture:

- **BB.Core** — Domain models and interfaces
- **BB.Application** — DbContext, Unit of Work, Generic Repository, DB seeding
- **BB.Infrastructure** — EF Core migrations
- **BB.Web** — Razor Pages UI + MVC API controllers

## Getting Started

### Prerequisites

- Visual Studio 2022 (Community, Professional, or Enterprise)
- The **ASP.NET and web development** workload installed
- .NET 9 SDK

### Build & Run Instructions

1. **Clone the repository**
   ```bash
   git clone https://github.com/LittlebigalanCM/BiteByByte.git
   ```

2. **Restore NuGet Packages**
   - Visual Studio normally restores packages automatically.
   - If not, right-click the solution in **Solution Explorer** and select **Restore NuGet Packages**.

3. **Set the Startup Project**
   - In **Solution Explorer**, right-click `BB.Web` and choose **Set as Startup Project**.

4. **Configure User Secrets**
   - See the [User Secrets](#user-secrets) section below.

5. **Configure the Database**
   - Open the **Package Manager Console** and run:
     ```
     Add-Migration Initial
     Update-Database
     ```

6. **Build the Project**
   - Press **Ctrl+Shift+B**, or go to **Build → Build Solution**.

7. **Run the Application**
   - Press **F5** (Debug) or **Ctrl+F5** (Run without debugging).

## User Secrets

Never commit API keys or connection strings to a repository. This project uses Visual Studio's **User Secrets Manager** to keep sensitive configuration out of source control.

The `appsettings.example.json` file in the `BB.Web` project shows the expected structure. Copy it as a reference when setting up your secrets.

To configure your local secrets:

1. In **Solution Explorer**, right-click the `BB.Web` project
2. Click **Manage User Secrets** — this opens a `secrets.json` file tied to your local machine
3. Add your connection string and Stripe keys following the structure in `appsettings.example.json`

Visual Studio applies configuration files in this order, with later files taking priority:

- `appsettings.json`
- `appsettings.{Environment}.json` (e.g. `appsettings.Development.json`)
- `secrets.json`

This means your local `secrets.json` will override anything in `appsettings.json` without touching the committed files.
