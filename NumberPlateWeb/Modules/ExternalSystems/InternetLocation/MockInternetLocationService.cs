namespace NumberPlateWeb.Modules.ExternalSystems.InternetLocation;

public class MockInternetLocationService : IInternetLocationService
{
    public async Task<InternetLocationResult> ResolveLocationAsync(string locationInput)
    {
        await Task.Delay(120);

        var location = string.IsNullOrWhiteSpace(locationInput)
            ? "Unknown location"
            : locationInput.Trim();

        return new InternetLocationResult
        {
            Location = location,
            Provider = "Internet / Location Service",
            InternetAvailable = true
        };
    }
}
