using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppGuvenlik.Models.Admin
{
    public class AdminTopModel
    {
        public int SelectedId { get; set; } //secili degeri tutacak olan
        public List<SelectListItem> UserList { get; set; } //dropdownlist'te gonderilen employee list icin
        public List<ReportVisitoryModel> StatusList { get; set; } //dropdownlist'ten secilen degere gore status listesi
        //ReportVisitoryModel tipinde olucak -> status teki visitorID ve employeeID yerine namler gelmesi icin
    }
}