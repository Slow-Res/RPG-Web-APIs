namespace Project.Data
{
    public interface IAuthRepository
    {
         Task<ServiceResponse<int>> Register(User user, string passowrd);
         Task<ServiceResponse<string>> Login(string username, string passowrd);
         Task<ServiceResponse<bool>> Logout(string username);
         Task<bool> UserExists(string username);

    }
}