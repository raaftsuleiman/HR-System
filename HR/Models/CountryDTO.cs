using AutoMapper;
using HR.Data;
using System.ComponentModel.DataAnnotations;

namespace HR.Models
{
    [AutoMap(typeof(Country), ReverseMap = true)]
    public class CountryDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
