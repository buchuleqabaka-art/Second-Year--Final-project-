using PART_3.Models;

namespace PART_3.Services
{
    public class InMemoryDataService : IDataService
    {
        private readonly List<Lecturer> _lecturers;
        private readonly List<Claim> _claims;
        private readonly List<User> _users;
        private int _lecturerIdCounter = 1;
        private int _claimIdCounter = 1;
        private int _userIdCounter = 1;

        public InMemoryDataService()
        {
            // Initialize with sample data
            _lecturers = new List<Lecturer>();
            _claims = new List<Claim>();
            _users = new List<User>();

            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            // Add sample lecturers
            var lecturer1 = new Lecturer
            {
                Id = _lecturerIdCounter++,
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@university.com",
                Department = "Computer Science",
                PhoneNumber = "123-456-7890"
            };
            var lecturer2 = new Lecturer
            {
                Id = _lecturerIdCounter++,
                FirstName = "Sarah",
                LastName = "Johnson",
                Email = "sarah.johnson@university.com",
                Department = "Mathematics",
                PhoneNumber = "123-456-7891"
            };

            _lecturers.Add(lecturer1);
            _lecturers.Add(lecturer2);

            // Add sample claims - WITH Lecturer field properly set
            _claims.Add(new Claim
            {
                Id = _claimIdCounter++,
                LecturerId = 1,
                ModuleName = "Advanced Programming",
                HoursWorked = 40,
                HourlyRate = 150,
                ClaimDate = DateTime.Now.AddDays(-10),
                Status = ClaimStatus.Approved,
                Lecturer = lecturer1  // Lecturer field SET
            });

            _claims.Add(new Claim
            {
                Id = _claimIdCounter++,
                LecturerId = 2,
                ModuleName = "Calculus I",
                HoursWorked = 35,
                HourlyRate = 140,
                ClaimDate = DateTime.Now.AddDays(-5),
                Status = ClaimStatus.Submitted,
                Lecturer = lecturer2  // Lecturer field SET
            });

            // Add sample users
            _users.Add(new User
            {
                Id = _userIdCounter++,
                Username = "lecturer1",
                Password = "password",
                Email = "john.smith@university.com",
                Role = UserRole.Lecturer,
                LecturerId = 1
            });

            _users.Add(new User
            {
                Id = _userIdCounter++,
                Username = "coordinator",
                Password = "password",
                Email = "coordinator@university.com",
                Role = UserRole.Coordinator
            });

            _users.Add(new User
            {
                Id = _userIdCounter++,
                Username = "manager",
                Password = "password",
                Email = "manager@university.com",
                Role = UserRole.AcademicManager
            });

            _users.Add(new User
            {
                Id = _userIdCounter++,
                Username = "hr",
                Password = "password",
                Email = "hr@university.com",
                Role = UserRole.HR
            });
        }

        // Lecturer operations
        public List<Lecturer> GetLecturers() => _lecturers;

        public Lecturer GetLecturer(int id) => _lecturers.FirstOrDefault(l => l.Id == id);

        public void AddLecturer(Lecturer lecturer)
        {
            lecturer.Id = _lecturerIdCounter++;
            _lecturers.Add(lecturer);
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            var existing = GetLecturer(lecturer.Id);
            if (existing != null)
            {
                existing.FirstName = lecturer.FirstName;
                existing.LastName = lecturer.LastName;
                existing.Email = lecturer.Email;
                existing.Department = lecturer.Department;
                existing.PhoneNumber = lecturer.PhoneNumber;
            }
        }

        public void DeleteLecturer(int id)
        {
            var lecturer = GetLecturer(id);
            if (lecturer != null)
                _lecturers.Remove(lecturer);
        }

        // Claim operations
        public List<Claim> GetClaims() => _claims;

        public Claim GetClaim(int id) => _claims.FirstOrDefault(c => c.Id == id);

        public void AddClaim(Claim claim)
        {
            claim.Id = _claimIdCounter++;
            // SET the Lecturer field based on LecturerId
            claim.Lecturer = GetLecturer(claim.LecturerId);
            _claims.Add(claim);

            Console.WriteLine($"New claim added: ID={claim.Id}, Module={claim.ModuleName}, Hours={claim.HoursWorked}, Rate={claim.HourlyRate}, Total=R{claim.TotalAmount}, Lecturer={claim.Lecturer?.FirstName} {claim.Lecturer?.LastName}");
        }

        public void UpdateClaim(Claim claim)
        {
            var existing = GetClaim(claim.Id);
            if (existing != null)
            {
                existing.ModuleName = claim.ModuleName;
                existing.HoursWorked = claim.HoursWorked;
                existing.HourlyRate = claim.HourlyRate;
                existing.ClaimDate = claim.ClaimDate;
                existing.Comments = claim.Comments;
                existing.Status = claim.Status;
                existing.ApprovedDate = claim.ApprovedDate;
                existing.ApprovedBy = claim.ApprovedBy;
                // Also update Lecturer if LecturerId changes
                if (existing.LecturerId != claim.LecturerId)
                {
                    existing.LecturerId = claim.LecturerId;
                    existing.Lecturer = GetLecturer(claim.LecturerId);
                }
            }
        }

        public List<Claim> GetClaimsByLecturer(int lecturerId) =>
            _claims.Where(c => c.LecturerId == lecturerId).ToList();

        public List<Claim> GetClaimsByStatus(ClaimStatus status) =>
            _claims.Where(c => c.Status == status).ToList();

        // User operations
        public User Authenticate(string username, string password) =>
            _users.FirstOrDefault(u => u.Username == username && u.Password == password);

        public User GetUser(string username) =>
            _users.FirstOrDefault(u => u.Username == username);

        public void AddUser(User user)
        {
            user.Id = _userIdCounter++;
            _users.Add(user);
        }

        // Report operations
        public List<Claim> GetApprovedClaimsForPayment() =>
            _claims.Where(c => c.Status == ClaimStatus.Approved).ToList();

        public decimal GetTotalPaymentsPending() =>
            GetApprovedClaimsForPayment().Sum(c => c.TotalAmount);
    }
}