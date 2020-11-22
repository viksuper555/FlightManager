using FlightManager.DataModels;
using FlightManager.Services.ServiceModels;
using System;
using System.Collections.Generic;

namespace FlightManager.Services.Contracts
{
    public interface IFlightService
    {
        public void Create(FlightServiceModel flight);

        public Flight GetFlightById(string id);

        public IEnumerable<Flight> GetAllByPage(int currPage);

        public int GetCount();
        public bool FlightExists(string id);
        public void Edit(FlightServiceModel flight);

        public void DeleteById(string id);

    }
}
