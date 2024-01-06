using AutoMapper;
using HR.Data;

namespace HR.Models
{
    [AutoMap(typeof(LeaveRequest), ReverseMap = true)]
    public class LeaveRequestDTO
    {
        public int id { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }
        public int EmployeeID { get; set; }
        public  Employee employee { get; set; }
    }
}
