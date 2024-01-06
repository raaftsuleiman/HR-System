namespace HR.Models
{
    public class VmCountryCity
    {
        public CityDTO cityDTO { get; set; }

        public CountryDTO count { get; set; }
        public List<CountryDTO> country { get; set; }
        public List<CityDTO> Cities { get; set; }
    }
}
