using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightManager.Services;
using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;
using FlightManager.ViewModels.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManager.Controllers
{
    public class ManagerController : Controller
    {
        private readonly IManagerService managerService;

        public ManagerController(IManagerService managerService)
        {
            this.managerService = managerService;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            if (managerService.Exists(id))
            {
                var manager = managerService.GetById(id);

                var viewModel = new EditManagerViewModel
                {
                    Id = manager.Id,
                    FirstName = manager.FirstName,
                    LastName = manager.LastName,
                    Address = manager.Address,
                    EGN = manager.EGN,
                };

                return View(viewModel);
            }
            return Redirect("Manager/All?page=1&showPerPage=10&orderBy=FirstNameAsc");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EditManagerViewModel model)
        {
            if (managerService.Exists(model.Id) && ModelState.IsValid)
            {
                var serviceModel = new ManagerServiceModel
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address = model.Address,
                    EGN = model.EGN
                };
                managerService.Edit(serviceModel);
            }
            return Redirect("Manager/All?page=1&showPerPage=10&orderBy=FirstNameAsc");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (managerService.Exists(id))
            {
                managerService.DeleteById(id);
            }
            return Redirect("Manager/All?page=1&showPerPage=10&orderBy=FirstNameAsc");
        }
    }
}