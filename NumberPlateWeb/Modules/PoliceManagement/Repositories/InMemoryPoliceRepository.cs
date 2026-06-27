using NumberPlateWeb.Modules.PoliceManagement.Models;

namespace NumberPlateWeb.Modules.PoliceManagement.Repositories;

public class InMemoryPoliceRepository : IPoliceRepository
{
    private readonly List<PoliceOfficer> _officers =
    [
        new()
        {
            Id = 1,
            FullName = "Nguyen Van An",
            BadgeNumber = "P001",
            UnitName = "Traffic Police District 1",
            PhoneNumber = "0901000001",
            Username = "an.p001",
            IsActive = true
        },
        new()
        {
            Id = 2,
            FullName = "Tran Minh Quan",
            BadgeNumber = "P002",
            UnitName = "Traffic Police District 3",
            PhoneNumber = "0901000002",
            Username = "quan.p002",
            IsActive = true
        }
    ];

    private readonly object _syncRoot = new();
    private int _nextId = 3;

    public IReadOnlyCollection<PoliceOfficer> GetAll()
    {
        lock (_syncRoot)
        {
            return _officers
                .Select(Clone)
                .ToList();
        }
    }

    public PoliceOfficer? Find(int id)
    {
        lock (_syncRoot)
        {
            var officer = _officers.FirstOrDefault(item => item.Id == id);
            return officer is null ? null : Clone(officer);
        }
    }

    public PoliceOfficer Add(PoliceOfficer officer)
    {
        lock (_syncRoot)
        {
            officer.Id = _nextId++;
            _officers.Add(Clone(officer));
            return Clone(officer);
        }
    }

    public void Update(PoliceOfficer officer)
    {
        lock (_syncRoot)
        {
            var index = _officers.FindIndex(item => item.Id == officer.Id);

            if (index >= 0)
            {
                _officers[index] = Clone(officer);
            }
        }
    }

    private static PoliceOfficer Clone(PoliceOfficer officer)
    {
        return new PoliceOfficer
        {
            Id = officer.Id,
            FullName = officer.FullName,
            BadgeNumber = officer.BadgeNumber,
            UnitName = officer.UnitName,
            PhoneNumber = officer.PhoneNumber,
            Username = officer.Username,
            IsActive = officer.IsActive
        };
    }
}
