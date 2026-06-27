namespace NumberPlateWeb.Modules.ExternalSystems.PlateRecognition;

public interface IPlateRecognitionGateway
{
    Task<PlateRecognitionResult> RecognizeAsync(string cameraFrameReference);
}
