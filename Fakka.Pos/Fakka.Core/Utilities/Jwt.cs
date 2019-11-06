using System.IdentityModel.Tokens.Jwt;

namespace Fakka.Core.Utilities
{
    public static class Jwt
    {
        public static JwtSecurityToken DecodeBearerToken(string bearerToken)
        {

            return new JwtSecurityTokenHandler().ReadJwtToken(bearerToken);
        }

    }
}
