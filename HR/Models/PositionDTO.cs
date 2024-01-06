using AutoMapper;
using HR.Data;

namespace HR.Models
{
    [AutoMap(typeof(Position), ReverseMap = true)]
    public class PositionDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public Department department { get; set; }
    }
}
