using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace HR.Controllers
{
    
    public class EmployeeController : Controller
    {
         private readonly ICityService cityservice;
         private readonly ICountryService countryService;
         private readonly IDepartmentService departmentService;
         private readonly IPositionService positionService;
         private readonly IEmployeeService employeeService;
         private readonly IAccountService accountService;
    
        public EmployeeController(ICityService _cityservice, ICountryService _countryService , IDepartmentService _departmentService , IPositionService _positionService, IEmployeeService _employeeService, IAccountService _accountService) 
        {
            cityservice = _cityservice;
            countryService = _countryService;
            departmentService = _departmentService;
            positionService = _positionService;
            employeeService = _employeeService;
            accountService = _accountService;
         
        }
        //[Authorize(Roles = "Admin,Employee")]
          [Authorize(Roles ="Admin")]
        public IActionResult Index()
        {
            List<CityDTO> cityDTO = new List<CityDTO>();
            List<EmployeeDTO> employee = new List<EmployeeDTO>();
            List<PositionDTO> positions= new List<PositionDTO>();
            VmEmployeeAndRelatedEntities vm = new VmEmployeeAndRelatedEntities();
            vm.country = countryService.ListCountry();
            vm.city = cityDTO;
            vm.department= departmentService.ListDepartment();
            vm.employees = employee;
            vm.position = positions;


            return View("NewEmployee",vm);
        }
        [Authorize(Roles = "Employee")]
        public IActionResult MyProfile()
        {
           
            var employee = employeeService.GetEmployeeByUserId(User.FindFirstValue(ClaimTypes.NameIdentifier));

           
            var userProfileViewModel = new VmUserProfile
            {
                employee = new EmployeeDTO
                {
                    ImageName = employee.ImageName,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    PhoneNumber = employee.PhoneNumber,
                    Gender = employee.Gender,
                    countries = employee.countries,
                    cities= employee.cities,
                    department= employee.department,
                    positions= employee.positions,
                    Salary = employee.Salary,
                    DateOfBirth= employee.DateOfBirth,
                    HireDate= employee.HireDate,
                 
                },

                user = new UsersDTO
                {
                    Username = employee.applicationUser.UserName
                }
               
            };
            return View("MyProfile", userProfileViewModel);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddNewEmployee(VmEmployeeAndRelatedEntities vm)
        {

            List<EmployeeDTO> employees = new List<EmployeeDTO>();

            
            ModelState.Remove("employee.id");
            ModelState.Remove("country");
            ModelState.Remove("city");
            ModelState.Remove("department");
            ModelState.Remove("position");      
            ModelState.Remove("employees");
            ModelState.Remove("signUpModel");  
            ModelState.Remove("employee.ImageName");
            ModelState.Remove("employee.department");
            ModelState.Remove("employee.countries");
            ModelState.Remove("employee.cities");
            ModelState.Remove("employee.positions");
            ModelState.Remove("employee.applicationUser");




            if (true)
            {

                string extention = vm.employee.iformfile.FileName.Split('.')[1];
                string guid = Guid.NewGuid().ToString();
                string fileName = guid + "." + extention;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
                vm.employee.iformfile.CopyTo(new FileStream(path, FileMode.Create));

                vm.employee.ImageName = fileName;


              
                var (createResult, generatedUserId) = await accountService.CreateAccount(vm);

                ViewData["result"] = createResult;
                if (createResult.Succeeded)
                {
                    vm.employee.ApplicationUserId = generatedUserId;
                    employeeService.AddEmployee(vm);
                }

                

            }
            List<CityDTO> cityDtos = new List<CityDTO>();
            List<PositionDTO> positions = new List<PositionDTO>();
            SignUpModel _sgnUpModel = new SignUpModel();
            vm.country = countryService.ListCountry();
            vm.city = cityDtos;
            vm.position= positions;
            vm.department = departmentService.GetDepartments();
            vm.employees = employees;
            vm.signUpModel= _sgnUpModel;


            vm.employee = null;
            ModelState.Clear();

            return View("NewEmployee",vm);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult EditEmployee(int id) 
        {
            try
            {
               
                EmployeeDTO employeeDTO = employeeService.getUserById(id);

                if (employeeDTO == null)
                {
                    
                    return NotFound(); 
                }

                
                SignUpModel smodel = employeeService.getEmployeeUserById(employeeDTO.ApplicationUserId);

                
                VmEmployeeAndRelatedEntities vm = new VmEmployeeAndRelatedEntities();
                vm.country = countryService.ListCountry();
                vm.city = cityservice.ListOfCity();
                vm.position = positionService.ListOfPosition();
                vm.department = departmentService.GetDepartments();
                vm.signUpModel = smodel;
                vm.employee = employeeDTO;

                
                return View("NewEmployee", vm);
            }
            catch (Exception ex)
            {
               
                return RedirectToAction("GetAllEmployee", "Employee"); 
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEmployee(VmEmployeeAndRelatedEntities vm)
        {
            List<EmployeeDTO> employeeDTO = new List<EmployeeDTO>();
           

            ModelState.Remove("country");
            ModelState.Remove("city");
            ModelState.Remove("department");
            ModelState.Remove("position");
            ModelState.Remove("employees");
            ModelState.Remove("signUpModel");
            ModelState.Remove("employee.ImageName");
            ModelState.Remove("employee.department");
            ModelState.Remove("employee.countries");
            ModelState.Remove("employee.cities");
            ModelState.Remove("employee.positions");
            ModelState.Remove("employee.applicationUser");


            if (true)
            {

                if (vm.employee.iformfile != null)
                {
                    string extention = vm.employee.iformfile.FileName.Split('.')[1];
                    string guid = Guid.NewGuid().ToString();
                    string fileName = guid + "." + extention;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", fileName);
                    vm.employee.iformfile.CopyTo(new FileStream(path, FileMode.Create));

                    vm.employee.ImageName = fileName;
                }

                var result = await accountService.UpdateAccount(vm);

                ViewData["result"] = result;
                if (result.Succeeded)
                {

                    employeeService.EditEmployee(vm);
                }
               
            }
            List<CityDTO> cityDTO = new List<CityDTO>();
            
            vm.country = countryService.ListCountry();
            vm.city = cityDTO;
            vm.department = departmentService.GetDepartments();
            vm.employees = employeeDTO;
            vm.employee = employeeService.getUserById(vm.employee.Id);
            

           


           return RedirectToAction("GetAllEmployee", "Employee");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult GetAllEmployee(int PageNumber, string search, int? departmentFilter, int? positionFilter, int? countryFilter, string genderFilter)
        {
             ViewBag.SearchTerm = search;
             ViewBag.AllUniquedepartments = departmentService.GetDepartments();
             ViewBag.AllUniqueposition = positionService.GetPositions();
             ViewBag.AllUniquecountry = countryService.ListCountry();

            PaginatedList<EmployeeDTO> listofemloyee;

            string selecteddepartmentFilter = departmentFilter?.ToString();
            string selectedpositionFilter = positionFilter?.ToString();
            string selectedcountryFilter = countryFilter?.ToString();
            string selectedgenderFilter = genderFilter?.ToString();

            if (!string.IsNullOrEmpty(search) || departmentFilter.HasValue || positionFilter.HasValue || countryFilter.HasValue  || !string.IsNullOrEmpty(genderFilter))
            {
               
                var (paginatedList, totalCount) = employeeService.SearchEmployees(search, PageNumber, departmentFilter, positionFilter, countryFilter, genderFilter);

                listofemloyee = paginatedList;
                ViewBag.FilteredEmployeeCount = totalCount; 
            }
            else
            {
                
                listofemloyee = employeeService.LoadAll(PageNumber);
            }

            ViewBag.SelecteddepartmentFilter = selecteddepartmentFilter;
            ViewBag.selectedpositionFilter = selectedpositionFilter;
            ViewBag.selectedcountryFilter = selectedcountryFilter;
            ViewBag.selectedgenderFilter = selectedgenderFilter;

            return View("GetAllEmployee", listofemloyee);
        }

        [Authorize(Roles = "Admin")]
        public  IActionResult Details(int id)
        {
            try
            {
                var a = employeeService.getUserById(id);
                return View("Details", a);
            }
            catch (Exception ex)
            {
                
                return RedirectToAction("GetAllEmployee", "Employee"); 
            }
           
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) 
        {
            
            List<EmployeeDTO> employeeDTO = new List<EmployeeDTO>();

            VmEmployeeAndRelatedEntities vm = new VmEmployeeAndRelatedEntities();
            EmployeeDTO employeeDTOs = employeeService.getUserById(id);
            SignUpModel smodel = employeeService.getEmployeeUserById(employeeDTOs.ApplicationUserId);

            var deleteResult = await accountService.DeleteAccount(smodel.Id);

            ViewData["result"] = deleteResult;
            if (deleteResult.Succeeded)
            {
                employeeService.Delete(id);

            }

            return RedirectToAction("GetAllEmployee", "Employee");
        }

        

        public IActionResult getCityBYCountry(int Country_Id)
        {
            var x = cityservice.ListSearchByCountry(Country_Id);

            return Json(x);
        }
        public IActionResult getPositionBYDepartment(int Department_Id)
        {
            var a = positionService.ListSearchByDepartment(Department_Id);
            return Json(a);
        }



       
    }
}
