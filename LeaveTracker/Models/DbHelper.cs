// Models/DbHelper.cs
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using LeaveTracker.Models;

namespace LeaveTracker.Models
{
    public static class DbHelper
    {
        // Reads connection string from Web.config
        private static string ConnStr =>
            ConfigurationManager.ConnectionStrings["LeaveTrackerDB"].ConnectionString;

        // ── Employees ──────────────────────────────────────────────────────────

        public static List<Employee> GetAllEmployees()
        {
            var list = new List<Employee>();
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "SELECT EmployeeId, FullName, Email, Department, CreatedAt FROM Employees ORDER BY FullName",
                    con);
                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                        list.Add(MapEmployee(rdr));
                }
            }
            return list;
        }

        public static Employee GetEmployeeById(int id)
        {
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "SELECT EmployeeId, FullName, Email, Department, CreatedAt FROM Employees WHERE EmployeeId=@id",
                    con);
                cmd.Parameters.AddWithValue("@id", id);
                using (var rdr = cmd.ExecuteReader())
                    return rdr.Read() ? MapEmployee(rdr) : null;
            }
        }

        public static void AddEmployee(Employee e)
        {
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO Employees (FullName, Email, Department) VALUES (@name, @email, @dept)",
                    con);
                cmd.Parameters.AddWithValue("@name",  e.FullName);
                cmd.Parameters.AddWithValue("@email", e.Email);
                cmd.Parameters.AddWithValue("@dept",  e.Department);
                cmd.ExecuteNonQuery();
            }
        }

        // ── Leave Requests ─────────────────────────────────────────────────────

        public static List<LeaveRequest> GetAllLeaves(string statusFilter = null)
        {
            var list = new List<LeaveRequest>();
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var sql = @"
                    SELECT l.LeaveId, l.EmployeeId, e.FullName AS EmployeeName,
                           e.Department, l.LeaveType, l.StartDate, l.EndDate,
                           l.Reason, l.Status, l.AppliedOn
                    FROM   LeaveRequests l
                    JOIN   Employees e ON e.EmployeeId = l.EmployeeId";

                if (!string.IsNullOrEmpty(statusFilter))
                    sql += " WHERE l.Status = @status";

                sql += " ORDER BY l.AppliedOn DESC";

                var cmd = new MySqlCommand(sql, con);
                if (!string.IsNullOrEmpty(statusFilter))
                    cmd.Parameters.AddWithValue("@status", statusFilter);

                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read()) list.Add(MapLeave(rdr));
            }
            return list;
        }

        public static List<LeaveRequest> GetLeavesByEmployee(int empId)
        {
            var list = new List<LeaveRequest>();
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(@"
                    SELECT l.LeaveId, l.EmployeeId, e.FullName AS EmployeeName,
                           e.Department, l.LeaveType, l.StartDate, l.EndDate,
                           l.Reason, l.Status, l.AppliedOn
                    FROM   LeaveRequests l
                    JOIN   Employees e ON e.EmployeeId = l.EmployeeId
                    WHERE  l.EmployeeId = @empId
                    ORDER BY l.AppliedOn DESC", con);
                cmd.Parameters.AddWithValue("@empId", empId);
                using (var rdr = cmd.ExecuteReader())
                    while (rdr.Read()) list.Add(MapLeave(rdr));
            }
            return list;
        }

        public static LeaveRequest GetLeaveById(int leaveId)
        {
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(@"
                    SELECT l.LeaveId, l.EmployeeId, e.FullName AS EmployeeName,
                           e.Department, l.LeaveType, l.StartDate, l.EndDate,
                           l.Reason, l.Status, l.AppliedOn
                    FROM   LeaveRequests l
                    JOIN   Employees e ON e.EmployeeId = l.EmployeeId
                    WHERE  l.LeaveId = @id", con);
                cmd.Parameters.AddWithValue("@id", leaveId);
                using (var rdr = cmd.ExecuteReader())
                    return rdr.Read() ? MapLeave(rdr) : null;
            }
        }

        public static void ApplyLeave(LeaveRequest lr)
        {
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(@"
                    INSERT INTO LeaveRequests
                        (EmployeeId, LeaveType, StartDate, EndDate, Reason, Status)
                    VALUES
                        (@empId, @type, @start, @end, @reason, 'Pending')", con);
                cmd.Parameters.AddWithValue("@empId",  lr.EmployeeId);
                cmd.Parameters.AddWithValue("@type",   lr.LeaveType);
                cmd.Parameters.AddWithValue("@start",  lr.StartDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@end",    lr.EndDate.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@reason", lr.Reason ?? "");
                cmd.ExecuteNonQuery();
            }
        }

        public static void UpdateLeaveStatus(int leaveId, string status)
        {
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "UPDATE LeaveRequests SET Status=@status WHERE LeaveId=@id", con);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id",     leaveId);
                cmd.ExecuteNonQuery();
            }
        }

        public static void DeleteLeave(int leaveId)
        {
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(
                    "DELETE FROM LeaveRequests WHERE LeaveId=@id", con);
                cmd.Parameters.AddWithValue("@id", leaveId);
                cmd.ExecuteNonQuery();
            }
        }

        // ── Dashboard stats ────────────────────────────────────────────────────

        public static Dictionary<string, int> GetDashboardStats()
        {
            var stats = new Dictionary<string, int>();
            using (var con = new MySqlConnection(ConnStr))
            {
                con.Open();
                var cmd = new MySqlCommand(@"
                    SELECT
                        COUNT(*) AS Total,
                        SUM(Status='Pending')  AS Pending,
                        SUM(Status='Approved') AS Approved,
                        SUM(Status='Rejected') AS Rejected
                    FROM LeaveRequests", con);
                using (var rdr = cmd.ExecuteReader())
                {
                    if (rdr.Read())
                    {
                        stats["Total"]    = Convert.ToInt32(rdr["Total"]);
                        stats["Pending"]  = Convert.ToInt32(rdr["Pending"]);
                        stats["Approved"] = Convert.ToInt32(rdr["Approved"]);
                        stats["Rejected"] = Convert.ToInt32(rdr["Rejected"]);
                    }
                }
            }
            return stats;
        }

        // ── Private mappers ────────────────────────────────────────────────────

        private static Employee MapEmployee(MySqlDataReader r) => new Employee
        {
            EmployeeId = Convert.ToInt32(r["EmployeeId"]),
            FullName   = r["FullName"].ToString(),
            Email      = r["Email"].ToString(),
            Department = r["Department"].ToString(),
            CreatedAt  = Convert.ToDateTime(r["CreatedAt"])
        };

        private static LeaveRequest MapLeave(MySqlDataReader r) => new LeaveRequest
        {
            LeaveId      = Convert.ToInt32(r["LeaveId"]),
            EmployeeId   = Convert.ToInt32(r["EmployeeId"]),
            EmployeeName = r["EmployeeName"].ToString(),
            Department   = r["Department"].ToString(),
            LeaveType    = r["LeaveType"].ToString(),
            StartDate    = Convert.ToDateTime(r["StartDate"]),
            EndDate      = Convert.ToDateTime(r["EndDate"]),
            Reason       = r["Reason"].ToString(),
            Status       = r["Status"].ToString(),
            AppliedOn    = Convert.ToDateTime(r["AppliedOn"])
        };
    }
}
