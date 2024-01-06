using AutoMapper;
using HR.Data;
using HR.helper;
using HR.IService;
using HR.Models;
using Microsoft.EntityFrameworkCore;

namespace HR.Service
{
    public class PositionService: IPositionService
    {
        private readonly HRContext _hRContext;
        private readonly IMapper _mapper;
        Position position;


        public PositionService(HRContext hRContext, IMapper mapper)

        {
            _hRContext = hRContext;
            _mapper = mapper;
            position= new Position();
        }
        public List<PositionDTO> ListOfPosition()
        {
            List<Position> allPosition = _hRContext.positions.ToList();
            List<PositionDTO> listPositionDTO = _mapper.Map<List<PositionDTO>>(allPosition);

            return listPositionDTO;
        }
        public List<PositionDTO> ListSearchByDepartment(int Department_Id)
        {
            List<Position> allposition = _hRContext.positions.Where(e => e.DepartmentId == Department_Id).ToList();
            List<PositionDTO> listpositionDTO = _mapper.Map<List<PositionDTO>>(allposition);

            return listpositionDTO;
        }


        public void SavePosition(VmDepartmentPosition vm)
        {
             position = _mapper.Map<Position>(vm.positionDTO);
            _hRContext.positions.Add(position);
            _hRContext.SaveChanges();
        }

        public PaginatedList<PositionDTO> LoadAllPosition(int PageNumber)
        {
            List<Position> allpositions = _hRContext.positions.Include(c => c.department).ToList();

            List<PositionDTO> listPositionDTO = _mapper.Map<List<PositionDTO>>(allpositions);

            return PaginatedList<PositionDTO>.CreatePagnation(listPositionDTO, PageNumber);
        }

        public (PaginatedList<PositionDTO>, int) SearchPosition(string searchTerm, int PageNumber, int? departmentFilter)
        {
            var query = _hRContext.positions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(e => e.Name.Contains(searchTerm));
            }

            if (departmentFilter.HasValue)
            {
                query = query.Where(e => e.DepartmentId == departmentFilter.Value);
            }

            var filteredPosition = query.ToList();

            List<PositionDTO> filteredPositionDTO = _mapper.Map<List<PositionDTO>>(filteredPosition);

            var paginatedList = PaginatedList<PositionDTO>.CreatePagnation(filteredPositionDTO, PageNumber);

            return (paginatedList, filteredPosition.Count());
        }

        public void Delete(int id)
        {
            position = _hRContext.positions.Find(id);

            _hRContext.positions.Remove(position);

            _hRContext.SaveChanges();
        }

        public bool DeletePositions(int id)
        {
            var positionsToDelete = _hRContext.positions.Where(c => c.DepartmentId == id).ToList();

            if (positionsToDelete.Any())
            {
                _hRContext.positions.RemoveRange(positionsToDelete);

                _hRContext.SaveChanges();

                return true;
            }

            return false;
        }

        public List<PositionDTO> GetPositions()
        {


            List<Position> allPosition = (from dept in _hRContext.positions
                                              select dept).ToList();
            List<PositionDTO> PositionList = _mapper.Map<List<PositionDTO>>(allPosition);

            return PositionList;
        }

        public PositionDTO getPositionById(int id)
        {

            position = _hRContext.positions.Include(c => c.department).FirstOrDefault(e => e.Id == id);

            PositionDTO PositionDTO = _mapper.Map<PositionDTO>(position);

            return PositionDTO;
        }

        public void EditPosition(VmDepartmentPosition vm)
        {
            position = _mapper.Map<Position>(vm.positionDTO);

            _hRContext.positions.Attach(position);

            _hRContext.Entry(position).State = EntityState.Modified;

            _hRContext.SaveChanges();

        }

    }
}
