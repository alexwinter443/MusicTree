
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Milestone.Models;
using Milestone.Views.Services;
using Nancy.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Alex Vergara
 * 4/24/2022
 * cst-451
 * Capstone
 * 
 * Login Controller which directs the actions of the login module. 
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Controllers
{
    public class LoginController : Controller
    {

        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();   
           
            return RedirectToAction("Index", "Login");
        }


        public IActionResult Index()
        {
            return View();
        }

        // Processes
        // the login for the user
        public IActionResult ProcessLogin(UserModel user)
        {
           
            // implements security service
            SecurityService security = new SecurityService();
            // if -1 was returned, that means the user was not found
            // otherwise, the value should be the userID
            int userID = security.loginUser(user);




            HttpContext.Session.SetString("verified", "none");

            if (userID != -1)
            {
                // set the session variables
                HttpContext.Session.SetInt32("userID", userID);
                HttpContext.Session.SetString("verified", "verified");


                return RedirectToAction("Index", "AudioFile");
                //return View("Index", user);

            }
            else
            {
                return View("LoginFailure", user);
            }
        }

        public IActionResult SelectDifficulty()
        {
            return View("LoginSuccessful");
        }
    }
}
