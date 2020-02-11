using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Models.Admin
{
    public class AdminViewModel
    {
        public int? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int VisitorCount { get; set; }
    }
}