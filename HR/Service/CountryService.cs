using AutoMapper;
using HR.Data;
using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.Service
{
    public class CountryService : ICountryService
    {

        private readonly HRContext _hRContext;
        private readonly IMapper _mapper;
        Country country;


        public CountryService(HRContext hRContext, IMapper mapper)

        {
            _hRContext = hRContext;
            _mapper = mapper;
            country = new Country(); 

        }
        public List<CountryDTO> ListCountry()
        {
            List<Country> allCountry = _hRContext.countries.ToList();
            List<CountryDTO> listcountryDtos = _mapper.Map<List<CountryDTO>>(allCountry);

            return listcountryDtos;
        }

        public void SaveCountry(CountryDTO countryDTO)
        {
            country = _mapper.Map<Country>(countryDTO);
            _hRContext.countries.Add(country);
            _hRContext.SaveChanges();
        }

        public PaginatedList<CountryDTO> LoadAllCountry(int PageNumber)
        {
            List<Country> allCountry = _hRContext.countries.ToList();

            List<CountryDTO> listCountryDTO = _mapper.Map<List<CountryDTO>>(allCountry);

            return PaginatedList<CountryDTO>.CreatePagnation(listCountryDTO, PageNumber);
        }


        public (PaginatedList<CountryDTO>, int) SearchCountry(string searchTerm, int PageNumber)
        {
            var query = _hRContext.countries.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm));
            }

            var filteredCountrys = query.ToList();

            List<CountryDTO> filteredCountryDTO = _mapper.Map<List<CountryDTO>>(filteredCountrys);

            var paginatedList = PaginatedList<CountryDTO>.CreatePagnation(filteredCountryDTO, PageNumber);

            return (paginatedList, filteredCountrys.Count());
        }


        public void Delete(int id)
        {
             country = _hRContext.countries.Find(id);

            _hRContext.countries.Remove(country);

            _hRContext.SaveChanges();
        }

        public CountryDTO getCountryById(int id)
        {

            country = _hRContext.countries.FirstOrDefault(e => e.Id == id);

            CountryDTO countryDTO = _mapper.Map<CountryDTO>(country);

            return countryDTO;
        }

        public CityDTO getCityByCountryId(int Id)
        {

            City city = _hRContext.cities.Include(e => e.countries).FirstOrDefault(e => e.CountryId == Id);

            CityDTO cityDTO = new CityDTO();
            cityDTO.Id = city.Id;
            cityDTO.Name = city.Name;
      

            return cityDTO;
        }

        public void EditCountry(CountryDTO countryDTO)
        {
            country = _mapper.Map<Country>(countryDTO);

            _hRContext.countries.Attach(country);

            _hRContext.Entry(country).State = EntityState.Modified;

            _hRContext.SaveChanges();

        }


    }
}
