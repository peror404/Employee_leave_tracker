-- ============================================
--  Employee Leave Tracker — MySQL Schema
-- ============================================

CREATE DATABASE IF NOT EXISTS LeaveTrackerDB;
USE LeaveTrackerDB;

-- Employees table
CREATE TABLE IF NOT EXISTS Employees (
    EmployeeId   INT AUTO_INCREMENT PRIMARY KEY,
    FullName     VARCHAR(100) NOT NULL,
    Email        VARCHAR(150) NOT NULL UNIQUE,
    Department   VARCHAR(100) NOT NULL,
    CreatedAt    DATETIME DEFAULT CURRENT_TIMESTAMP
);

-- Leave requests table
CREATE TABLE IF NOT EXISTS LeaveRequests (
    LeaveId      INT AUTO_INCREMENT PRIMARY KEY,
    EmployeeId   INT NOT NULL,
    LeaveType    VARCHAR(50) NOT NULL,        -- Sick / Casual / Annual
    StartDate    DATE NOT NULL,
    EndDate      DATE NOT NULL,
    Reason       VARCHAR(500),
    Status       VARCHAR(20) DEFAULT 'Pending', -- Pending / Approved / Rejected
    AppliedOn    DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId) ON DELETE CASCADE
);

-- Seed employees
INSERT INTO Employees (FullName, Email, Department) VALUES
('Arjun Sharma',  'arjun@company.com',  'Engineering'),
('Priya Mehta',   'priya@company.com',  'HR'),
('Rohan Verma',   'rohan@company.com',  'Finance'),
('Sneha Kapoor',  'sneha@company.com',  'Marketing');

-- Seed leave requests
INSERT INTO LeaveRequests (EmployeeId, LeaveType, StartDate, EndDate, Reason, Status) VALUES
(1, 'Sick',   '2025-06-01', '2025-06-03', 'Fever and rest',         'Approved'),
(2, 'Annual', '2025-06-10', '2025-06-15', 'Family vacation',        'Pending'),
(3, 'Casual', '2025-06-05', '2025-06-05', 'Personal work',          'Rejected'),
(4, 'Sick',   '2025-06-08', '2025-06-09', 'Doctor appointment',     'Pending');
