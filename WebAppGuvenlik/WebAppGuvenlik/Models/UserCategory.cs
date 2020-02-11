using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.Web.Mvc;

namespace WebAppGuvenlik.Models
{
    [Table("UserCategory")]
    public class UserCategory
    {
       [Key]
        public int ID { get; set; }

        [DisplayName("User Role")]
        [StringLength(50)]
        public string Name { get; set; }

    }
}