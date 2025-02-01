namespace AuthService.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Token)> LoginAsync(string email, string password);
        Task<(bool Success, string[] Errors)> RegisterAsync(string email, string password, string username);
        Task<bool> ValidateTokenAsync(string token);
    }
}
