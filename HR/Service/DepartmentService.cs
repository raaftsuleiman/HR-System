using AutoMapper;
using HR.Data;
using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.Service
{
    public class DepartmentService :IDepartmentService
    {
        private readonly HRContext _hRContext;
        private readonly IMapper _mapper;
        Department department;

        public DepartmentService(HRContext hRContext,IMapper mapper) 
        
        {
            _hRContext = hRContext;
            _mapper = mapper;
            department = new Department();
        }

        public void SaveDepartment(DepartmentDTO departmentDTO)
        {
            department = _mapper.Map<Department>(departmentDTO);
            _hRContext.departments.Add(department);
            _hRContext.SaveChanges();
        }

        public List<DepartmentDTO> ListDepartment()
        {
            List<Department> allDepartment = _hRContext.departments.ToList();
            List<DepartmentDTO> listDepartmentDTO = _mapper.Map<List<DepartmentDTO>>(allDepartment);

            return listDepartmentDTO;
        }

        public List<DepartmentDTO> GetDepartments()
        {
          

            List<Department> allDepartment = (from dept in _hRContext.departments
                                              select dept).ToList();
            List<DepartmentDTO> departmentList = _mapper.Map<List<DepartmentDTO>>(allDepartment);

            return departmentList;
        }


        public PaginatedList<DepartmentDTO> LoadAllDepartment(int PageNumber)
        {
            List<Department> alldepartment = _hRContext.departments.ToList();

            List<DepartmentDTO> listdepartmentDTO = _mapper.Map<List<DepartmentDTO>>(alldepartment);

            return PaginatedList<DepartmentDTO>.CreatePagnation(listdepartmentDTO, PageNumber);
        }


        public (PaginatedList<DepartmentDTO>, int) SearchDepartment(string searchTerm, int PageNumber)
        {
            var query = _hRContext.departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm));
            }
            
            var filteredDepartments = query.ToList();

            List<DepartmentDTO> filteredDepartmentDTO = _mapper.Map<List<DepartmentDTO>>(filteredDepartments);

            var paginatedList = PaginatedList<DepartmentDTO>.CreatePagnation(filteredDepartmentDTO, PageNumber);

            return (paginatedList, filteredDepartments.Count());
        }

        public void Delete(int id)
        {
            Department dep = _hRContext.departments.Find(id);

            _hRContext.departments.Remove(dep);

            _hRContext.SaveChanges();
        }

        public DepartmentDTO getDepartmentById(int id)
        {

            Department dep = _hRContext.departments.FirstOrDefault(e => e.Id == id);

            DepartmentDTO departmentDTO = _mapper.Map<DepartmentDTO>(dep);

            return departmentDTO;
        }

        public void EditDepartment(DepartmentDTO departmentDTO)
        {
             Department dep = _mapper.Map<Department>(departmentDTO);

            _hRContext.departments.Attach(dep);

            _hRContext.Entry(dep).State = EntityState.Modified;

            _hRContext.SaveChanges();

        }

    }
}
