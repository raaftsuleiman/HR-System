namespace HR.Models
{
    public class VmDepartmentPosition
    {
        public PositionDTO positionDTO { get; set; }
        public List<DepartmentDTO> departments { get; set; }
        public List<PositionDTO> positions { get; set; }
    }
}
