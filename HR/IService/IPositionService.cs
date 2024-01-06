using HR.helper;
using HR.Models;

namespace HR.IService
{
    public interface IPositionService
    {
        List<PositionDTO> ListSearchByDepartment(int Department_Id);
        List<PositionDTO> ListOfPosition();
        void SavePosition(VmDepartmentPosition vm);
        PaginatedList<PositionDTO> LoadAllPosition(int PageNumber);
        (PaginatedList<PositionDTO>, int) SearchPosition(string searchTerm, int PageNumber, int? departmentFilter);
        void Delete(int id);
        PositionDTO getPositionById(int id);
        void EditPosition(VmDepartmentPosition vm);

        bool DeletePositions(int id);
        List<PositionDTO> GetPositions();
    }
}
