using System.Text.RegularExpressions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Vision.V1;
using NumberPlateWeb.Modules.Shared;

namespace NumberPlateWeb.Modules.ExternalSystems.PlateRecognition;

public partial class GoogleVisionPlateRecognitionGateway : IPlateRecognitionGateway
{
    private readonly GoogleVisionOcrOptions _options;
    private readonly MockPlateRecognitionGateway _fallbackGateway = new();
    private ImageAnnotatorClient? _client;

    public GoogleVisionPlateRecognitionGateway(GoogleVisionOcrOptions options)
    {
        _options = options;
    }

    public async Task<PlateRecognitionResult> RecognizeAsync(string cameraFrameReference, string? capturedImageBase64)
    {
        var imageBytes = DecodeImage(capturedImageBase64);

        if (imageBytes.Length == 0)
        {
            var fallback = await _fallbackGateway.RecognizeAsync(cameraFrameReference, capturedImageBase64);
            fallback.Provider = "Manual input fallback";
            return fallback;
        }

        if (string.IsNullOrWhiteSpace(_options.CredentialPath) || !File.Exists(_options.CredentialPath))
        {
            var fallback = await _fallbackGateway.RecognizeAsync(cameraFrameReference, capturedImageBase64);
            fallback.Provider = "Google Vision key missing - manual fallback";
            fallback.RawPayload = "Missing credential file";
            return fallback;
        }

        try
        {
            var client = GetClient();
            var annotations = await client.DetectTextAsync(Image.FromBytes(imageBytes));
            var rawText = annotations.FirstOrDefault()?.Description ?? string.Empty;
            var plateNumber = ExtractPlateNumber(rawText);

            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                plateNumber = PlateNumberFormatter.Normalize(cameraFrameReference);
            }

            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                plateNumber = "UNKNOWN";
            }

            var confidence = annotations.Count > 1
                ? Convert.ToDecimal(annotations.Skip(1).Max(item => item.Score))
                : 0.90m;

            return new PlateRecognitionResult
            {
                PlateNumber = plateNumber,
                Confidence = confidence,
                Provider = "Google Cloud Vision OCR",
                RawPayload = rawText
            };
        }
        catch (Exception exception)
        {
            var fallback = await _fallbackGateway.RecognizeAsync(cameraFrameReference, capturedImageBase64);
            fallback.Provider = "Google Cloud Vision OCR error - manual fallback";
            fallback.RawPayload = exception.Message;
            return fallback;
        }
    }

    private ImageAnnotatorClient GetClient()
    {
        if (_client is not null)
        {
            return _client;
        }

        _client = new ImageAnnotatorClientBuilder
        {
            GoogleCredential = CredentialFactory
                .FromFile<ServiceAccountCredential>(_options.CredentialPath)
                .ToGoogleCredential()
        }.Build();

        return _client;
    }

    private static byte[] DecodeImage(string? capturedImageBase64)
    {
        if (string.IsNullOrWhiteSpace(capturedImageBase64))
        {
            return [];
        }

        var payload = capturedImageBase64.Trim();
        var commaIndex = payload.IndexOf(',');

        if (commaIndex >= 0)
        {
            payload = payload[(commaIndex + 1)..];
        }

        try
        {
            return Convert.FromBase64String(payload);
        }
        catch (FormatException)
        {
            return [];
        }
    }

    private static string ExtractPlateNumber(string rawText)
    {
        if (string.IsNullOrWhiteSpace(rawText))
        {
            return string.Empty;
        }

        var candidates = rawText
            .Split(['\r', '\n', ' ', '\t'], StringSplitOptions.RemoveEmptyEntries)
            .Select(PlateNumberFormatter.Normalize)
            .Where(value => value.Length is >= 5 and <= 10)
            .Where(value => value.Any(char.IsDigit) && value.Any(char.IsLetter))
            .ToList();

        var fullText = PlateNumberFormatter.Normalize(rawText);
        candidates.AddRange(PlatePattern()
            .Matches(fullText)
            .Select(match => PlateNumberFormatter.Normalize(match.Value)));

        return candidates
            .Distinct()
            .OrderByDescending(ScoreCandidate)
            .FirstOrDefault() ?? string.Empty;
    }

    private static int ScoreCandidate(string value)
    {
        var score = 0;

        if (value.Length is >= 7 and <= 9)
        {
            score += 5;
        }

        if (value.Length >= 2 && char.IsDigit(value[0]) && char.IsDigit(value[1]))
        {
            score += 3;
        }

        if (value.Any(char.IsLetter))
        {
            score += 2;
        }

        return score;
    }

    [GeneratedRegex(@"[0-9]{2}[A-Z]{1,2}[0-9]{4,6}", RegexOptions.IgnoreCase)]
    private static partial Regex PlatePattern();
}
