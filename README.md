# 🗓️ Employee Leave Tracker

A full-stack **Employee Leave Management System** built with **ASP.NET MVC**, **C#**, and **MySQL**. This project demonstrates real-world CRUD operations, database integration, and a clean responsive UI using Bootstrap 5.

---

## 📸 Features

- 📊 **Dashboard** — Live stats showing Total, Pending, Approved, and Rejected requests
- ✅ **Apply for Leave** — Form with full server-side validation
- 🔍 **Filter by Status** — View Pending / Approved / Rejected requests instantly
- ✔️ **Approve / Reject** — One-click status management from dashboard or detail page
- 📄 **Leave Details** — Full breakdown of each request with action buttons
- 🗑️ **Delete Requests** — Remove any leave entry with confirmation prompt
- 📱 **Responsive UI** — Clean Bootstrap 5 design that works on all screen sizes

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Backend | ASP.NET MVC (.NET Framework 4.7.2) |
| Language | C# |
| Database | MySQL |
| ORM / DB Access | MySql.Data (ADO.NET) |
| Frontend | Razor Views, Bootstrap 5 |
| IDE | Visual Studio 2019 / 2022 |

---

## 📁 Project Structure

```
LeaveTracker/
├── Controllers/
│   └── LeaveController.cs        # All route actions (Index, Apply, Approve, Reject, Delete, Details)
├── Models/
│   ├── LeaveRequest.cs           # LeaveRequest + Employee model classes
│   └── DbHelper.cs               # All SQL queries using MySql.Data (ADO.NET)
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml        # Shared navbar + Bootstrap layout
│   └── Leave/
│       ├── Index.cshtml          # Dashboard — stat cards + leave table
│       ├── Apply.cshtml          # Apply leave form
│       └── Details.cshtml        # Single leave detail + approve/reject
├── database.sql                  # MySQL schema + seed data
├── Web.config                    # Connection string configuration
└── README.md
```

---

## 🗃️ Database Schema

```sql
-- Employees
CREATE TABLE Employees (
    EmployeeId   INT AUTO_INCREMENT PRIMARY KEY,
    FullName     VARCHAR(100) NOT NULL,
    Email        VARCHAR(150) NOT NULL UNIQUE,
    Department   VARCHAR(100) NOT NULL,
    CreatedAt    DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Leave Requests
CREATE TABLE LeaveRequests (
    LeaveId     INT AUTO_INCREMENT PRIMARY KEY,
    EmployeeId  INT NOT NULL,
    LeaveType   VARCHAR(50) NOT NULL,         -- Sick / Casual / Annual
    StartDate   DATE NOT NULL,
    EndDate     DATE NOT NULL,
    Reason      VARCHAR(500),
    Status      VARCHAR(20) DEFAULT 'Pending', -- Pending / Approved / Rejected
    AppliedOn   DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId) ON DELETE CASCADE
);
```

---

## ⚙️ Getting Started

### Prerequisites

- [Visual Studio 2019 or 2022](https://visualstudio.microsoft.com/)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) + MySQL Workbench
- .NET Framework 4.7.2

---

### 1. Clone the Repository

```bash
git clone https://github.com/your-username/employee-leave-tracker.git
cd employee-leave-tracker
```

---

### 2. Set Up the Database

Open **MySQL Workbench** and run the provided SQL file:

```sql
source /path/to/database.sql;
```

Or open `database.sql` and execute it directly in MySQL Workbench. This will:
- Create the `LeaveTrackerDB` database
- Create `Employees` and `LeaveRequests` tables
- Insert 4 sample employees and 4 sample leave requests

---

### 3. Open in Visual Studio

- Open Visual Studio
- Go to **File → Open → Project/Solution**
- Select the `.sln` file

> **If creating from scratch:** Create a new **ASP.NET Web Application (.NET Framework)** project, select **MVC** template, and copy all files into the matching folders.

---

### 4. Install NuGet Package

Open **Tools → NuGet Package Manager → Package Manager Console** and run:

```powershell
Install-Package MySql.Data
```

---

### 5. Configure the Connection String

Open `Web.config` and update your MySQL credentials:

```xml
<connectionStrings>
  <add name="LeaveTrackerDB"
       connectionString="Server=localhost;Port=3306;Database=LeaveTrackerDB;Uid=root;Pwd=your_password;CharSet=utf8;"
       providerName="MySql.Data.MySqlClient" />
</connectionStrings>
```

Replace `root` and `your_password` with your actual MySQL username and password.

---

### 6. Set Default Route

Open `App_Start/RouteConfig.cs` and update the default controller:

```csharp
routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Leave", action = "Index", id = UrlParameter.Optional }
);
```

---

### 7. Run the Project

Press **F5** in Visual Studio. The app will open at:

```
http://localhost:{port}/Leave
```

---

## 🧪 SQL Operations Covered

| Operation | Used In |
|---|---|
| `SELECT` with `JOIN` | Fetch all leaves with employee name & department |
| `INSERT` | Apply new leave, add employee |
| `UPDATE` | Approve / Reject leave status |
| `DELETE` | Remove a leave request |
| `COUNT` / `SUM` aggregate | Dashboard stats |
| `WHERE` filter | Filter by status, filter by employee |
| `ORDER BY` | Sort by applied date descending |

---

## 🚀 Future Enhancements

- [ ] Login system with Admin and Employee roles
- [ ] Email notifications on approval/rejection
- [ ] Leave balance tracking per employee
- [ ] Export leave report to PDF or Excel
- [ ] Calendar view for leave visualization

---

## 👨‍💻 Author

**Your Name**
- 🔗 LinkedIn: [linkedin.com/in/your-profile](https://linkedin.com/in/harshit-kumar-pathak-9991072a7)
- 💻 GitHub: [github.com/your-username](https://github.com/peror404)

---

## 📄 License

This project is open source and available under the [MIT License](LICENSE).
