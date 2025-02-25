using System.Text.Json.Serialization;

namespace Portfolio.Models.Responses
{
    public class ProblemDetails
    {
        private string _title = string.Empty;

        [JsonPropertyName("status")]
        public int Status { get; init; } = 0;

        [JsonPropertyName("title")]
        public virtual string Title
        {
            get { return _title; }
            init
            {
                _title = $"🙈 {value}";
            }
        }

        [JsonPropertyName("detail")]
        public virtual string Detail { get; init; } = string.Empty;

        [JsonPropertyName("type")]
        public string? Type { get; init; }

        [JsonPropertyName("instance")]
        public string? Instance { get; init; }
    }
}
