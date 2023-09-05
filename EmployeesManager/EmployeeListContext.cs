using EmployeesManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager
{
    public class EmployeeListContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }

        public EmployeeListContext(DbContextOptions<EmployeeListContext> options) : base(options) {}
    }
}
