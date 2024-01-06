using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Data
{
    [Table("Countries")]
    public class Country
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Employee> employees { get; set; }
        public List<City> cities { get; set; }

    }
}
