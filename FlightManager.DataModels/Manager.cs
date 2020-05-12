using Microsoft.AspNetCore.Identity;
using System;

namespace FlightManager.DataModels
{
    public class Manager : IdentityUser
    {
        public Manager()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EGN { get; set; }
        public string Address { get; set; }
        public override string PhoneNumber { get; set; }

    }
}
