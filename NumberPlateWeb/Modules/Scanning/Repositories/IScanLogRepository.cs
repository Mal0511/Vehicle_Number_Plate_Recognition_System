using NumberPlateWeb.Modules.Scanning.Models;

namespace NumberPlateWeb.Modules.Scanning.Repositories;

public interface IScanLogRepository
{
    IReadOnlyCollection<ScanLog> GetAll();
    ScanLog Add(ScanLog log);
}
