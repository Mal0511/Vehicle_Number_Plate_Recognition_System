using NumberPlateWeb.Modules.PoliceManagement.Models;

namespace NumberPlateWeb.Modules.PoliceManagement.Repositories;

public interface IPoliceRepository
{
    IReadOnlyCollection<PoliceOfficer> GetAll();
    PoliceOfficer? Find(int id);
    PoliceOfficer Add(PoliceOfficer officer);
    void Update(PoliceOfficer officer);
}
