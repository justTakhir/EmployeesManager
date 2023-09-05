using EmployeesManager.DTOs;

namespace EmployeesManager.Utils
{
    public static class StatusCodes
    {
        public static readonly StatusCode OK = new StatusCode(0, "OK");
        public static readonly StatusCode INVALID_EMPLOYEE_ID = new StatusCode(1, "Invalid employee ID");
        public static readonly StatusCode INVALID_SUPERVISOR_ID = new StatusCode(2, "Invalid supervisor ID");
        public static readonly StatusCode FIRED_SUPERVISOR = new StatusCode(3, "Attempt to appoint a fired supervisor");
        public static readonly StatusCode DIRECTOR_EXISTS = new StatusCode(4, "Director already exists");
        public static readonly StatusCode NOT_FIRED_EMPLOYEE = new StatusCode(5, "This employee is not fired");
        public static readonly StatusCode ALREADY_FIRED = new StatusCode(6, "This employee is alredy fired");
        public static readonly StatusCode SUPERVISOR_NOT_SUBORDINATE = new StatusCode(7, "New supervisor is not subordinate of fired employee");
        public static readonly StatusCode DIRECTOR_NOT_EXISTS = new StatusCode(8, "Director not exists");
    }
}
