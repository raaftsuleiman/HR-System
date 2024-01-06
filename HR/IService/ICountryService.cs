using HR.helper;
using HR.Models;

namespace HR.IService
{
    public interface ICountryService
    {
        List<CountryDTO> ListCountry();
        PaginatedList<CountryDTO> LoadAllCountry(int PageNumber);
        (PaginatedList<CountryDTO>, int) SearchCountry(string searchTerm, int PageNumber);
        void SaveCountry(CountryDTO countryDTO);
        void Delete(int id);
        CountryDTO getCountryById(int id);
        void EditCountry(CountryDTO countryDTO);

        CityDTO getCityByCountryId(int Id);
    }
}
