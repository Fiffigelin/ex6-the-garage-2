using System;
using Domain.General;
using Domain.Vehicles;
using Domain.Garages;
using static Service.VehicleService;
using System.Collections.Generic;

namespace ConsoleUtilities;

public static class VehicleTableConsole
{
  public static void ViewVehiclesTable(Garage facility)
  {
    List<int> columnWidths = [1, 12, 12, 10, 10];
    List<string> columnHeader = ["", "License", "Type", "Color", "Wheels"];

    List<RowData<Vehicle>> rows = [];
    List<Vehicle> vehicles = [];
    for (int i = 0; i < facility.Vehicles.Count; i++)
    {
      Vehicle? vehicle = facility.Vehicles[i];
      if (vehicle is not null)
      {
        rows.Add(GenericTableConsole.FormatDataRow(
          vehicle,
          [
            "",
            $"{vehicle.LicensePlate}",
            $"{vehicle.Type}",
            $"{vehicle.Color}",
            $"{vehicle.NumberOfWheels}"
          ],
          columnWidths,
          () => ViewVehicleInformation(vehicle, facility)
        ));

        vehicles.Add(vehicle);
      }
    }

    GenericTableConsole.PrintTable(
      () => [
        $"Facility: {facility.Name}",
        $"Amount of Parkinglots: {facility.ParkingLots}",
        $"Free Parkinglots: {facility.ParkingLots - facility.Vehicles.Count}"
      ],
      columnHeader,
      columnWidths,
      rows,
      () => [VehicleStat(facility.Vehicles)]);
  }
  public static void ViewVehicleWithGarageNameTable(List<VehicleSearchResult> results, bool showStat = false)
  {
    List<int> columnWidths = [1, -14, -10, -12, -10, 6];
    List<string> header = ["Search Results"];
    List<string> columnHeader = ["", "Facility", "License", "Type", "Color", "Wheels"];

    List<RowData<VehicleSearchResult>> rows = [];
    List<Vehicle> vehicles = [];

    foreach (VehicleSearchResult result in results)
    {
      rows.Add(GenericTableConsole.FormatDataRow(
            result,
            [
                "",
                result.Garage.Name,
                result.Vehicle.LicensePlate?.Length > 0 ? result.Vehicle.LicensePlate : "",
                result.Vehicle.Type.ToString(),
                result.Vehicle.Color.ToString(),
                result.Vehicle.NumberOfWheels.ToString()
            ],
            columnWidths,
            () => ViewVehicleInformation(
                result.Vehicle,
                result.Garage)
      ));

      vehicles.Add(result.Vehicle);
    }

    GenericTableConsole.PrintTable(() => header, columnHeader, columnWidths, rows, showStat ? () => [VehicleStat(vehicles)] : () => [""]);
  }

  public static void ViewAllVehicles(List<VehicleSearchResult> results)
  {
    List<int> columnWidths = [1, -14, -10, -12, -10, 6];
    List<string> header = ["Show Vehicles"];
    List<string> columnHeader = ["", "Facility", "License", "Type", "Color", "Wheels"];

    List<RowData<VehicleSearchResult>> rows = [];
    List<Vehicle> vehicles = [];

    foreach (VehicleSearchResult result in results)
    {
      rows.Add(GenericTableConsole.FormatDataRow(
          result,
          [
              "",
                result.Garage.Name,
                result.Vehicle.LicensePlate?.Length > 0 ? result.Vehicle.LicensePlate : "",
                result.Vehicle.Type.ToString(),
                result.Vehicle.Color.ToString(),
                result.Vehicle.NumberOfWheels.ToString()
          ],
          columnWidths,
          () => true
      ));

      vehicles.Add(result.Vehicle);
    }

    GenericTableConsole.PrintNoIntegrationTable(() => header, columnHeader, columnWidths, rows);
  }

  private static bool ViewVehicleInformation(Vehicle vehicle, Garage facility)
  {
    List<string> options = ["Exit Parking", "Back"];

    int input = MenuConsole.MenuHandler(["", vehicle.GetInfo(), ""], options);

    switch (input)
    {
      case 0:
        bool removed = facility.RemoveVehicle(vehicle);

        if (removed)
        {
          ParkingMessageConsole.LeaveGarage(vehicle.Type);
        }
        else
        {
          ParkingMessageConsole.WheresMyCar(vehicle.Type);
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);

        return removed;
      case 1:
      case -1:
      default:
        return false;
    }
  }

  private static string VehicleStat(List<Vehicle> vehicles)
  {
    int car = 0;
    int bus = 0;
    int motorcycle = 0;
    int airplane = 0;
    int boat = 0;

    foreach (Vehicle vehicle in vehicles)
    {
      switch (vehicle.Type)
      {
        case VehicleType.Car:
          car++;
          break;
        case VehicleType.Bus:
          bus++;
          break;
        case VehicleType.Motorcycle:
          motorcycle++;
          break;
        case VehicleType.Airplane:
          airplane++;
          break;
        case VehicleType.Boat:
          boat++;
          break;
      }
    }

    //refaktorering: lägger till skriven text också då emojisarna inte verkar fungera överallt
    return $"Car {VehicleConsole.PrintVehicleType(VehicleType.Car)} : {car} | Bus {VehicleConsole.PrintVehicleType(VehicleType.Bus)} : {bus} | Motorcycle {VehicleConsole.PrintVehicleType(VehicleType.Motorcycle)} : {motorcycle} | Airplane {VehicleConsole.PrintVehicleType(VehicleType.Airplane)} : {airplane} | Boat {VehicleConsole.PrintVehicleType(VehicleType.Boat)} : {boat}";
  }
}