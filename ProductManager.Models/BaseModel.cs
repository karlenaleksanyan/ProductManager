using System.Text.Json.Serialization;
using static ProductManager.Models.Enums;

namespace ProductManager.Models
{
    public class BaseModel
    {
        [JsonIgnore]
        public bool IsValid { get; set; } = true;

        [JsonIgnore]
        public string FriendlyErrorMsg { get; set; }

        [JsonIgnore]
        public string DeveloperErrorMsg { get; set; }

        [JsonIgnore]
        public ErrorTypes ErrorType { get; set; }
    }
}