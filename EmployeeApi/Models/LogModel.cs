
namespace EmployeeApi.Models
{
    public class LogModel
    {
        public string LogType { get; set; }

        public string LogName { get; set; }

        public string Reference { get; set; }

        public string Request { get; set; }

        public string Response { get; set; }

        public string Url { get; set; }

        public string RequestPath { get; set; }

        public string Message { get; set; }

        public double ResponseTime { get; set; }
    }
}
