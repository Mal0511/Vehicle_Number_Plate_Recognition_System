namespace NumberPlateWeb.Modules.ExternalSystems.InternetLocation;

public interface IInternetLocationService
{
    Task<InternetLocationResult> ResolveLocationAsync(string locationInput);
}
