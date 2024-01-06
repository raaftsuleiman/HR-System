using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Data
{
    [Table("Employees")]
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public double Salary { get; set; }
        public DateTime HireDate { get; set; }

        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        public virtual Department department { get; set; }

        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country countries { get; set; }

        [ForeignKey("City")]
        public int CityId { get; set; }
        public virtual City cities { get; set; }

        public List<LeaveRequest> leaveRequests { get; set; }

        [ForeignKey("Position")]
        public int PositionId { get; set; }
        public virtual Position positions { get; set; }

        [ForeignKey("ApplicationUser")]
        public string? ApplicationUserId { get; set; }
        public virtual ApplicationUser applicationUser { get; set; }
    }
}
