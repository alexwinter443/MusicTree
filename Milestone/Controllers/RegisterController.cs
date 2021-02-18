using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Milestone.Models;
using Milestone.Views.Services;

/*
 * Alex Vergara and Kacey Morris
 * January 31, 2021
 * CST 247
 * Minesweeper Web Application
 * 
 * Register Controller which directs the functions of the registration module. 
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Controllers
{
    public class RegisterController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessRegistration(UserModel user)
        {
            SecurityService security = new SecurityService();
            if (security.IsValid(user))
            {
                return View("RegistrationSuccessful", user);
            }
            else
            {
                return View("RegistrationFailure", user);
            }
        }

        
    }
}
