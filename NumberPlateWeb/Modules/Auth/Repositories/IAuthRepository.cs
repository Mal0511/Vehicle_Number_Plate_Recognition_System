using NumberPlateWeb.Modules.Auth.Models;

namespace NumberPlateWeb.Modules.Auth.Repositories;

public interface IAuthRepository
{
    AppUser? FindByCredentials(string username, string password);
    IReadOnlyCollection<AppUser> GetAll();
}
