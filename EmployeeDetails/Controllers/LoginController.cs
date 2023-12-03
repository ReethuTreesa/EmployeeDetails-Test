using BusinessEntities;
using BusinessLayer;
using EmployeeDetails.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

namespace EmployeeDetails.Controllers
{
    public class LoginController : Controller
    {
        public DateTime OtpCrtDate;
        private IConfiguration Configuration;

        public LoginController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            ViewBag.IsLoginVisible = false;

            return View();
        }

       

    }
}
