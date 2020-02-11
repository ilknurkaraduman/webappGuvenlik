using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppGuvenlik.Helper.AppAuthorize;
using WebAppGuvenlik.Helper.SessionHelper;
using WebAppGuvenlik.Models;
using WebAppGuvenlik.Models.Employee;
using WebAppGuvenlik.Models.Security;

namespace WebAppGuvenlik.Controllers
{
    public class SecurityController : Controller
    {

        private GuvenlikDbContext dbContext = new GuvenlikDbContext();

        [AppAuthorize(3)]
        public ActionResult Index()
        {
            return View();
        }

        [AppAuthorize(3)]
        public ActionResult Visitors(string sortOrder, string searchString) //ziyarete gelen tum ziyaretciler (report*)
        {
            List<VisitorsModel> model = new List<VisitorsModel>();
            model = (from v in dbContext.Visitor.ToList() //dnm-> ustteki modelde Id ye karsılık gelen user Name icerir
                     join u in dbContext.User.ToList() on v.EmployeeID equals u.Id
                     join uu in dbContext.User.ToList() on v.SecurityID equals uu.Id
                     join s in dbContext.Status.ToList() on v.ID equals s.VisitorID
                     select new VisitorsModel()
                     {
                         ID = v.ID,
                         Name = v.Name,
                         Surname = v.Surname,
                         Phone = v.Phone,
                         Date = v.Date,
                         Description = v.Description,
                         EmployeeId = v.EmployeeID,
                         SecurityId = v.SecurityID,
                         EmployeeName = u.Name,
                         SecurityName = uu.Name,
                         VisitorStatus = s.Name
                     }).ToList();
            var visitors = from s in model
                           select s;

            ViewBag.NameSortParm1 = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm2 = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
            ViewBag.NameSortParm3 = String.IsNullOrEmpty(sortOrder) ? "employee_desc" : "";
            ViewBag.NameSortParm4 = String.IsNullOrEmpty(sortOrder) ? "security_desc" : "";
            ViewBag.NameSortParm5 = String.IsNullOrEmpty(sortOrder) ? "status_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (!String.IsNullOrEmpty(searchString)) //isim veya soyisime gore arama
            {
                visitors = visitors.Where(s => s.Name.Contains(searchString)
                                       || s.Surname.Contains(searchString)
                                       || s.VisitorStatus.Contains(searchString));
            }

            switch (sortOrder) //desc biciminde siralar -default id dir.
            {
                case "status_desc":
                    visitors = visitors.OrderByDescending(s => s.VisitorStatus);
                    break;
                case "name_desc":
                    visitors = visitors.OrderByDescending(s => s.Name);
                    break;
                case "surname_desc":
                    visitors = visitors.OrderByDescending(s => s.Surname);
                    break;
                case "employee_desc":
                    visitors = visitors.OrderByDescending(s => s.EmployeeName);
                    break;
                case "security_desc":
                    visitors = visitors.OrderByDescending(s => s.SecurityName);
                    break;
                //case "Date":
                //    visitors = visitors.OrderBy(s => s.Date);
                //    break;
                //case "date_desc":
                //    visitors = visitors.OrderByDescending(s => s.Date);
                //    break;
                default:
                    visitors = visitors.OrderByDescending(s => s.ID);
                    break;
            }

            return View(visitors.ToList());

            //return View(visitors.ToList());
        }

        [AppAuthorize(3)]
        public ActionResult VisitorAdd()
        {
            List<SelectListItem> employeelist = new List<SelectListItem>();
            employeelist = (from u in dbContext.User.ToList()
                            where u.UserCategoryID == 2
                            select new SelectListItem()
                            {
                                Value = u.Id.ToString(),
                                Text = u.Name
                            }).ToList();
            ViewBag.employeeList = employeelist;
            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VisitorAdd(Visitor visitor)
        {
            if (ModelState.IsValid)
            {
                visitor.SecurityID = SessionHelper.CurrentUser.Id;
                visitor.Date = DateTime.Now;
                dbContext.Visitor.Add(visitor);
                dbContext.SaveChanges();

                Status status = new Status();
                status.Name = "waiting"; //ziyaret durumu eklenir
                status.EmployeeID = visitor.EmployeeID;
                status.VisitorID = visitor.ID;
                dbContext.Status.Add(status);
                dbContext.SaveChanges();
                return RedirectToAction("VisitorStatus");
            }
            List<SelectListItem> employeelist = new List<SelectListItem>();
            employeelist = (from u in dbContext.User.ToList()
                            where u.UserCategoryID == 2
                            select new SelectListItem()
                            {
                                Value = u.Id.ToString(),
                                Text = u.Name
                            }).ToList();
            ViewBag.employeeList = employeelist;
            return View();

        }

        [AppAuthorize(3)]
        public ActionResult VisitorRemove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Visitor visitor = dbContext.Visitor.Find(id);
            if (visitor == null)
            {
                return HttpNotFound();
            }
            return View(visitor);
        }
        [HttpPost, ActionName("VisitorRemove")]
        [ValidateAntiForgeryToken]
        public ActionResult UserRemove(int id)
        {
            Visitor visitor = dbContext.Visitor.Find(id);
            dbContext.Visitor.Remove(visitor);
            dbContext.SaveChanges();
            return RedirectToAction("Visitors");
        }

        [AppAuthorize(3)]
        public ActionResult VisitorEdit(int? id)
        {
            List<SelectListItem> employeelist = new List<SelectListItem>();
            employeelist = (from u in dbContext.User.ToList()
                            where u.UserCategoryID == 2
                            select new SelectListItem()
                            {
                                Value = u.Id.ToString(),
                                Text = u.Name
                            }).ToList();
            ViewBag.employeeList = employeelist;
            List<Visitor> list = dbContext.Visitor.ToList();
            Visitor dnm = dbContext.Visitor.Where(k => k.ID == id).SingleOrDefault();
            Visitor model = new Visitor()
            {
                ID = dnm.ID,
                Name = dnm.Name,
                Surname = dnm.Surname,
                Phone = dnm.Phone,
                Date = dnm.Date,
                Description = dnm.Description,
                EmployeeID = dnm.EmployeeID,
                SecurityID = dnm.SecurityID
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult VisitorEdit(Visitor m, int? id)
        {
            if (ModelState.IsValid)
            {
                List<Visitor> list = dbContext.Visitor.ToList();
                Visitor dnm = dbContext.Visitor.Where(k => k.ID == m.ID).SingleOrDefault();
                dnm.Name = m.Name;
                dnm.Surname = m.Surname;
                dnm.Phone = m.Phone;
                dnm.Date = DateTime.Now;
                dnm.Description = m.Description;
                dnm.EmployeeID = m.EmployeeID;
                dnm.SecurityID = SessionHelper.CurrentUser.Id;
                dbContext.SaveChanges();
                ViewBag.sonuc = "Update Record";
                return RedirectToAction("Visitors");
            }
            List<SelectListItem> employeelist = new List<SelectListItem>();
            employeelist = (from u in dbContext.User.ToList()
                            where u.UserCategoryID == 2
                            select new SelectListItem()
                            {
                                Value = u.Id.ToString(),
                                Text = u.Name
                            }).ToList();
            ViewBag.employeeList = employeelist;
            List<Visitor> listt = dbContext.Visitor.ToList();
            Visitor x = dbContext.Visitor.Where(k => k.ID == id).SingleOrDefault();
            Visitor model = new Visitor()
            {
                ID = x.ID,
                Name = x.Name,
                Surname = x.Surname,
                Phone = x.Phone,
                Date = x.Date,
                Description = x.Description,
                EmployeeID = x.EmployeeID,
                SecurityID = x.SecurityID
            };
            return View(model);
        }

        [AppAuthorize(3)]
        public ActionResult Report1() //bugün gelen ziyaretçilerin listesi; dayofyearlar esitlendi
        {
            List<VisitorsModel> model = new List<VisitorsModel>();
            model = (from v in dbContext.Visitor.ToList()
                     join u in dbContext.User.ToList() on v.EmployeeID equals u.Id
                     join uu in dbContext.User.ToList() on v.SecurityID equals uu.Id
                     join s in dbContext.Status.ToList() on v.ID equals s.VisitorID
                     where v.Date.Value.DayOfYear == DateTime.Today.DayOfYear && v.Date != null
                     select new VisitorsModel()
                     {
                         ID = v.ID,
                         Name = v.Name,
                         Surname = v.Surname,
                         Phone = v.Phone,
                         Date = v.Date,
                         Description = v.Description,
                         EmployeeId = v.EmployeeID,
                         SecurityId = v.SecurityID,
                         EmployeeName = u.Name,
                         SecurityName = uu.Name,
                         VisitorStatus = s.Name
                     }).ToList();
            return View(model);
        }

        //[AppAuthorize(3)]
        //public ActionResult Report2() //toplam kayıt sayısı, onaylanan toplam, reddedilen toplam (?)tek sayfa 3 bolmeli olabilir
        //{

        //    return View();
        //}

        [AppAuthorize(3)]
        public ActionResult VisitorStatus() //ziyaretcilerin ziyaret durumlari (?) idler name olarak gozukmeli
        {
            List<StatusModel> model = new List<StatusModel>();
            model = (from v in dbContext.Visitor.ToList()
                     join s in dbContext.Status.ToList() on v.ID equals s.VisitorID
                     where v.Date.Value.Hour == DateTime.Now.Hour && v.Date != null && v.Date.Value.DayOfYear == DateTime.Today.DayOfYear  //sadece suanki saat dilimde kayıt olan ziyaretcilerin ziyeret durumları listelenir
                     select new StatusModel()
                     {
                         ID = s.ID,
                         Name = s.Name,
                         Description = v.Description,
                         VisitorID = v.ID,
                         VisitorName = v.Name,
                         EndDate = s.EndDate
                     }).ToList();

            //var dnm = (from u in dbContext.Status.ToList()
            //           join k in dbContext.Visitor.ToList() on u.VisitorID equals k.ID
            //           where k.Date.Value.Hour == DateTime.Now.Hour && k.Date != null && k.Date.Value.DayOfYear == DateTime.Today.DayOfYear  //sadece suanki saat dilimde kayıt olan ziyaretcilerin ziayeret durumları listelenir
            //           select new Status()
            //           {
            //               ID = u.ID,
            //               Name = u.Name,
            //               Description = u.Description,
            //               EmployeeID = u.EmployeeID,
            //               VisitorID = u.VisitorID,
            //               EndDate = u.EndDate
            //           }).ToList();
            return View(model);
        }

    }
}