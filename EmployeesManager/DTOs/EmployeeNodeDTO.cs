namespace EmployeesManager.DTOs
{
    public class EmployeeNodeDTO
    {
        public EmployeeDTO Employee { get; set; }
        public List<EmployeeNodeDTO> Subordinates { get; set; }

        public EmployeeNodeDTO(EmployeeDTO employee)
        {
            Employee = employee;
            Subordinates = new List<EmployeeNodeDTO>();
        }
    }
}
