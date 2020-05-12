using FlightManager.DataModels;
using FlightManager.Services.ServiceModels;
using System;
using System.Collections.Generic;

namespace FlightManager.Services.Contracts
{
    public interface IFlightService
    {
        void Create(FlightServiceModel flight);

        Flight GetFlightById(string id);

        IEnumerable<Flight> GetAllByPage(int currPage);

        int GetCount();
        bool Exists(string id);   
        
        void Edit(FlightServiceModel flight);

        void DeleteById(string id);

        public int GetGMTDifference(Flight flight);
    }
}
