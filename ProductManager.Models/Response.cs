using static ProductManager.Models.Enums;

namespace ProductManager.Models
{
    public class Response
    {
        public Response()
        {
            this.Result = new { };
        }

        public Response(object result)
        {
            this.Result = result;
        }

        public Status Status { get; set; } = new Status();
        public object Result { get; set; }
    }

    public class Status
    {
        public bool Success { get; set; } = true;
    }

    public class ErrorStatus : Status
    {
        public string DeveloperErrorMsg { get; set; }
        public string FriendlyErrorMsg { get; set; }
        public ErrorTypes ErrorType { get; set; }
    }
}