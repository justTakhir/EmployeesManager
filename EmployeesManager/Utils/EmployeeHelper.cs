using EmployeesManager.DTOs;
using EmployeesManager.Models;

namespace EmployeesManager.Utils
{
    public static class EmployeeHelper
    {
        public static EmployeeDTO ToDTO(this Employee employee)
        {
            return new EmployeeDTO(
                employee.ID,
                employee.FirstName,
                employee.MiddleName,
                employee.LastName,
                employee.JobTitle,
                employee.Supervisor?.ID,
                employee.IsFired
            );
        }
    }
}
