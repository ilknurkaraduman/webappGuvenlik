using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Models.Admin
{
    public class ReportVisitoryModel 
        //status tablosundaki visitorID ve employeeID lerin yerine nameler goruntulenmesi icin
    {
        public int ID { get; set; }
       
        public string Name { get; set; }

        public string Description { get; set; }

        public string EmployeeName { get; set; }

        public string VisitorName { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? Click { get; set; }
    }
}