using NumberPlateWeb.Modules.Auth.Models;
using NumberPlateWeb.Modules.Auth.Repositories;
using NumberPlateWeb.Modules.Auth.ViewModels;

namespace NumberPlateWeb.Modules.Auth.Services;

public class AuthService
{
    private readonly IAuthRepository _repository;

    public AuthService(IAuthRepository repository)
    {
        _repository = repository;
    }

    public AppUser? Login(LoginRequest request)
    {
        return _repository.FindByCredentials(request.Username, request.Password);
    }

    public int UserCount()
    {
        return _repository.GetAll().Count;
    }
}
