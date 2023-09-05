using EmployeesManager.Models;

namespace EmployeesManager.DTOs
{
    public class StatusWithContentDTO<T>
    {
        public StatusCode Status { get; set; }
        public T? Content { get; set; }

        public StatusWithContentDTO(StatusCode status, T? content)
        {
            Status = status;
            Content = content;
        }
    }
}
