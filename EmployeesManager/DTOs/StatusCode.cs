namespace EmployeesManager.DTOs
{
    public class StatusCode
    {
        public int Code { get; }
        public string Description { get; }

        public StatusCode(int code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
