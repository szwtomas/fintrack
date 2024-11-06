using System.Text.Json.Serialization;

namespace Fintrack.Dto;

public class SignUpRequest
{
    [JsonPropertyName("email")] public string Email { get; set; } = null!;
    [JsonPropertyName("password")] public string Password { get; set; } = null!;
}