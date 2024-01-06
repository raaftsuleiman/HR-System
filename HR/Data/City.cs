using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR.Data
{
    [Table("Cities")]
    public class City
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Country")]
        public int CountryId { get; set; }
        public virtual Country countries { get; set; }
        public List<Employee> employees { get; set; }

    }
}
