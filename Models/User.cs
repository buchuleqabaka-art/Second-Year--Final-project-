namespace PART_3.Models
{
    public enum UserRole
    {
        Lecturer,
        Coordinator,
        AcademicManager,
        HR
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // In real app, use hashed passwords
        public string Email { get; set; }
        public UserRole Role { get; set; }
        public int? LecturerId { get; set; } // For lecturer users
    }
}