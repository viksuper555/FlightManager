using FlightManager.DataModels;
using FlightManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManager.Services.Contracts
{
    public interface IReservationService
    {
        public void Create(ReservationServiceModel reservation);

        public Reservation GetById(string id); 
        int GetCount();

        public IEnumerable<Reservation> GetAllReservations();

        public IEnumerable<Reservation> GetAllReservationsByFlightId(string id);

        public IEnumerable<Reservation> GetReservationsByFilterString(string searchString);

        public List<Reservation> GetAllReservationsByPage(int currPage, string searchString);

        public bool Exists(string id);

        public void Edit(ReservationServiceModel reservation);

        public void DeleteById(string id);
        void Confirm(string id);
    }
}
