using HR.Data;
using HR.helper;
using HR.Models;

namespace HR.IService
{
    public interface IEmployeeService
    {
        void AddEmployee(VmEmployeeAndRelatedEntities vm);
        PaginatedList<EmployeeDTO> LoadAll(int PageNumber);
        void EditEmployee(VmEmployeeAndRelatedEntities vm);
        EmployeeDTO getUserById(int id);

        void Delete(int id);

        (PaginatedList<EmployeeDTO>, int) SearchEmployees(string searchTerm, int PageNumber, int? departmentFilter, int? positionFilter, int? countryFilter,string genderFilter);
        SignUpModel getEmployeeUserById(String Id);
        Employee GetEmployeeByUserId(string userId);
    }
}
