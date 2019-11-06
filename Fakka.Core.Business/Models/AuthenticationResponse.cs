using Newtonsoft.Json;

namespace Fakka.Core.Business.Models
{
    public class AuthenticationResponse : BaseModels
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        [JsonProperty("referenceId")]
        public string ReferenceId { get; set; }
    }
}
