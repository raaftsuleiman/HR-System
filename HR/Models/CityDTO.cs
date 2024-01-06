using HR.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using AutoMapper;

namespace HR.Models
{
    [AutoMap(typeof(City), ReverseMap = true)]
    public class CityDTO
    {
       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public int CountryId { get; set; }
        public  Country countries { get; set; }
        
    }
}
