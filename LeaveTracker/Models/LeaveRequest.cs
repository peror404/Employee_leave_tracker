// Models/LeaveRequest.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace LeaveTracker.Models
{
    public class LeaveRequest
    {
        public int LeaveId { get; set; }

        [Required]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Please select a leave type.")]
        [Display(Name = "Leave Type")]
        public string LeaveType { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [MaxLength(500)]
        public string Reason { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime AppliedOn { get; set; }

        // Navigation — filled by JOIN query
        public string EmployeeName { get; set; }
        public string Department   { get; set; }

        // Computed helper
        public int TotalDays =>
            (EndDate - StartDate).Days + 1;
    }

    public class Employee
    {
        public int    EmployeeId  { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName    { get; set; }

        [Required, EmailAddress]
        public string Email       { get; set; }

        [Required]
        public string Department  { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
