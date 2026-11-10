using Microsoft.AspNetCore.Identity;
using SurveyBasket.API.Contracts.Authentication;
using System.Security.Cryptography;

namespace SurveyBasket.API.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpiryDays = 14;


        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            //Check User
            var user =await _userManager.FindByEmailAsync(email);
            if (user == null)
                return null;
            //Check Password
            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword) return null;
            //Generate Jwt & Refresh Token
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration =DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            //update db
            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            //Return new AuthResponse
            return new AuthResponse(user.Id,user.Email,user.FirstName,user.LastName,token,expiresIn,refreshToken,refreshTokenExpiration);
        }
        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return null;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken == null) return null;
            userRefreshToken.RevokedOn = DateTime.UtcNow;
            //Generate Jwt & Refresh Token
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            //update db
            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user); 
            //Return new AuthResponse
            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
        }

        //===========
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }

       
    }
}
