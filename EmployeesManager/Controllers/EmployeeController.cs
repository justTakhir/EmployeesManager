using EmployeesManager.DTOs;
using EmployeesManager.Models;
using EmployeesManager.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

using StatusCodes = EmployeesManager.Utils.StatusCodes;

namespace EmployeesManager.Controllers
{
    [ApiController]
    [Route("employees")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeListContext _context;

        public EmployeeController(EmployeeListContext context)
        {
            _context = context;
        }

        [HttpGet("hire")]
        public StatusCode HireEmployee(string firstName, string middleName, string lastName, string jobTitle, int? supervisorID)
        {
            Employee? supervisor = null;
            if (supervisorID.HasValue)
            {
                supervisor = _context.Employees.SingleOrDefault(e => e.ID == supervisorID);
                if (supervisor == null)
                {
                    return StatusCodes.INVALID_SUPERVISOR_ID;
                }
                if (supervisor.IsFired)
                {
                    return StatusCodes.FIRED_SUPERVISOR;
                }
            }
            else
            {
                var isDirectorExists = _context.Employees.Any(e => e.Supervisor == null && !e.IsFired);
                if (isDirectorExists)
                {
                    return StatusCodes.DIRECTOR_EXISTS;
                }
            }

            _context.Add(new Employee(firstName, middleName, lastName, jobTitle, supervisor));
            _context.SaveChanges();

            return StatusCodes.OK;
        }

        [HttpGet("delete")]
        public StatusCode DeleteEmployee(int ID)
        {
            Employee? employee = _context.Employees.SingleOrDefault(e => e.ID == ID);
            if (employee == null)
            {
                return StatusCodes.INVALID_EMPLOYEE_ID; 
            }

            if (!employee.IsFired)
            {
                return StatusCodes.NOT_FIRED_EMPLOYEE;
            }

            _context.Employees.Remove(employee);
            _context.SaveChanges();

            return StatusCodes.OK;
        }

        [HttpGet("employee")]
        public StatusWithContentDTO<EmployeeDTO> GetEmployee(int ID)
        {
            var status = StatusCodes.OK;

            var employee = _context.Employees.SingleOrDefault(e => e.ID == ID);
            if (employee == null)
            {
                status = StatusCodes.INVALID_EMPLOYEE_ID;
                return new StatusWithContentDTO<EmployeeDTO>(status, null);
            }

            _context.Entry(employee)
                .Reference(s => s.Supervisor)
                .Load();

            var content = employee.ToDTO();

            _context.SaveChanges();

            return new StatusWithContentDTO<EmployeeDTO>(status, content);
        }


        [HttpGet("fire")]
        public StatusCode FireEmployee(int ID, int? newSupervisorID)
        {
            var firedEmployee = _context.Employees.SingleOrDefault(e => e.ID == ID);
            if (firedEmployee == null)
            {
                return StatusCodes.INVALID_EMPLOYEE_ID;
            }
            if (firedEmployee.IsFired)
            {
                return StatusCodes.ALREADY_FIRED;
            }

            _context.Entry(firedEmployee)
                .Reference(e => e.Supervisor)
                .Load();

            var firedEmployeeSupervisor = firedEmployee.Supervisor;

            if (newSupervisorID == null)
            {
                var areSubordinatesExist = _context.Employees.Any(e => e.Supervisor.ID == ID);
                if (areSubordinatesExist)
                {
                    return StatusCodes.INVALID_SUPERVISOR_ID;
                }
            }
            else
            {
                var newSupervisor = _context.Employees.SingleOrDefault(e => e.ID == newSupervisorID);
                if (newSupervisor == null)
                {
                    return StatusCodes.INVALID_SUPERVISOR_ID;
                }

                _context.Entry(newSupervisor)
                    .Reference(s => s.Supervisor)
                    .Load();

                if (ID != newSupervisor.Supervisor.ID)
                {
                    return StatusCodes.SUPERVISOR_NOT_SUBORDINATE;
                }

                var subordinates = _context.Employees.Where(s => s.Supervisor.ID == ID);
                foreach (var s in subordinates)
                {
                    s.Supervisor = newSupervisor;
                }

                newSupervisor.Supervisor = firedEmployeeSupervisor;
                newSupervisor.JobTitle = firedEmployee.JobTitle;
            }

            firedEmployee.IsFired = true;
            _context.SaveChanges();

            return StatusCodes.OK;
        }

        [HttpGet("getTree")]
        public StatusWithContentDTO<EmployeeNodeDTO> ConstructOutputTree()
        {
            var IDToNode = new Dictionary<int, EmployeeNodeDTO>();
            var notFiredEmployees = _context.Employees.Include(e => e.Supervisor).Where(e => !e.IsFired).ToList();
            foreach(var e in notFiredEmployees)
            {
                IDToNode.Add(e.ID, new EmployeeNodeDTO(e.ToDTO()));
            }

            EmployeeNodeDTO? root = null;
            foreach(var (id, node) in IDToNode)
            {
                if (node.Employee.Supervisor != null)
                {
                    var supervisorID = node.Employee.Supervisor.Value;
                    if (IDToNode.ContainsKey(supervisorID))
                    {
                        IDToNode[supervisorID].Subordinates.Add(node);
                    }
                }
                else
                {
                    root = node;
                }
            }

            if (root == null)
            {
                return new StatusWithContentDTO<EmployeeNodeDTO>(StatusCodes.DIRECTOR_NOT_EXISTS, null);
            }

            return new StatusWithContentDTO<EmployeeNodeDTO>(StatusCodes.OK, root);
        }

    }
}
