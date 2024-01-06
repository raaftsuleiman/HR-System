using AutoMapper;
using HR.Data;
using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    [AutoMap(typeof(Department), ReverseMap = true)]
    public class DepartmentDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please Fill Department Name")]
        public string Name { get; set; }
    }
}
