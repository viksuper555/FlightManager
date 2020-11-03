using FlightManager.Data;
using FlightManager.DataModels;
using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FlightManager.Services
{
    public class ReservationService : IReservationService
    {
        private readonly FlightManagerDbContext context;

        private readonly IEmailSender emailSender;

        public ReservationService(FlightManagerDbContext context, IEmailSender emailSender)
        {
            this.emailSender = emailSender;
            this.context = context;
        }
        public void Create(ReservationServiceModel reservation)
        {

            if (reservation.TicketsCount <= 0)
                return;

            var flight = context.Flights.SingleOrDefault(f => f.Id == reservation.FlightId);

            if (reservation.TicketType == "Economy" && reservation.TicketsCount > flight.PassengerSeatsLeft)
                return;
            
            if (reservation.TicketType == "Business" && reservation.TicketsCount > flight.BusinessClassSeatsLeft)
                return;
            

            var newReservation = new Reservation()
            {
                FirstName = reservation.FirstName,
                SecondName = reservation.SecondName,
                LastName = reservation.LastName,
                EGN = reservation.EGN,
                PhoneNumber = reservation.PhoneNumber,
                Nationality = reservation.Nationality,
                TicketsCount = reservation.TicketsCount,
                TicketType = reservation.TicketType,
                Email = reservation.Email,
                IsConfirmed = false,
                FlightId = reservation.FlightId

            };

            context.Reservations.Add(newReservation);
            context.SaveChanges();

            //var appAddress = "https://localhost:5001/"; // TODO: Load proper appAddress for reservation service 
            var urls = Environment.GetEnvironmentVariable("applicationUrl").Split(";").First();

            var message = $@"Здравейте, {newReservation.FirstName} {newReservation.LastName}, желаете ли да потвърдите вашата резервация от 
                            {newReservation.TicketsCount} {newReservation.TicketType} билети от {flight.Origin} към {flight.Destination}?
                            <br/> Дата на излитане: {flight.Departure.ToString(" dd.MM.yyyy г. в hh:mm ч.")}     
                            <br/> Дата на кацане: {flight.Arrival.ToString(" dd.MM.yyyy г. в hh:mm ч.")}
                            <p>Натиснете <a href='{urls}/Reservation/Confirm?id={newReservation.Id}'>тук</a> за да я потвърдите.
                            <br/> Натиснете <a href='{urls}/Reservation/Delete?id={newReservation.Id}'>тук</a> за да я откажете.
                            </p>";


            emailSender.SendEmailAsync(reservation.Email, "Confirm Your Reservation", message).GetAwaiter().GetResult();
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return context.Reservations.OrderByDescending(r => r.Email).ToList();
        }

        public IEnumerable<Reservation> GetAllReservationsByFlightId(string id)
        {
            return context.Reservations.Where(x => x.FlightId == id).ToList();
        }

        public IEnumerable<Reservation> GetReservationsByFilterString(string searchString)
        {
            var reservations = new List<Reservation>();
            if (!string.IsNullOrEmpty(searchString))
            {
                reservations = context.Reservations.Where(r => r.Email.ToLower().Contains(searchString.ToLower())).ToList();
            }
            else
            {
                reservations = context.Reservations.ToList();
            }
            return reservations;
        }
        public Reservation GetById(string id)
        {
            if(Exists(id))
            {
                return context.Reservations.SingleOrDefault(x => x.Id == id);
            }
            throw new ArgumentException("Invalid id!");
        }

        public int GetCount()
        {
            return context.Reservations.Count();
        }

        public void Edit(ReservationServiceModel reservation)
        {
            if (Exists(reservation.Id))
            {
                var dbReservation = GetById(reservation.Id);

                dbReservation.FirstName = reservation.FirstName;
                dbReservation.SecondName = reservation.SecondName;
                dbReservation.LastName = reservation.LastName;
                dbReservation.EGN = reservation.EGN;
                dbReservation.Nationality = reservation.Nationality;
                dbReservation.PhoneNumber = reservation.PhoneNumber;
                dbReservation.TicketsCount = reservation.TicketsCount;
                dbReservation.Email = reservation.Email;
                dbReservation.IsConfirmed = reservation.IsConfirmed;
                dbReservation.FlightId = reservation.FlightId;

                context.Reservations.Update(dbReservation);
                context.SaveChanges();

            }
            else
            {
                throw new ArgumentException("Invalid id!");

            }
        }

        public void DeleteById(string id)
        {
            if(Exists(id))
            {
                var reservation = GetById(id);
                if (reservation.IsConfirmed)
                    return;
                context.Reservations.Remove(reservation);
                context.SaveChanges();
            }
            else
            {
                throw new ArgumentException("Invalid id!");
            }
        }
        
        public bool Exists(string id)
        {
            return context.Reservations.Any(x => x.Id == id);
        }

        public List<Reservation> GetAllReservationsByPage(int currPage, string searchString)
        {
            int firstHalf = currPage * 8;               //Taking first n pages of elements and
            int toBeSkipped = (currPage - 1) * 8;       //substracting first n-1 pages to get n page's elements

            return GetReservationsByFilterString(searchString)
                .OrderByDescending(r => r.Email)
                .Take(firstHalf).Skip(toBeSkipped).ToList();
        }

        public void Confirm(string id)
        {
            if (!Exists(id))
                throw new ArgumentException("Invalid id!");

            var reservation = GetById(id);

            if (reservation.IsConfirmed)
                return;

            reservation.IsConfirmed = true;

            var flight = context.Flights.SingleOrDefault(f => f.Id == reservation.FlightId);
            if(reservation.TicketType == "Economy")
                flight.PassengerSeatsLeft -= reservation.TicketsCount;

            else if (reservation.TicketType == "Business")
                flight.BusinessClassSeatsLeft -= reservation.TicketsCount;

            context.Flights.Update(flight);
            context.Reservations.Update(reservation);

            context.SaveChanges();
        }
    }
}
