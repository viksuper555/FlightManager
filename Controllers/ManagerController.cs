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

        //[Authorize(Roles = "Admin")]
        public IActionResult GetAll(int page, int showPerPage, string filterBy, string searchString)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");

            if (page < 1 || showPerPage < 1)
            {
                return Redirect("/Home/Index");
            }

            int managersCount = managerService.GetCount() - 1;

            var endPage = (managersCount / showPerPage) + 1;

            if (endPage > 1)
                endPage--;

            if (page > endPage)
                return Redirect("/Home/Index");

            var managers = managerService.GetAll(page, showPerPage, filterBy, searchString);

            var viewModel = new IndexManagersViewModel
            {
                Managers = new List<ManagerViewModel>(),
                ManagersCount = managersCount,
                ManagersPerPage = showPerPage,
                FilterBy = filterBy,
                CurrentPage = page,
                EndPage = endPage
            };

            foreach(var manager in managers)
            {
                viewModel.Managers.Add(new ManagerViewModel
                {
                    Id = manager.Id,
                    FirstName = manager.FirstName,
                    LastName = manager.LastName,
                    Username = manager.UserName,
                    Address = manager.Address,
                    EGN = manager.EGN,
                    Email = manager.Email,
                    PhoneNumber = manager.PhoneNumber
                });
            }

            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");

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
            return Redirect("GetAll?page=1&showPerPage=10&filterBy=Username");
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EditManagerViewModel model)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");
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
            return Redirect("GetAll?page=1&showPerPage=10&filterBy=Username");
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");
            if (managerService.Exists(id))
            {
                managerService.DeleteById(id);
            }
            return Redirect("/GetAll?page=1&showPerPage=10&filterBy=Username");
        }
    }
}