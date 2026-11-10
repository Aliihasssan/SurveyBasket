
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.API.Contracts.Authentication
{
    public class JwtProvider : IJwtProvider
    {
        public (string token, int expiresIn) GenerateToken(ApplicationUser user)
        {
            Claim[] claims = [
                new (JwtRegisteredClaimNames.Sub,user.Id),
                new(JwtRegisteredClaimNames.Email,user.Email),
                new(JwtRegisteredClaimNames.GivenName,user.FirstName),
                new(JwtRegisteredClaimNames.FamilyName,user.LastName),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                ];
            //-------------------------

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kfdfdf"));
            var signingCredintials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var expiresIn = 30;
            //--------------------
            var token = new JwtSecurityToken
                   (
                        issuer : "SurveyBasketApp",
                        audience : "SurveyBasketUsers",
                        claims : claims,
                        expires : DateTime.UtcNow.AddMinutes(expiresIn),
                        signingCredentials : signingCredintials
                   );
            //--------------------------

            return (token : new JwtSecurityTokenHandler().WriteToken(token) , expiresIn : expiresIn *60);


        }

        public string? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kfdfdf"));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = symmetricSecurityKey,
                    ValidateIssuerSigningKey=true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew =TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                return jwtToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            }
            catch
            {
                return null;
            }
        }
    }
}

