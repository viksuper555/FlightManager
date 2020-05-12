using FlightManager.Data;
using FlightManager.DataModels;
using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlightManager.Services
{
    public class FlightService : IFlightService
    {
        private readonly FlightManagerDbContext context;

        public FlightService(FlightManagerDbContext context)
        {
            this.context = context;
        }
        public void Create(FlightServiceModel flight)
        {
            var newFlight = new Flight
            {
                Origin = flight.Origin,
                Destination = flight.Destination,
                Departure = flight.Departure,
                Arrival = flight.Arrival,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                PassengerSeatsLeft = flight.PassengerSeatsLeft,
                BusinessClassSeatsLeft = flight.BusinessClassSeatsLeft
            };

            context.Flights.Add(newFlight);
            context.SaveChanges();
        }

        public void DeleteById(string id)
        {
            if (Exists(id))
            {
                var flight = GetFlightById(id);

                context.Flights.Remove(flight);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Invalid id!");
            }
        }

        public void Edit(FlightServiceModel flight)
        {
            if (Exists(flight.Id))
            {
                var dbFlight = GetFlightById(flight.Id);

                dbFlight.Origin = flight.Origin;
                dbFlight.Destination = flight.Destination;
                dbFlight.Departure = flight.Departure;
                dbFlight.Arrival = flight.Arrival;
                dbFlight.PlaneType = flight.PlaneType;
                dbFlight.PlaneNumber = flight.PlaneNumber;
                dbFlight.PilotName = flight.PilotName;
                dbFlight.PassengerSeatsLeft = flight.PassengerSeatsLeft;
                dbFlight.BusinessClassSeatsLeft = flight.BusinessClassSeatsLeft;

                context.Flights.Update(dbFlight);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Invalid id!");
            }

        }

        public bool Exists(string id)
        {
            return context.Flights.Any(x => x.Id == id);
        }

        public IEnumerable<Flight> GetAllByPage(int currPage)
        {

            int firstHalf = currPage * 8;               //Taking first n pages of elements and
            int toBeSkipped = (currPage - 1) * 8;       //substracting first n-1 pages to get n page's elements

            return context.Flights.OrderByDescending(f => f.Departure)
                .Take(firstHalf)
                .Skip(toBeSkipped)
                .ToList();
        }

        public int GetCount()
        {
            return context.Flights.Count();
        }

        public Flight GetFlightById(string id)
        {
            if (Exists(id))
            {
                return context.Flights.SingleOrDefault(x => x.Id == id);
            }
            throw new ArgumentException("Invalid id!");

        }

        public int GetGMTDifference(Flight flight)
        {
            var GMTOrig = flight.Origin switch
            {
                "Bulgaria" => 2,
                "Mexico" => -6,
                "Singapore" => 8,
                "Estonia" => 2,
                "Russia" => 3,
                "Inida" => 5,
                _ => 0,
            };
            var GMTDest = flight.Destination switch
            {
                "Bulgaria" => 2,
                "Mexico" => -6,
                "Singapore" => 8,
                "Estonia" => 2,
                "Russia" => 3,
                "India" => 5,
                _ => 0,
            };
            return Math.Abs(GMTDest - GMTOrig);
        
        }
    }
}
