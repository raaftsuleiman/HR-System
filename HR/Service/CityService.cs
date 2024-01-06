using AutoMapper;
using HR.Data;
using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.Service
{
    public class CityService :ICityService
    {
        private readonly HRContext _hRContext;
        private readonly IMapper _mapper;
        City city;
        

        public CityService(HRContext hRContext, IMapper mapper)

        {
            _hRContext = hRContext;
            _mapper = mapper;
            city= new City();
            
        }
        public List<CityDTO> ListOfCity()
        {
            List<City> allCity = _hRContext.cities.ToList();
            List<CityDTO> listCityDTO = _mapper.Map<List<CityDTO>>(allCity);

            return listCityDTO;
        }
        public List<CityDTO> ListSearchByCountry(int Country_Id)
        {
            List<City> allCity = _hRContext.cities.Where(e => e.CountryId == Country_Id).ToList();
            List<CityDTO> listcitDto = _mapper.Map<List<CityDTO>>(allCity);

            return listcitDto;
        }

        public void SaveCity(VmCountryCity vm)
        {
             city = _mapper.Map<City>(vm.cityDTO);
            _hRContext.cities.Add(city);
            _hRContext.SaveChanges();
        }

        public PaginatedList<CityDTO> LoadAllCity(int PageNumber)
        {
            List<City> allcities = _hRContext.cities.Include(c => c.countries).ToList();

            List<CityDTO> listcityDTO = _mapper.Map<List<CityDTO>>(allcities);

            return PaginatedList<CityDTO>.CreatePagnation(listcityDTO, PageNumber);
        }

        public (PaginatedList<CityDTO>, int) SearchCity(string searchTerm, int PageNumber, int? countryFilter)
        {
            var query = _hRContext.cities.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm));
            }           

            if (countryFilter.HasValue)
            {
                query = query.Where(e => e.CountryId == countryFilter.Value);
            }          

            var filteredCity = query.ToList();

            List<CityDTO> filteredcityDTO = _mapper.Map<List<CityDTO>>(filteredCity);

            var paginatedList = PaginatedList<CityDTO>.CreatePagnation(filteredcityDTO, PageNumber);

            return (paginatedList, filteredCity.Count());
        }

        public void Delete(int id)
        {
             city = _hRContext.cities.Find(id);
 
            _hRContext.cities.Remove(city);

            _hRContext.SaveChanges();
        }

        public bool DeleteCities(int id)
        {
            var citiesToDelete = _hRContext.cities.Where(c => c.CountryId == id).ToList();

            if (citiesToDelete.Any())
            {
                _hRContext.cities.RemoveRange(citiesToDelete);

                _hRContext.SaveChanges();

                return true;
            }

            return false;
        }


        public CityDTO getCityById(int id)
        {

                  city = _hRContext.cities.Include(c => c.countries).FirstOrDefault(e => e.Id == id);

                 CityDTO cityDTO = _mapper.Map<CityDTO>(city);

            return cityDTO;
        }

        public void EditCity(VmCountryCity vm)
        {
             city = _mapper.Map<City>(vm.cityDTO);

            _hRContext.cities.Attach(city);

            _hRContext.Entry(city).State = EntityState.Modified;

            _hRContext.SaveChanges();

        }

    }
}
