
using System.IO;
using System.Text.Json;
using Domain.Garages;
using Domain.Vehicles;

namespace Data;

public static class GarageSeeder
{
  // refaktor: ändra från svenska till engelska
  public static void Seed(GarageData garageData, string filePath)
  {
    string json = File.ReadAllText(filePath);

    JsonSerializerOptions options = new()
    {
      PropertyNameCaseInsensitive = true
    };

    GarageSeedData seedData =
        JsonSerializer.Deserialize<GarageSeedData>(json, options)
        ?? throw new InvalidDataException(
            "Could not read the file.");

    foreach (GarageSeedItem item in seedData.Garages)
    {
      Garage garage = new(item.Name, item.ParkingLots);

      foreach (VehicleSeedItem vehicleItem in item.Vehicles)
      {
        Vehicle vehicle = VehicleFactory.Create(vehicleItem);

        if (!garage.AddVehicle(vehicle))
        {
          throw new InvalidDataException(
              $"Could not park vehicle in Garage {item.Name}.");
        }
      }

      garageData.AddFacility(garage);
    }
  }
}
