using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Manager
{
    public class IndexManagersViewModel
    {
        public List<ManagerViewModel> Managers { get; set; }

        public int ManagersCount { get; set; }

        public int CurrentPage { get; set; }

        public int ManagersPerPage { get; set; }

        public string OrderBy { get; set; }

        public int EndPage { get; set; }
    }
}
