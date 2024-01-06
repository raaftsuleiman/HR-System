using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace HR.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PositionController : Controller
    {
        private readonly IPositionService positionService;
        private readonly IDepartmentService departmentService;
        public PositionController(IPositionService _positionService, IDepartmentService _departmentService)
        {
            positionService = _positionService;
            departmentService = _departmentService;
        }

        public IActionResult Index()
        {
            List<PositionDTO> positionDTOs = new List<PositionDTO>();
            VmDepartmentPosition vm = new VmDepartmentPosition();
            vm.departments = departmentService.GetDepartments();
            vm.positions = positionDTOs;

            return View("AddNewPosition", vm);
        }

        public IActionResult AddNewPosition(VmDepartmentPosition vm)
        {

            positionService.SavePosition(vm);

            return RedirectToAction("GetAllPositions");
        }

        public IActionResult GetAllPositions(int PageNumber, string search, int? departmentFilter)
        {
            ViewBag.SearchTerm = search;
            ViewBag.AllUniquedepartments = departmentService.GetDepartments();

            PaginatedList<PositionDTO> listofpositions;

            string selecteddepartmentFilter = departmentFilter?.ToString();

            if (!string.IsNullOrEmpty(search) || departmentFilter.HasValue)
            {
                var (paginatedList, totalCount) = positionService.SearchPosition(search, PageNumber, departmentFilter);

                listofpositions = paginatedList;
                ViewBag.FilteredPositionCount = totalCount;
            }
            else
            {
                listofpositions = positionService.LoadAllPosition(PageNumber);
            }

            ViewBag.SelecteddepartmentFilter = selecteddepartmentFilter;

            return View("GetAllPositions", listofpositions);
        }

        public IActionResult Delete(int id)
        {

            positionService.Delete(id);

            return RedirectToAction("GetAllPositions", "Position");
        }


        public IActionResult EditPosition(int id)
        {
            try
            {

                PositionDTO PositionDTO = positionService.getPositionById(id);


                if (PositionDTO == null)
                {

                    return NotFound();
                }


                VmDepartmentPosition vm = new VmDepartmentPosition();

                vm.departments = departmentService.GetDepartments();

                vm.positionDTO = PositionDTO;


                return View("AddNewPosition", vm);
            }
            catch (Exception ex)
            {

                return RedirectToAction("GetAllPositions", "Position");
            }
        }

        public IActionResult UpdatePosition(VmDepartmentPosition vm)
        {

            positionService.EditPosition(vm);

            return RedirectToAction("GetAllPositions", "Position");
        }
    }
}
