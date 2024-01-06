using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountryController : Controller
    {
        private readonly ICountryService countryService;
        private readonly ICityService cityService;
        public CountryController(ICountryService _countryService, ICityService _cityService)
        {
            countryService = _countryService;
            cityService = _cityService;
        }


        public IActionResult Index()
        {
            return View("AddNewCountry");
        }

        public IActionResult SaveCountry(CountryDTO countryDTO)
        {
            countryService.SaveCountry(countryDTO);

            return View("AddNewCountry");
        }

        public IActionResult GetAllCountries(int PageNumber, string search)
        {
            ViewBag.SearchTerm = search;

            PaginatedList<CountryDTO> listofCountry;

            if (!string.IsNullOrEmpty(search))
            {

                var (paginatedList, totalCount) = countryService.SearchCountry(search, PageNumber);

                listofCountry = paginatedList;
                ViewBag.FilteredCountryCount = totalCount;
            }
            else
            {

                listofCountry = countryService.LoadAllCountry(PageNumber);
            }

            return View("GetAllCountries", listofCountry);
        }



        public IActionResult Delete(int id)
        {

            CountryDTO countryDTOs = countryService.getCountryById(id);
          

            bool deleteResult =  cityService.DeleteCities(countryDTOs.Id);

          
            if (deleteResult)
            {
                countryService.Delete(id);

            }
            

            return RedirectToAction("GetAllCountries", "Country");
        }


        public IActionResult EditCountry(int id)
        {
            try
            {

                CountryDTO CountryDTO = countryService.getCountryById(id);


                if (CountryDTO == null)
                {

                    return NotFound();
                }


                return View("AddNewCountry", CountryDTO);
            }
            catch (Exception ex)
            {

                return RedirectToAction("GetAllCountries", "Country");
            }
        }

        public IActionResult UpdateCountry(CountryDTO CountryDTO)
        {

            countryService.EditCountry(CountryDTO);

            return RedirectToAction("GetAllCountries", "Country");
        }




    }
}
