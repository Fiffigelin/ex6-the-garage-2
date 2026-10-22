using System;
using System.Collections.Generic;
using Domain.Vehicles;

namespace Domain.Garages;

public class GarageData
{
  private List<Garage> _garages = [];

  public List<Garage> Garages
  {
    get
    {
      return _garages;
    }
  }

  public void AddFacility(Garage facility)
  {
    ArgumentNullException.ThrowIfNull(facility);

    _garages.Add(facility);
  }

  public bool IsNameUnique(string name)
  {
    foreach (Garage facility in _garages)
    {
      if (string.Equals(
        facility.Name,
        name,
        StringComparison.OrdinalIgnoreCase
      ))
      {
        return false;
      }
    }

    return true;
  }

  public List<Garage> AvailableParking()
  {
    List<Garage> available = [];

    foreach (Garage garage in _garages)
    {
      if (garage.Vehicles.Count < garage.ParkingLots)
      {
        available.Add(garage);
      }
    }

    return available;
  }

  public bool IsLicenseplateUnique(string license)
  {
    ArgumentException.ThrowIfNullOrWhiteSpace(license);

    foreach (Garage pf in _garages)
    {
      List<Vehicle> vehicles = pf.Vehicles;

      foreach (Vehicle? vh in vehicles)
      {
        if (string.Equals(
            vh.LicensePlate,
            license.Trim(),
            StringComparison.OrdinalIgnoreCase))
        {
          return false;
        }
      }
    }

    return true;
  }
}

