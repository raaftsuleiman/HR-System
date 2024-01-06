using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Data
{
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employee> employees { get; set; }

        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public virtual Department department { get; set; }
    }
}
