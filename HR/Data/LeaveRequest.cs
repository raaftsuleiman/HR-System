using System.ComponentModel.DataAnnotations;

namespace HR.Data
{
    public class LeaveRequest
    {
        [Key]
        public int id { get; set; }
        public string LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public int EmployeeID { get; set; }
        public virtual Employee employee { get; set; }
    }
}
