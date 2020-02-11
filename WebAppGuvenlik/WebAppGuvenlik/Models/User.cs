using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Web.Mvc;

namespace WebAppGuvenlik.Models
{
    [Table("User")]
    public partial class User
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter surname")]
        [DisplayName("Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Enter email address")]
        [DisplayName("E-mail")]
        [EmailAddress(ErrorMessage = "Please make sure you enter your email address correctly.")] //ekledim
        public string Mail { get; set; }
        
        [DisplayName("Phone")]
        [Phone(ErrorMessage = "Please make sure you enter your phone correctly.")]
        [Required(ErrorMessage = "Enter phone number")]
        public string Phone { get; set; }
        
        [DisplayName("Username")]
        [Required(ErrorMessage = "Enter Username")]
        public string UserName { get; set; }
        
        [DisplayName("Password")]
        [Required(ErrorMessage = "Enter Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Select User Category")]
        [DisplayName("User Role")]
        public int UserCategoryID { get; set; }
        
        public string ResetPasswordCode { get; set; }
    }
}
