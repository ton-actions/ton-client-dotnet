using System.Text.Json.Serialization;

namespace ch1seL.TonNet.RustAdapter.Models
{
    public class CreateContextResponse : ErrorResponse
    {
        [JsonPropertyName("result")] public uint? ContextNumber { get; set; }
    }
}