using Fakka.Core.Enums;

namespace Fakka.Core.Models
{
    /// <summary>
    /// Object used in ApplicationManager
    /// </summary>
    public class Client
    {
        public Client(string clientId, string scope, string clientSecret, string grantType, UserRole[] validRoles)
        {
            ClientId = clientId;
            Scope= scope;
            ClientSecret = clientSecret;
            GrantType = grantType;
            ValidRoles = validRoles;
        }
        public string ClientId { get; }
        public string Scope { get; }
        public string ClientSecret { get; }
        public string GrantType { get; }
        public UserRole[] ValidRoles { get; }
    }
}