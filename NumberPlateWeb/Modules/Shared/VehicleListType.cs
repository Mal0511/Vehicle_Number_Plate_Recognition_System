namespace NumberPlateWeb.Modules.Shared;

public enum VehicleListType
{
    Blacklist = 1,
    Whitelist = 2
}

public static class VehicleListTypeExtensions
{
    public static string ToDisplayName(this VehicleListType type)
    {
        return type switch
        {
            VehicleListType.Blacklist => "Blacklist",
            VehicleListType.Whitelist => "Whitelist",
            _ => type.ToString()
        };
    }
}
