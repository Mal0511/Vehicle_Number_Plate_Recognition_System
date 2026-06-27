using NumberPlateWeb.Modules.Auth.Models;

namespace NumberPlateWeb.Modules.Auth.Repositories;

public class InMemoryAuthRepository : IAuthRepository
{
    private readonly List<AppUser> _users =
    [
        new()
        {
            Username = "admin",
            Password = "admin123",
            DisplayName = "System Admin",
            Role = "Admin"
        },
        new()
        {
            Username = "police",
            Password = "police123",
            DisplayName = "Police Officer",
            Role = "Police"
        }
    ];

    public AppUser? FindByCredentials(string username, string password)
    {
        return _users.FirstOrDefault(user =>
            user.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase)
            && user.Password == password);
    }

    public IReadOnlyCollection<AppUser> GetAll()
    {
        return _users.Select(user => new AppUser
        {
            Username = user.Username,
            Password = user.Password,
            DisplayName = user.DisplayName,
            Role = user.Role
        }).ToList();
    }
}
