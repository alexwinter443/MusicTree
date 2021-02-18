using Milestone.Models;
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
 * Security Service which acts as a business service for the functions which require security. 
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Views.Services
{
    public class SecurityService
    {

        SecurityDAO SecurityDAO = new SecurityDAO();

        public bool IsValid(UserModel user)
        {
            return SecurityDAO.RegisterUser(user);
        }

        public bool loginUser(UserModel user)
        {
            return SecurityDAO.findByUsernamePassword(user);
        }
    }
}
