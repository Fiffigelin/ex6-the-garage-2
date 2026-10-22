using System;
using System.Collections.Generic;
using ConsoleUtilities;
using Domain.Garages;

namespace Service;


public static class ParkingFacilityService
{
  public static void HandleParkingFacilities(GarageData garages)
  {
    TitleConsole.DisplayMainTitle();
    Console.WriteLine();

    ViewFacilities(garages);
  }

  public static void ViewFacilities(GarageData data)
  {
    while (true)
    {
      List<Garage> facilities = data.Garages;
      List<string> options = ["Add New Parking Facility"];

      foreach (Garage facility in facilities)
      {
        options.Add(facility.Name);
      }
      options.Add("Go Back To Main Page");

      int input = MenuConsole.MenuHandler(["\nHandle parking facilities\n"], options);

      if (input == 0)
      {
        AddNewParkingFacility(data);
      }
      else if (input == options.Count - 1 || input == -1)
      {
        return;
      }
      else
      {
        VehicleTableConsole.ViewVehiclesTable(facilities[input]);
      }
    }
  }

  private static void AddNewParkingFacility(GarageData data)
  {
    TitleConsole.DisplayMainTitle();
    Console.WriteLine("Add New Parking Facility\n");

    string facilityName = InputConsole.FacilityNameInput("Name: ", data);
    int parkingLots = InputConsole.IntInput("Amount of parkinglots: ");

    try
    {
      data.AddFacility(new Domain.Garages.Garage(facilityName, parkingLots));
      Console.WriteLine($"\nParking facility '{facilityName}' has been added with {parkingLots} lots");
    }
    catch (ArgumentException ex)
    {
      Console.WriteLine($"\nCould not create the parking facility: {ex.Message}");
    }

    Console.WriteLine("Press any key to continue...");
    Console.ReadKey(true);
  }
}