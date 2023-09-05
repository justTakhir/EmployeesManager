using EmployeesManager.Models;

namespace EmployeesManager.DTOs
{
    public class EmployeeDTO
    {
        public EmployeeDTO(int id, string firstName, string middleName, string lastName, string jobTitle, int? supervisor, bool isFired)
        {
            ID = id;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            JobTitle = jobTitle;
            Supervisor = supervisor;
            IsFired = isFired;
        }

        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public int? Supervisor { get; set; }
        public bool IsFired { get; set; }
    }
}
