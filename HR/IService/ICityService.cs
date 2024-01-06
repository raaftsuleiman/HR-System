using AutoMapper;
using HR.Data;
using HR.helper;
using HR.Models;

namespace HR.IService
{
    public interface ICityService
    {
        List<CityDTO> ListSearchByCountry(int Country_Id);
        List<CityDTO> ListOfCity();
        void SaveCity(VmCountryCity vm);
        PaginatedList<CityDTO> LoadAllCity(int PageNumber);
        (PaginatedList<CityDTO>, int) SearchCity(string searchTerm, int PageNumber, int? countryFilter);
        void Delete(int id);
        CityDTO getCityById(int id);
        void EditCity(VmCountryCity vm);

        bool DeleteCities(int id);
    }
}
