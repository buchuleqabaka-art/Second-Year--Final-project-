using System.ComponentModel.DataAnnotations;

namespace PART_3.Models
{
    public class Lecturer
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Department { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}