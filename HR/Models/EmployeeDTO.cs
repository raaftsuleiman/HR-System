using AutoMapper;
using HR.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Models
{
    [AutoMap(typeof(Employee), ReverseMap = true)]
    public class EmployeeDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Fill First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Fill Last Name")]
        public string LastName { get; set; }
        [RegularExpression(@"^(\0[789]|07[789])\d{7}$", ErrorMessage = "Invalid Format Phone Number")]
        public string PhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set;}

        public IFormFile? iformfile { get; set; }
        public string ImageName { get; set;}

        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        
        public DateTime DateOfBirth { get; set;}

        [Required(ErrorMessage = "Please Fill Gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Fill Salary")]
        [DataType(DataType.Currency)]
        [Range(0, Double.MaxValue, ErrorMessage = "Salary must be a positive value")]
        public double Salary { get; set; }

        public DateTime HireDate { get; set; }

       
        public int DepartmentID { get; set; }
        public  Department department { get; set; }

        public int CountryId { get; set; }
        public  Country countries { get; set; }

        public int CityId { get; set; }
        public  City cities { get; set; }

        public int PositionId { get; set; }
        public  Position positions { get; set; }

        public string ApplicationUserId { get; set; }
        public  ApplicationUser applicationUser { get; set; }
    }
}
