using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Reservation
{
    public class IndexReservationsViewModel
    {
        public List<ReservationViewModel> Reservations {get; set;}
        public int ReservationsCount { get; set; }
        public int CurrentPage { get; set; }
        public int EndPage { get; set; }
    }
}
