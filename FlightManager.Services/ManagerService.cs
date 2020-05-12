using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FlightManager.Data;
using FlightManager.DataModels;
using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;

namespace FlightManager.Services
{
    public class ManagerService : IManagerService
    {
        private readonly FlightManagerDbContext context;

        public ManagerService(FlightManagerDbContext context)
        {
            this.context = context;
        }

        public Manager GetById(string id)
        {
            if (Exists(id))
                return context.Users.SingleOrDefault(x => x.Id == id);
            throw new ArgumentException("Invalid id!");
        }

        public int GetCount()
        {
            return context.Users.Count();
        }
        
        public IEnumerable<Manager> GetAll(int currPage, int showPerPage, string filterBy, string searchString)
        {
            var roleId = context.Roles.SingleOrDefault(n => n.Name != "Admin").Id;
            var ManagersIdList = context.UserRoles.Where(ru => ru.RoleId == roleId).Select(ru => ru.UserId).ToList();

            var managers = new List<Manager>();
            foreach(var managerId in ManagersIdList)
            {
                var manager = context.Users.SingleOrDefault(m => m.Id == managerId);
                managers.Add(manager);
            }
            if(!string.IsNullOrEmpty(searchString))
            { 
                managers = filterBy switch
                {
                    "Email" => managers.Where(m => m.Email.ToLower().Contains(searchString.ToLower())).ToList(),
                    "FirstName" => managers.Where(m => m.FirstName.ToLower().Contains(searchString.ToLower())).ToList(),
                    "LastName" => managers.Where(m => m.LastName.ToLower().Contains(searchString.ToLower())).ToList(),
                    "Username" => managers.Where(m => m.UserName.ToLower().Contains(searchString.ToLower())).ToList(),
                    _ => managers.OrderBy(m => m.UserName).ToList()
                };
            }

            int firstHalf = currPage * 8;               //Taking first n pages of elements and
            int toBeSkipped = (currPage - 1) * 8;       //substracting first n-1 pages to get n page's elements

            var pageContent = managers.Take(firstHalf).Skip(toBeSkipped).ToList();

            return pageContent;
        }

        public void Edit(ManagerServiceModel manager)
        {
            if (Exists(manager.Id))
            {
                var newManager = context.Users.SingleOrDefault(u => u.Id == manager.Id);

                newManager.FirstName = manager.FirstName;
                newManager.LastName = manager.LastName;
                newManager.EGN = manager.EGN;
                newManager.Address = manager.Address;
                newManager.PhoneNumber = manager.PhoneNumber;

                context.Users.Update(newManager);
                context.SaveChanges();
            }
            else
                throw new ArgumentException("Invalid id!");
        }
        
        public void DeleteById(string id)
        {
            var manager = GetById(id);

            context.Users.Remove(manager);
            context.SaveChanges();

        }
        
        public bool Exists(string id)
        {
            return context.Users.Any(x => x.Id == id);
        }

    }
}
