using HR.helper;
using HR.Models;

namespace HR.IService
{
    public interface IDepartmentService
    {
        void SaveDepartment(DepartmentDTO departmentDTO);
        List<DepartmentDTO> ListDepartment();
        List<DepartmentDTO> GetDepartments();

        PaginatedList<DepartmentDTO> LoadAllDepartment(int PageNumber);
        (PaginatedList<DepartmentDTO>, int) SearchDepartment(string searchTerm, int PageNumber);
        void Delete(int id);
        DepartmentDTO getDepartmentById(int id);

        void EditDepartment(DepartmentDTO departmentDTO);


    }
}
