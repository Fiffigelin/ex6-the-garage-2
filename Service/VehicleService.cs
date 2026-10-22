using System;
using System.Collections.Generic;
using ConsoleUtilities;
using Domain.Garages;
using Domain.General;
using Domain.Vehicles;

namespace Service;


public static partial class VehicleService
{

  // En träff i sökningen: fordonet och parkeringshuset det står i.
  public sealed record VehicleSearchResult(Garage Garage, Vehicle Vehicle);
  public static void ParkVehicle(GarageData data)
  {
    while (true)
    {
      Console.Clear();
      TitleConsole.DisplayMainTitle();
      Console.WriteLine();

      List<Garage> garages = data.AvailableParking();
      List<string> options = [];

      foreach (Garage garage in garages)
      {
        options.Add(garage.Name);
      }
      options.Add("Back");

      int input = MenuConsole.MenuHandler(["\nChoose a Parking Facility"], options);

      if (input == -1 || input == options.Count - 1)
      {
        return;
      }

      VehicleType? type = VehicleConsole.ChooseVehicleType();
      if (type is null)
      {
        continue;
      }

      Garage parking = garages[input];
      Vehicle? vehicle;

      try
      {
        vehicle = AddVehicle((VehicleType)type, data);
      }
      catch (ArgumentException ex)
      {
        Console.WriteLine($"Could not create the vehicle: {ex.Message}");
        Console.WriteLine("Press any key to try again...");
        Console.ReadKey(true);
        continue;
      }

      bool parked = parking.AddVehicle(vehicle);

      if (parked)
      {
        ParkingMessageConsole.ParkingCar(vehicle.Type);
      }
      else
      {
        ParkingMessageConsole.NoMoreLots(vehicle.Type);
      }

      Console.WriteLine("Press any key to continue...");
      Console.ReadKey(true);
      return;
    }
  }

  public static Vehicle AddVehicle(VehicleType type, GarageData data)
  {
    string color = InputConsole.StringInput("Color: ");
    string licenseplate = InputConsole.LicenceInput("Licenseplate: ", data);

    switch (type)
    {
      case VehicleType.Car:
        return CreateCar(color, licenseplate);
      case VehicleType.Motorcycle:
        return CreateMotorcycle(color, licenseplate);
      case VehicleType.Bus:
        return CreateBus(color, licenseplate);
      case VehicleType.Airplane:
        return CreateAirplane(color, licenseplate);
      case VehicleType.Boat:
        return CreateBoat(color, licenseplate);
      default:
        throw new ArgumentException("Could not add vehicle. Please try again!");
    }
  }

  public static void SearchVehicles(GarageData data)
  {
    Console.Clear();
    TitleConsole.DisplayMainTitle();
    Console.WriteLine("\nSearch vehicles - leave a field empty to skip it.\n");

    string? licensePlate = InputConsole.OptionalStringInput("License plate: ");
    string? color = InputConsole.OptionalStringInput("Color: ");
    int? numberOfWheels = InputConsole.OptionalIntInput("Number of wheels: ");
    VehicleType? type = VehicleConsole.ChooseVehicleTypeFilter("Any", ["\nSearch vehicles", "Type of vehicle\n"]);

    SearchVehicle filter = new(licensePlate, color, numberOfWheels, type);
    List<VehicleSearchResult> results = SearchVehicle(filter, data);

    VehicleTableConsole.ViewSearchTable(results);
  }

  public static void ShowAllParkedVehicles(GarageData data)
  {
    while (true)
    {
      Console.Clear();
      TitleConsole.DisplayMainTitle();
      VehicleType? type = VehicleConsole.ChooseVehicleTypeFilter(" All", ["\nShow Parked Cars By Type"]);

      SearchVehicle filter = new(null, null, null, type);
      List<VehicleSearchResult> results = SearchVehicle(filter, data);

      VehicleTableConsole.ViewAllVehicles(results);
      ConsoleKey key = Console.ReadKey(true).Key;

      switch (key)
      {
        case ConsoleKey.Escape:
          return;
        default:
          break;
      }

    }
  }

  // söker i enskilt parkeringshus
  private static List<Vehicle> SearchVehicle(SearchVehicle search, Garage garage)
  {
    ArgumentNullException.ThrowIfNull(search);
    ArgumentNullException.ThrowIfNull(garage);

    List<Vehicle>? vehicles = garage.Vehicles;
    List<Vehicle> results = [];

    foreach (Vehicle vehicle in vehicles)
    {
      if (vehicle is not null && search.Matches(vehicle))
      {
        results.Add(vehicle);
      }
    }

    return results;
  }

  // Söker i ALLA parkeringshus och berättar även vilket hus fordonet står i.
  private static List<VehicleSearchResult> SearchVehicle(
      SearchVehicle filter,
      GarageData data)
  {
    ArgumentNullException.ThrowIfNull(filter);
    ArgumentNullException.ThrowIfNull(data);

    List<VehicleSearchResult> hits = [];

    foreach (Garage garage in data.Garages)
    {
      List<Vehicle> vehicles = SearchVehicle(filter, garage);

      foreach (Vehicle vehicle in vehicles)
      {
        hits.Add(new VehicleSearchResult(garage, vehicle));
      }
    }

    return hits;
  }
}