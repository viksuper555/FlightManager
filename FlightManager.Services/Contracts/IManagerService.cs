using FlightManager.DataModels;
using FlightManager.Services.ServiceModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManager.Services.Contracts
{
    public interface IManagerService
    {
        public int GetCount();
        public IEnumerable<Manager> GetAll(int page, int showPerPage, string filterBy, string searchString);
        public bool Exists(string id);
        public Manager GetById(string id);
        public void Edit(ManagerServiceModel manager);
        public void DeleteById(string id);
    }
}
