using AutoMapper;
using HR.Data;
using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.Service
{
    public class EmployeeService: IEmployeeService
    {
        private readonly HRContext _hRContext;
        private readonly IMapper _mapper;

        Employee emp;

        public EmployeeService(HRContext hRContext, IMapper mapper)

        {
            _hRContext = hRContext;
            _mapper = mapper;
            emp = new Employee();

        }
        public void AddEmployee(VmEmployeeAndRelatedEntities vm)
        {

            emp = _mapper.Map<Employee>(vm.employee);

         

            _hRContext.employee.Add(emp);
            _hRContext.SaveChanges();


        }
        public void EditEmployee(VmEmployeeAndRelatedEntities vm)
        {
            emp = _mapper.Map<Employee>(vm.employee);

            _hRContext.employee.Attach(emp);

            _hRContext.Entry(emp).State = EntityState.Modified;

            _hRContext.SaveChanges();

        }
        public PaginatedList<EmployeeDTO> LoadAll(int PageNumber)
        {
            List<Employee> allemployee = _hRContext.employee.Include(e => e.department)
                .Include(e => e.positions)
                .Include(e => e.cities)
                .ThenInclude(c => c.countries).ToList();

            List<EmployeeDTO> listemployeeDTO = _mapper.Map<List<EmployeeDTO>>(allemployee);

            return PaginatedList<EmployeeDTO>.CreatePagnation(listemployeeDTO, PageNumber);
        }

        public EmployeeDTO getUserById(int id)
        {

            Employee emp = _hRContext.employee.Include(e => e.department)
                .Include(e => e.positions)
                .Include(e => e.cities)
                .Include(c => c.countries)
                .FirstOrDefault(e => e.Id == id);
              EmployeeDTO employeeDTO = _mapper.Map<EmployeeDTO>(emp);

            return employeeDTO;
        }
        public SignUpModel getEmployeeUserById(String Id)
        {

                Employee emp = _hRContext.employee.Include(e => e.applicationUser).FirstOrDefault(e => e.ApplicationUserId == Id);

                SignUpModel signUpModel = new SignUpModel();
                signUpModel.Id = emp.applicationUser.Id;
                signUpModel.Username = emp.applicationUser.UserName;
                signUpModel.Password = emp.applicationUser.PasswordHash;

                return signUpModel;
        }

        public Employee GetEmployeeByUserId(string userId)
        {
           
            return _hRContext.employee
                .Include(e => e.department)
                .Include(e => e.positions)
                .Include(e => e.cities)
                .Include(c => c.countries)
                .Include(e => e.applicationUser) 
                .FirstOrDefault(e => e.ApplicationUserId == userId);
        }

        public void Delete(int id)
        {
            Employee emp = _hRContext.employee.Find(id);

            _hRContext.employee.Remove(emp);

            _hRContext.SaveChanges();
        }

      

        public (PaginatedList<EmployeeDTO>, int) SearchEmployees(string searchTerm, int PageNumber, int? departmentFilter, int? positionFilter, int? countryFilter, string genderFilter)
        {
            var query = _hRContext.employee
                .Include(e => e.department)
                .Include(e => e.positions)
                .Include(e => e.cities)
                .ThenInclude(c => c.countries)
                .AsQueryable();

            
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.FirstName.Contains(searchTerm) || e.LastName.Contains(searchTerm));
            }

            if (departmentFilter.HasValue)
            {
                query = query.Where(e => e.DepartmentID == departmentFilter.Value);
            }

            if (positionFilter.HasValue)
            {
                query = query.Where(e => e.PositionId == positionFilter.Value);
            }

            if (countryFilter.HasValue)
            {
                query = query.Where(e => e.cities.CountryId == countryFilter.Value);
            }
     

            if (!string.IsNullOrWhiteSpace(genderFilter))
            {
                query = query.Where(e => e.Gender == genderFilter);
            }

            var filteredEmployees = query.ToList();

            List<EmployeeDTO> filteredEmployeeDTOs = _mapper.Map<List<EmployeeDTO>>(filteredEmployees);

            var paginatedList = PaginatedList<EmployeeDTO>.CreatePagnation(filteredEmployeeDTOs, PageNumber);

            return (paginatedList, filteredEmployees.Count());
        }


    }


}
