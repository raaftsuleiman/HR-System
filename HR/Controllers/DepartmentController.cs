using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService departmentService;
        private readonly IPositionService positionService;
        public DepartmentController(IDepartmentService _departmentService, IPositionService _positionService) 
        {
            departmentService = _departmentService;
            positionService = _positionService;
        }
        public IActionResult Index()
        {
            return View("AddNewDepartment");
        }

        public IActionResult SaveDepartment(DepartmentDTO departmentDTO) 
        {
            departmentService.SaveDepartment(departmentDTO);

            return View("AddNewDepartment");
        }
        public IActionResult GetAllDepartment(int PageNumber, string search)
        {
            ViewBag.SearchTerm = search;

            PaginatedList<DepartmentDTO> listofDepartment;

            if (!string.IsNullOrEmpty(search))
            {

                var (paginatedList, totalCount) = departmentService.SearchDepartment(search, PageNumber);

                listofDepartment = paginatedList;
                ViewBag.FilteredDepartmentCount = totalCount;
            }
            else
            {

                listofDepartment = departmentService.LoadAllDepartment(PageNumber);
            }

            return View("GetAllDepartment", listofDepartment);
        }


        public IActionResult Delete(int id)
        {

            DepartmentDTO departmentDTOs = departmentService.getDepartmentById(id);


            bool deleteResult = positionService.DeletePositions(departmentDTOs.Id);


            if (deleteResult)
            {
                departmentService.Delete(id);

            }
            return RedirectToAction("GetAllDepartment", "Department");
        }
            public IActionResult EditDepartment(int id)
        {
            try
            {
                
                DepartmentDTO departmentDTO = departmentService.getDepartmentById(id);

                
                if (departmentDTO == null)
                {
                   
                    return NotFound(); 
                }

                
                return View("AddNewDepartment", departmentDTO);
            }
            catch (Exception ex)
            {
               
                return RedirectToAction("GetAllDepartment", "Department"); 
            }
        }

        public IActionResult UpdateDepartment(DepartmentDTO departmentDTO)
        {

            departmentService.EditDepartment(departmentDTO);

            return RedirectToAction("GetAllDepartment", "Department");
        }


    }
}
