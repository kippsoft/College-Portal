using Microsoft.AspNetCore.Identity;
namespace College_Portal.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public string? UserType { get; set; } // Admin, Trainer, Trainee
        public string? AdmissionNo { get; set; }
        public string? StaffNo { get; set; }
    }
    public class Trainee
    {
        public int Id { get; set; }

        public string AdmissionNumber { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
    public class ExamRegistrationRequest
    {
        public int Id { get; set; }

        // Trainee
        public string TraineeId { get; set; }
        public ApplicationUser Trainee { get; set; }

        // Unit being registered for
        public int UnitId { get; set; }
        public Unit Unit { get; set; }

        public DateTime RequestDate { get; set; }

        public ExamRequestStatus Status { get; set; }

        // Tutor Approval
        public string? ApprovedByTutorId { get; set; }
        public DateTime? TutorApprovalDate { get; set; }

        // Finance Approval
        public string? ApprovedByFinanceId { get; set; }
        public DateTime? FinanceApprovalDate { get; set; }

        // Finalization
        public string? FinalizedByExamOfficerId { get; set; }
        public DateTime? FinalizationDate { get; set; }

        // Rejection (optional but recommended)
        public string? RejectionReason { get; set; }
        public string? RejectedById { get; set; }
        public DateTime? RejectionDate { get; set; }
    }
    public enum ExamRequestStatus
    {
        PendingTutorApproval = 1,
        PendingFinanceApproval = 2,
        PendingFinalization = 3,
        Approved = 4,
        Rejected = 5
    }
    public class Trainer
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public string? CourseTutorId { get; set; }
        public ApplicationUser? CourseTutor { get; set; }

        public ICollection<Unit> Units { get; set; }
    }
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string? HODId { get; set; }
        public ApplicationUser? HOD { get; set; }

        public ICollection<Course> Courses { get; set; }
    }
    
 
}
