using Microsoft.AspNetCore.Mvc;
using Milestone.Models;
using Milestone.Views.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Alex Vergara and Kacey Morris
 * January 31, 2021
 * CST 247
 * Minesweeper Web Application
 * 
 * Login Controller which directs the actions of the login module. 
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ProcessLogin(UserModel user)
        {
            SecurityService security = new SecurityService();
            if (security.loginUser(user))
            {
                return View("LoginSuccessful", user);
            }
            else
            {
                return View("LoginFailure", user);
            }
        }
    }
}
