using System.ComponentModel.DataAnnotations;

namespace PART_3.Models
{
    public enum ClaimStatus
    {
        Submitted,
        Approved,
        Rejected,
        Paid
    }

    public class Claim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Module Name is required")]
        [StringLength(100, ErrorMessage = "Module Name cannot exceed 100 characters")]
        public string ModuleName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Hours Worked is required")]
        [Range(1, 200, ErrorMessage = "Hours worked must be between 1 and 200")]
        public int HoursWorked { get; set; }

        [Required(ErrorMessage = "Hourly Rate is required")]
        [Range(0, 1000, ErrorMessage = "Hourly rate must be between 0 and 1000")]
        public int HourlyRate { get; set; }

        public decimal TotalAmount => HoursWorked * HourlyRate;

        [Required(ErrorMessage = "Claim Date is required")]
        [DataType(DataType.Date)]
        public DateTime ClaimDate { get; set; }

        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        public string? Comments { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }

        // Navigation property - make it optional
        public Lecturer? Lecturer { get; set; }

        // REMOVED: [Required] attribute - LecturerId will be set in controller
        public int LecturerId { get; set; }
    }
}