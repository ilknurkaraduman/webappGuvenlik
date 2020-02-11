using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Models.Security
{
    public class VisitorsModel
    {
        public int ID { get; set; }
        [DisplayName("Name")]
        public string Name { get; set; }
        [DisplayName("Surname")]
        public string Surname { get; set; }
        [DisplayName("Phone")]
        public string Phone { get; set; }
        [DisplayName("Date")]
        public DateTime? Date { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        [DisplayName("Employee to Visit")]
        public int EmployeeId { get; set; }
        [DisplayName("who adds to the system")]
        public int SecurityId { get; set; }

        [DisplayName("Employee to Visit")]
        public string EmployeeName { get; set; }
        [DisplayName("who adds to the system")]
        public string SecurityName { get; set; }


        [DisplayName("Status")]
        public string VisitorStatus { get; set; }
    }
}