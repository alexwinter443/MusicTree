using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

/*
 * Alex Vergara
 * 4/24/2022
 * cst 451
 * Capstone
 * 
 * User Model which defines the properties and methods of a user.
 * 
 * This is our own work as influenced by class time and examples. 
 */

namespace Milestone.Models
{
    public class UserModel
    {
        // for database storage
        private int ID { get; set; }
        [Required]
        [DisplayName(@"First Name")]
        public string firstName { get; set; }

        [Required]
        [DisplayName(@"Last Name")]
        public string lastName { get; set; }

        [DisplayName(@"Gender")]
        public string gender { get; set; }

        [DisplayName(@"Age")]
        public int age { get; set; }

        [EmailAddress]
        [Required]
        [DisplayName(@"Email")]
        public string email { get; set; }

        [Required]
        [DisplayName(@"Username")]
        public string username { get; set; }

        [Required]
        [DisplayName(@"Password")]
        public string password { get; set; }
    }
}
