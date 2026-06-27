using NumberPlateWeb.Modules.PoliceManagement.Models;
using NumberPlateWeb.Modules.PoliceManagement.Repositories;
using NumberPlateWeb.Modules.PoliceManagement.ViewModels;

namespace NumberPlateWeb.Modules.PoliceManagement.Services;

public class PoliceService
{
    private readonly IPoliceRepository _repository;

    public PoliceService(IPoliceRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyCollection<PoliceOfficer> GetAll()
    {
        return _repository.GetAll()
            .OrderByDescending(officer => officer.IsActive)
            .ThenBy(officer => officer.FullName)
            .ToList();
    }

    public PoliceOfficer Create(PoliceOfficerInput input)
    {
        var officer = new PoliceOfficer
        {
            FullName = input.FullName.Trim(),
            BadgeNumber = input.BadgeNumber.Trim().ToUpperInvariant(),
            UnitName = input.UnitName.Trim(),
            PhoneNumber = input.PhoneNumber.Trim(),
            Username = input.Username.Trim(),
            IsActive = true
        };

        return _repository.Add(officer);
    }

    public void ToggleActive(int id)
    {
        var officer = _repository.Find(id);

        if (officer is null)
        {
            return;
        }

        officer.IsActive = !officer.IsActive;
        _repository.Update(officer);
    }
}
