using NumberPlateWeb.Modules.Shared;

namespace NumberPlateWeb.Modules.ExternalSystems.PlateRecognition;

public class MockPlateRecognitionGateway : IPlateRecognitionGateway
{
    public async Task<PlateRecognitionResult> RecognizeAsync(string cameraFrameReference)
    {
        await Task.Delay(180);

        var normalized = PlateNumberFormatter.Normalize(cameraFrameReference);

        if (string.IsNullOrWhiteSpace(normalized))
        {
            normalized = "UNKNOWN";
        }

        var confidence = cameraFrameReference.Contains("blur", StringComparison.OrdinalIgnoreCase)
            ? 0.74m
            : 0.96m;

        return new PlateRecognitionResult
        {
            PlateNumber = normalized,
            Confidence = confidence,
            Provider = "External OCR Camera API",
            RawPayload = cameraFrameReference
        };
    }
}
