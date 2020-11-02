using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManager.Services.ServiceModels
{
    public class ReservationServiceModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string EGN { get; set; }
        public string PhoneNumber { get; set; }
        public string Nationality { get; set; }
        public string TicketType { get; set; }
        public string Email { get; set; }
        public int TicketsCount { get; set; }
        public bool IsConfirmed { get; set; }
        public string FlightId { get; set; }

    }
}
