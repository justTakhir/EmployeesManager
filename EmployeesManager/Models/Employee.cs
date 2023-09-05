using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeesManager.Models
{
    public class Employee
    {
        public Employee() { }
        public Employee(string firstName, string middleName, string lastName, string jobTitle, Employee? supervisor)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            JobTitle = jobTitle;
            Supervisor = supervisor;
        }

        //[Key]
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        //[ForeignKey(nameof(Employee))]
        //public int? Supervisor { get; set; }
        public Employee? Supervisor { get; set; }
        public bool IsFired { get; set; }
    }
}
