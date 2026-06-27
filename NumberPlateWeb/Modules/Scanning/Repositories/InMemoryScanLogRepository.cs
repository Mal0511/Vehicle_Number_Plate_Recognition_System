using NumberPlateWeb.Modules.Scanning.Models;

namespace NumberPlateWeb.Modules.Scanning.Repositories;

public class InMemoryScanLogRepository : IScanLogRepository
{
    private readonly List<ScanLog> _logs = [];
    private readonly object _syncRoot = new();
    private int _nextId = 1;

    public IReadOnlyCollection<ScanLog> GetAll()
    {
        lock (_syncRoot)
        {
            return _logs.Select(Clone).ToList();
        }
    }

    public ScanLog Add(ScanLog log)
    {
        lock (_syncRoot)
        {
            log.Id = _nextId++;
            _logs.Add(Clone(log));
            return Clone(log);
        }
    }

    private static ScanLog Clone(ScanLog log)
    {
        return new ScanLog
        {
            Id = log.Id,
            PlateNumber = log.PlateNumber,
            OfficerName = log.OfficerName,
            Location = log.Location,
            RecognitionProvider = log.RecognitionProvider,
            Confidence = log.Confidence,
            ListStatus = log.ListStatus,
            NotificationSent = log.NotificationSent,
            ScannedAt = log.ScannedAt
        };
    }
}
