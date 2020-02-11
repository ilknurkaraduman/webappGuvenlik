using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Models.Employee
{
    public class StatusModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EmployeeID { get; set; }
        public int VisitorID { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Click { get; set; }

        public string VisitorName { get; set; }
        public string EmployeeName { get; set; }
        public string SecurityName { get; set; }
    }
}