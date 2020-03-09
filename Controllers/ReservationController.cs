using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;
using FlightManager.ViewModels.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManager.Controllers
{
    public class ReservationController : Controller
    {
        private readonly IFlightService flightService;

        private readonly IReservationService reservationService;
        
        public ReservationController(IFlightService flightService, IReservationService reservationService)
        {
            this.flightService = flightService;
            this.reservationService = reservationService;
        }
        public IActionResult Create(string id)
        {
            if(flightService.Exists(id))
            {
                var viewModel = new CreateReservationViewModel
                {
                    FlightId = id
                };
                return View(viewModel);
            }
            return Redirect("/Home/Index");
            
        }

        [HttpPost]
        public IActionResult Create(CreateReservationViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return Redirect("/Reservation/Create");
            }

            var reservation = new ReservationServiceModel
            {
                FirstName = viewModel.FirstName,
                SecondName = viewModel.SecondName,
                LastName = viewModel.LastName,
                EGN = viewModel.EGN,
                PhoneNumber = viewModel.PhoneNumber,
                Nationality = viewModel.Nationality,
                TicketType = viewModel.TicketType,
                Email = viewModel.Email,
                TicketsCount = viewModel.TicketsCount,
                IsConfirmed = false,
                FlightId = viewModel.FlightId
            };

            reservationService.Create(reservation);

            return Redirect("/Home/Index");
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult IndexReservations(int page, string searchString)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");

            int reservationsPerPage = 8;

            if (page < 1)
            {
                return Redirect("Home/Index");
            }

            var reservations = reservationService.GetAllReservationsByPage(page, searchString);
            var reservationsCount = reservationService.GetReservationsByFilterString(searchString).Count();
            var endPage = (reservationsCount / reservationsPerPage) + 1;

            if (endPage > 1 &&  reservationsCount % 8 == 0)
                endPage--;

            if (page > endPage)
                return Redirect("/Home/Index");


            var viewModel = new IndexReservationsViewModel
            {
                SearchString = searchString,
                ReservationsCount = reservationsCount,
                CurrentPage = page,
                EndPage = endPage,
                Reservations = new List<ReservationViewModel>()
            };

            foreach(var res in reservations)
            {
                viewModel.Reservations.Add(
                    new ReservationViewModel
                    {
                        FirstName = res.FirstName,
                        SecondName = res.SecondName,
                        LastName = res.LastName,
                        EGN = res.EGN,
                        PhoneNumber = res.PhoneNumber,
                        Nationality = res.Nationality,
                        TicketType = res.TicketType,
                        Email = res.Email,
                        TicketsCount = res.TicketsCount,
                        IsConfirmed = res.IsConfirmed,
                        Id = res.Id
                    });
            }
            return View(viewModel);
        }

        public IActionResult Confirm(string id)
        {
            if(reservationService.Exists(id))
            {
                reservationService.Confirm(id);
            }
            return Redirect("/Home/Index");
        }

        public IActionResult Delete(string id)
        {
            if(reservationService.Exists(id))
            {
                reservationService.DeleteById(id);
            }
            return Redirect("/Reservation/IndexReservations?page=1");
        }
    }
}