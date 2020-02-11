using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Helper.SessionHelper
{
    public class SessionUser
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Surname { get; set; }

        //[StringLength(11)]
        //public string TC { get; set; }

        //[StringLength(12)]
        //public string PhoneNumber { get; set; }

        public int RoleId { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        //public bool IsActive { get; set; }

        [StringLength(50)]
        public string Username { get; set; }

        public int UserCategoryId { get; set; }

    }
}