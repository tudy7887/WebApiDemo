using Learning.Model;

namespace Learning.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
