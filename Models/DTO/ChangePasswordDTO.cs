using Newtonsoft.Json;

namespace ProjetWeb.Models.DTO
{
    public class ChangePasswordDto
    {
        [JsonProperty("oldPassword")] public string OldPassword { get; set; }
        [JsonProperty("newPassword")] public string NewPassword { get; set; }
    }
}