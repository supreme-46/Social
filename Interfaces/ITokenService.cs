using Social.Entities;

namespace Social.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
