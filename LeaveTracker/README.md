# Employee Leave Tracker — ASP.NET MVC + C# + MySQL

## Project Structure

```
LeaveTracker/
├── Controllers/
│   └── LeaveController.cs       # All actions: Index, Apply, Approve, Reject, Delete, Details
├── Models/
│   ├── LeaveRequest.cs          # LeaveRequest + Employee model classes
│   └── DbHelper.cs              # All SQL queries using MySql.Data
├── Views/
│   ├── Shared/
│   │   └── _Layout.cshtml       # Navbar + Bootstrap layout
│   └── Leave/
│       ├── Index.cshtml         # Dashboard with stats + leave table
│       ├── Apply.cshtml         # Apply leave form
│       └── Details.cshtml       # Single leave detail + approve/reject
├── database.sql                 # MySQL schema + seed data
└── Web.config                   # Connection string config
```

## Features

- Dashboard with live stats (Total / Pending / Approved / Rejected)
- Apply for leave with validation (employee, type, date range, reason)
- Approve or Reject pending requests (from dashboard or detail page)
- Filter leaves by status
- Delete any leave request
- View full details of any request
- Bootstrap 5 UI — clean and responsive

## Setup Steps

### 1. Database
Open MySQL Workbench or any MySQL client and run:
```sql
source /path/to/database.sql
```
Or paste the contents of `database.sql` and execute.

### 2. Visual Studio
- Open Visual Studio 2019 or 2022
- Create New Project → ASP.NET Web Application (.NET Framework)
- Choose MVC template
- Target Framework: .NET Framework 4.7.2

### 3. Install NuGet Package
Open Package Manager Console and run:
```
Install-Package MySql.Data
```

### 4. Copy files
- Copy each file from this project into the matching folder in your VS project
- Replace the auto-generated HomeController with LeaveController
- Update the default route in RouteConfig.cs:

```csharp
// App_Start/RouteConfig.cs
routes.MapRoute(
    name: "Default",
    url: "{controller}/{action}/{id}",
    defaults: new { controller = "Leave", action = "Index", id = UrlParameter.Optional }
);
```

### 5. Connection String
In Web.config, update:
```xml
Server=localhost;Port=3306;Database=LeaveTrackerDB;Uid=root;Pwd=yourpassword;
```
Replace `root` and `yourpassword` with your MySQL credentials.

### 6. Run
Press F5 in Visual Studio. The dashboard opens at http://localhost:{port}/Leave

## SQL Operations Used

| Operation | Where Used |
|-----------|-----------|
| SELECT with JOIN | GetAllLeaves, GetLeaveById |
| INSERT | ApplyLeave, AddEmployee |
| UPDATE | UpdateLeaveStatus (Approve/Reject) |
| DELETE | DeleteLeave |
| Aggregate (COUNT, SUM) | GetDashboardStats |
| WHERE filter | GetLeavesByEmployee, Filter by status |
| ORDER BY | All list queries |
