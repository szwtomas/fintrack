using System.Text.Json.Serialization;

namespace Fintrack.Dto;

public class UserDto
{
    [JsonPropertyName("userId")] public int UserId { get; set; }
    [JsonPropertyName("email")] public string Email { get; set; } = null!;
}