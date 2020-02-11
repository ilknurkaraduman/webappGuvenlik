using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace WebAppGuvenlik.Models
{
    public class Status
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("Visit Confirmation Status")]
        public string Name { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Employee ID")]
        public int EmployeeID { get; set; }

        [DisplayName("Visitor ID")]
        public int VisitorID { get; set; }

        [DisplayName("End Date")]
        public DateTime? EndDate { get; set; }

        public bool? Click { get; set; }

    }
}