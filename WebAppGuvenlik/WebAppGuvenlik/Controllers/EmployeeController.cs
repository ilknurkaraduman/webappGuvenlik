using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppGuvenlik.Helper.AppAuthorize;
using WebAppGuvenlik.Helper.SessionHelper;
using WebAppGuvenlik.Models;
using WebAppGuvenlik.Models.Employee;
using WebAppGuvenlik.Models.Security;

namespace WebAppGuvenlik.Controllers
{
    public class EmployeeController : Controller
    {
        private GuvenlikDbContext dbContext = new GuvenlikDbContext();

        // GET: Employee
        [AppAuthorize(2)]
        public ActionResult Index()
        {
            return View();
        }

        [AppAuthorize(2)]
        public ActionResult ViewVisitors() //personelin kendisine gelen ziyaretcilerin listesi 
                                           //IDlerin Namelerinin gelmesi icin-> visitorsmodel oluşturdum
        {
            int sessionId = SessionHelper.CurrentUser.Id;   //giris yapan kullanıcının idsini alir.

            //int[] array = dbContext.Status.Where(x => x.EmployeeID == sessionId).Select(x => x.VisitorID).ToArray();
            //var result = dbContext.Visitor.Where(x => x.EmployeeID == sessionId && !array.Contains(x.ID)).ToList();
            //(+)diger yontem de visitoridleri array'e aktarıp daha sonra onlar ile visitor.Idsi esit olmayan visitorları getir diyoruz

            List<VisitorsModel> model = new List<VisitorsModel>();
            model = (from v in dbContext.Visitor.ToList()
                     join u in dbContext.User.ToList() on v.EmployeeID equals u.Id
                     join uu in dbContext.User.ToList() on v.SecurityID equals uu.Id
                     join s in dbContext.Status.ToList() on v.ID equals s.VisitorID
                     where v.EmployeeID == sessionId && v.Click != true
                     select new VisitorsModel()
                     {
                         ID = v.ID,
                         Name = v.Name,
                         Surname = v.Surname,
                         Phone = v.Phone,
                         Date = v.Date,
                         Description = v.Description,
                         SecurityId = v.SecurityID,
                         SecurityName = uu.Name,
                         VisitorStatus = s.Name
                     }).OrderByDescending(x => x.ID).ToList();
            return View(model);
        }

        [AppAuthorize(2)]
        public ActionResult Confirm(int id) //ziyaretci onay eylemi
        {
            List<Visitor> list = dbContext.Visitor.ToList();
            Visitor dnm = dbContext.Visitor.Where(k => k.ID == id).SingleOrDefault();
            dnm.Click = true;
            dbContext.SaveChanges(); //tıklanıldı olarak kaydeder

            List<Status> listt = dbContext.Status.ToList();
            Status x = dbContext.Status.Where(k => k.VisitorID == id).SingleOrDefault(); //actionlink ten id yi cekiyor
            x.Name = "confirmed"; //status.name'i confirmed olarak kaydeder
            x.Description = x.Description;
            x.EmployeeID = x.EmployeeID;
            x.VisitorID = x.VisitorID;
            x.EndDate = x.EndDate; 
            dbContext.SaveChanges();
            return RedirectToAction("ViewConfirm");
        }

        [AppAuthorize(2)]
        public ActionResult ViewConfirm() //onaylanan ziyaretciler ve EndVisit eylemi icerir
        {
            //int sessionId = SessionHelper.CurrentUser.Id;

            List<StatusModel> model = new List<StatusModel>();
            model = (from v in dbContext.Visitor.ToList() //dnm-> ustteki modelde Id ye karsılık gelen user Name icerir
                     join u in dbContext.User.ToList() on v.EmployeeID equals u.Id
                     join uu in dbContext.User.ToList() on v.SecurityID equals uu.Id
                     join s in dbContext.Status.ToList() on v.ID equals s.VisitorID
                     where v.EmployeeID == SessionHelper.CurrentUser.Id && s.Name == "confirmed" && s.Click != true //giris yapan kullanıcıya ait olanlar gelicek
                     select new StatusModel()
                     {
                         ID = s.ID,
                         Name = s.Name,
                         Description = v.Description,
                         EmployeeID = v.EmployeeID,
                         VisitorID = v.ID,
                         VisitorName = v.Name,
                         EmployeeName = u.Name,
                         SecurityName = uu.Name
                     }).OrderByDescending(x => x.ID).ToList();
            return View(model);
        }

        [AppAuthorize(2)]
        public ActionResult EndVisit(int? id) //ziyareti bitir eylemi
        {
            List<Status> list = dbContext.Status.ToList();
            Status dnm = dbContext.Status.Where(k => k.ID == id).SingleOrDefault(); //actionlink ten id yi cekiyor
            dnm.Name = "visit is over"; //status.name'i bitti olarak kaydeder
            dnm.Description = dnm.Description;
            dnm.EmployeeID = dnm.EmployeeID;
            dnm.VisitorID = dnm.VisitorID;
            dnm.EndDate = DateTime.Now; //status.enddate'i; ziyaret bittigi anki tiklanilan zamani alir
            dbContext.SaveChanges();
            return RedirectToAction("ViewVisitors");
        }

        [AppAuthorize(2)]
        public ActionResult Reject(int id) //ziyaretci red eylemi
        {
            List<Visitor> list = dbContext.Visitor.ToList();
            Visitor dnm = dbContext.Visitor.Where(k => k.ID == id).SingleOrDefault();
            dnm.Click = true;
            dbContext.SaveChanges(); //tıklanıldı olarak kaydeder

            List<Status> listt = dbContext.Status.ToList();
            Status x = dbContext.Status.Where(k => k.VisitorID == id).SingleOrDefault(); //actionlink ten id yi cekiyor
            x.Name = "rejected"; //status.name'i rejected olarak kaydeder
            x.Description = x.Description;
            x.EmployeeID = x.EmployeeID;
            x.VisitorID = x.VisitorID;
            //x.EndDate = x.EndDate;
            x.EndDate = DateTime.Now;
            dbContext.SaveChanges();
            return RedirectToAction("ViewReject");
        }

        [AppAuthorize(2)]
        public ActionResult ViewReject() //reddedilen ziyaretler ve SendDescription eylemi icerir
        {
            List<StatusModel> model = new List<StatusModel>();
            model = (from v in dbContext.Visitor.ToList() //dnm-> ustteki modelde Id ye karsılık gelen user Name icerir
                     join u in dbContext.User.ToList() on v.EmployeeID equals u.Id
                     join uu in dbContext.User.ToList() on v.SecurityID equals uu.Id
                     join s in dbContext.Status.ToList() on v.ID equals s.VisitorID
                     where v.EmployeeID == SessionHelper.CurrentUser.Id && s.Name == "rejected" && s.Click != true //giris yapan kullanıcıya ait olanlar gelicek
                     select new StatusModel()
                     {
                         ID = s.ID,
                         Name = s.Name,
                         Description = v.Description,
                         EmployeeID = v.EmployeeID,
                         VisitorID = v.ID,
                         VisitorName = v.Name,
                         EmployeeName = u.Name,
                         SecurityName = uu.Name
                     }).OrderByDescending(x => x.ID).ToList();

            return View(model);
        }

        [AppAuthorize(2)]
        public ActionResult SendDescription(int? id) //Red için açıklama mesajı yollar
        {
            List<Status> list = dbContext.Status.ToList();
            Status x = dbContext.Status.Where(k => k.ID == id).SingleOrDefault();
            x.Click = true;
            dbContext.SaveChanges(); //tıklanıldı olarak kaydeder

            List<Status> Slist = dbContext.Status.ToList();
            Status dnm = dbContext.Status.Where(k => k.ID == id ).SingleOrDefault();
            Status model = new Status()
            {
                ID = dnm.ID,
                Name = dnm.Name,
                Description = dnm.Description,
                EmployeeID = dnm.EmployeeID,
                VisitorID = dnm.VisitorID,
                EndDate = dnm.EndDate
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult SendDescription(Status m) //Red için açıklama mesajı yollar:POST
        {
            List<Visitor> Slist = dbContext.Visitor.ToList();
            Status dnm = dbContext.Status.Where(k => k.ID == m.ID).SingleOrDefault();
            dnm.Description = m.Description;
            dbContext.SaveChanges();
            return RedirectToAction("ViewVisitors");
        }


    }
}