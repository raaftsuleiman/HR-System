using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CityController : Controller
    {
        private readonly ICityService cityService;
        private readonly ICountryService countryService;
       public CityController(ICityService _cityService, ICountryService _countryService)
        {
            cityService = _cityService;
            countryService = _countryService;
        }
        public IActionResult Index()
        {
              List<CityDTO> Cities = new List<CityDTO>();
              VmCountryCity vm = new VmCountryCity();
              vm.country = countryService.ListCountry();
              vm.Cities= Cities;

            return View("AddNewCity",vm);
        }

        public IActionResult AddNewCity(VmCountryCity vm)
        {
            
            cityService.SaveCity(vm);

            return RedirectToAction("GetAllCities");
        }

        public IActionResult GetAllCities(int PageNumber, string search, int? countryFilter)
        {
            ViewBag.SearchTerm = search;
            ViewBag.AllUniqueCountries = countryService.ListCountry();

            PaginatedList<CityDTO> listofcities;

            string selectedCountryFilter = countryFilter?.ToString();

            if (!string.IsNullOrEmpty(search) || countryFilter.HasValue)
            {
                var (paginatedList, totalCount) = cityService.SearchCity(search, PageNumber, countryFilter);

                listofcities = paginatedList;
                ViewBag.FilteredCityCount = totalCount;
            }
            else
            {
                listofcities = cityService.LoadAllCity(PageNumber);
            }

            ViewBag.SelectedCountryFilter = selectedCountryFilter;

            return View("GetAllCities", listofcities);
        }

        public IActionResult Delete(int id)
        {

            cityService.Delete(id);

            return RedirectToAction("GetAllCities", "City");
        }


        public IActionResult EditCity(int id)
        {
            try
            {
                
                CityDTO cityDTO = cityService.getCityById(id);

            
                if (cityDTO == null)
                {
                    
                    return NotFound(); 
                }

                         
                VmCountryCity vm = new VmCountryCity();

                vm.country = countryService.ListCountry();

                vm.cityDTO = cityDTO;

                
                return View("AddNewCity", vm);
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("GetAllCities", "City");
            }
        }

        public IActionResult UpdateCity(VmCountryCity vm)
        {
           
            cityService.EditCity(vm);
                
            return RedirectToAction("GetAllCities", "City");
        }





    }
}
