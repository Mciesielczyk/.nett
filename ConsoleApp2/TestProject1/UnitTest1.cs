using NUnit.Framework;
using ConsoleApp2;
using System;
using System.Linq;

namespace TestProject1
{
    public class RentalCompanyTests
    {
        private RentalCompany company;
        private Vehicle vehicle1;
        private Vehicle vehicle2;

        [SetUp]
        public void Setup()
        {
            company = RentalCompany.Instance; 

            vehicle1 = new Motorcycle(1, "Yamaha", "R1", 2021, true, 1000);
            vehicle2 = new Motorcycle(2, "Kawasaki", "Ninja", 2020, true, 750);

            company.AddVehicle(vehicle1);
            company.AddVehicle(vehicle2);
        }

        [Test]
        public void Test_AddVehicle_Should_Add_Vehicle_To_Company()
        {
            Assert.Contains(vehicle1, company.vehicles);
            Assert.Contains(vehicle2, company.vehicles);
        }

        [Test]
        public void Test_ReserveVehicle_Should_Reserve_Vehicle()
        {
            company.ReserveVehicle(1, "John Doe");

      
            Assert.IsFalse(vehicle1.IsAvailable);
            Assert.AreEqual(3, company.reservations.Count);

            var reservation = company.reservations.First();
            Assert.AreEqual("John Doe", reservation.GetVehicle().getId() == 1 ? "John Doe" : "");
        }

        [Test]
        public void Test_CancelReservation_Should_Cancel_Reservation()
        {
            company.ReserveVehicle(1, "John Doe");

            vehicle1.CancelReserve();
            Assert.AreEqual(0, company.reservations.Count);

            Assert.IsFalse(vehicle1.IsAvailable);
        }

        [Test]
        public void Test_OnNewReservation_Should_Trigger_Event()
        {
            string eventMessage = null;

            company.OnNewReservation += (message) => eventMessage = message;

            company.ReserveVehicle(1, "John Doe");

            Assert.IsNotNull(eventMessage);
            Assert.AreEqual("Rezerwacja dokonana przez John Doe.", eventMessage);
        }

        [Test]
        public void Test_ListAvailableVehicles_Should_List_Available_Vehicles()
        {
            company.ReserveVehicle(1, "John Doe");

            var availableVehicles = company.vehicles.Where(v => v.IsAvailable).ToList();

            Assert.AreEqual(3, availableVehicles.Count);
            Assert.Contains(vehicle2, availableVehicles);
        }
    }
}
