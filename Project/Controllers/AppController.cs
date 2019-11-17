using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Services;
using Project.ViewModels;

namespace Project.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDatabaseRepository _repository;
        public AppController(IMailService mailservice,DatabaseContext context,IDatabaseRepository repository)
        {
            _mailService = mailservice;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("contact")]
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                //Send the mail
                _mailService.SendMessage("Masum@gmail.com", model.Subject, $"Form:{model.Name}-{model.Email},Message: {model.Message}");
                ViewBag.UserMessage = "Mail Sent";
                ModelState.Clear();
            }

            return View();
        }
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        [Authorize]
        public IActionResult shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }
    }
}
