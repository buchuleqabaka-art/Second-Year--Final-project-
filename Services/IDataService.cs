using PART_3.Models;

namespace PART_3.Services
{
    public interface IDataService
    {
        // Lecturer operations
        List<Lecturer> GetLecturers();
        Lecturer GetLecturer(int id);
        void AddLecturer(Lecturer lecturer);
        void UpdateLecturer(Lecturer lecturer);
        void DeleteLecturer(int id);

        // Claim operations
        List<Claim> GetClaims();
        Claim GetClaim(int id);
        void AddClaim(Claim claim);
        void UpdateClaim(Claim claim);
        List<Claim> GetClaimsByLecturer(int lecturerId);
        List<Claim> GetClaimsByStatus(ClaimStatus status);

        // User operations
        User Authenticate(string username, string password);
        User GetUser(string username);
        void AddUser(User user);

        // Report data
        List<Claim> GetApprovedClaimsForPayment();
        decimal GetTotalPaymentsPending();
    }
}