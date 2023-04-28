using Models;
using Models.DTO;
using System.Security.Claims;
using WebAPI.Token;

namespace WebAPI.Abstract
{
    public interface ITokenService
    {
        Task<List<Claim>> GetClaims(UserModel model);
        Task<RefreshTokenRequest> Refresh(RefreshTokenRequest refreshTokenRequest);
        Task<string> GetToken(IEnumerable<Claim> claims);
        Task<string> GetRefreshToken();
        Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    }
}
