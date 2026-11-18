using Microsoft.AspNetCore.Identity;
using SurveyBasket.API.Abstractions.cs;
using SurveyBasket.API.Contracts.Authentication;
using System.Security.Cryptography;

namespace SurveyBasket.API.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpiryDays = 14;


        public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default) 
        {
            //Check User
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            //Check Password
            var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!isValidPassword)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            //Generate Jwt & Refresh Token
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);
            var refreshToken = GenerateRefreshToken();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDays);
            //update db
            user.RefreshTokens.Add(new RefreshTokens
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });
            await _userManager.UpdateAsync(user);

            //Return new AuthResponse
            var responce = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
            return Result.Success(responce);
        }
        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken == null)
                return Result.Failure<AuthResponse>(UserErrors.RefreshTokenNotFound);
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
            var result = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);
            return Result.Success(result);  
        }

        //===========
        public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {
            var userId = _jwtProvider.ValidateToken(token);
            if (userId is null)
                return Result.Failure(UserErrors.InvalidCredentials);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
                return Result.Failure(UserErrors.InvalidCredentials);

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);
            if (userRefreshToken == null)
                return Result.Failure(UserErrors.RefreshTokenNotFound);

            userRefreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
            return Result.Success();
        }
        //===========================================
        private static string GenerateRefreshToken()
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        }


    }
}
