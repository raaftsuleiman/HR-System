namespace HR.Models
{
    public class VmEmployeeAndRelatedEntities
    {
        
        public EmployeeDTO employee { get; set; }
        public List<CountryDTO> country { get; set; }
        public List<CityDTO> city { get; set; }
        public List<DepartmentDTO> department { get; set; }
        public List<PositionDTO> position { get; set; }
        public List<EmployeeDTO> employees { get; set; }

        public SignUpModel signUpModel { get; set; }
        
    }
}
