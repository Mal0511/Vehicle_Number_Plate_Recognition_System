namespace NumberPlateWeb.Modules.ExternalSystems.PlateRecognition;

public class PlateRecognitionResult
{
    public string PlateNumber { get; set; } = string.Empty;
    public decimal Confidence { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string RawPayload { get; set; } = string.Empty;
}
