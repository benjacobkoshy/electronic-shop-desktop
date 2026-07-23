# Electronic Shop Desktop Management System

A standalone desktop application designed for repair and electronic shops to manage stock inventory, track repair ticket workflows, and generate customer billing receipts.

## Key Features

- **Inventory Control:** Add, update, and manage stock levels. Automatic deduction when parts are linked to customer jobs.
- **Job Status Tracking:** Manage customer repair tickets with real-time status updates (*In Progress*, *Waiting for Stock*, *Resolved*).
- **Billing & Receipts:** Generate detailed receipts including service charges and used spare parts.
- **Local Storage:** Runs completely offline using an embedded SQLite database.

## Built With

- **Framework:** .NET 8 / WPF
- **Architecture:** MVVM Pattern (via `CommunityToolkit.Mvvm`)
- **Database:** SQLite (via Entity Framework Core)
- **UI Styling:** MaterialDesignInXamlToolkit / WPF-UI

## Getting Started

### Prerequisites
- Visual Studio 2022+ with **.NET Desktop Development** workload installed.
- .NET 8 SDK or higher.

### Installation
1. Clone the repository:
   ```bash
   git clone [https://github.com/benjacobkoshy/electronic-shop-desktop.git]