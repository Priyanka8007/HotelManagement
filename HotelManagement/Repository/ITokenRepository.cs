
using Microsoft.AspNetCore.Identity;

namespace HotelManagement.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
