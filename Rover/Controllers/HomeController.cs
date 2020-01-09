using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rover.Models;

namespace Rover.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult MoveRover(RoverViewModel roverViewModel)
        {
            var _roverMovement = new RoverMovement(roverViewModel);

            try
            {
                _roverMovement.ValidatePlateau();
            }
            catch (Exception ex)
            {
                roverViewModel.Error = ex.Message;
                return View("Index", roverViewModel);
            }

            try
            {
                _roverMovement.Navigate(roverViewModel.Rover1);
            }
            catch (Exception ex)
            {
                roverViewModel.Error = string.Concat("Rover 1: ", ex.Message);
                return View("Index", roverViewModel);
            }

            try
            {
                _roverMovement.Navigate(roverViewModel.Rover2);
            }
            catch (Exception ex)
            {
                roverViewModel.Error = string.Concat("Rover 2: ", ex.Message);
            }

            return View("Index", roverViewModel);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
