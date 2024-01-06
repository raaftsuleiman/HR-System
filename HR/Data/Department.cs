using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Data
{
    [Table("Departments")]
    public class Department
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Employee> employees { get; set; }
        public List<Position> position { get; set; }
    }
}
