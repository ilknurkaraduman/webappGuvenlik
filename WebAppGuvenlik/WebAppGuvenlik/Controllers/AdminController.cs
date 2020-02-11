using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppGuvenlik.Models;
using System.Web.Mvc;
using WebAppGuvenlik.Helper.AppAuthorize;
using WebAppGuvenlik.Helper.SessionHelper;
using System;
using WebAppGuvenlik.Models.Admin;
using System.Net;

namespace WebAppGuvenlik.Controllers
{
    public class AdminController : Controller
    {
        private GuvenlikDbContext dbContext = new GuvenlikDbContext();

        // GET: Admin
        //[AppAuthorize(1)]
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [AppAuthorize(1)]
        public ActionResult Users(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm1 = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortParm2 = String.IsNullOrEmpty(sortOrder) ? "surname_desc" : "";
            ViewBag.NameSortParm3 = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewBag.NameSortParm4 = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm5 = String.IsNullOrEmpty(sortOrder) ? "cat_id_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var users = from s in dbContext.User
                        select s;
            if (!String.IsNullOrEmpty(searchString))   //isim veya soyisime gore arama
            {
                users = users.Where(s => s.Name.Contains(searchString)
                                       || s.Surname.Contains(searchString));
            }
            switch (sortOrder)   //desc biciminde siralar->default id'dir.
            {
                case "name_desc":
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case "surname_desc":
                    users = users.OrderByDescending(s => s.Surname);
                    break;
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "id_desc":
                    users = users.OrderByDescending(s => s.UserCategoryID);
                    break;
                case "cat_id_desc":
                    users = users.OrderByDescending(s => s.UserCategoryID);
                    break;
                default:
                    users = users.OrderBy(s => s.Id);
                    break;
            }
            return View(users.ToList());
        }

        [AppAuthorize(1)]
        public ActionResult UserAdd()
        {
            List<SelectListItem> usercategorylist = new List<SelectListItem>();
            usercategorylist = (from u in dbContext.UserCategory.ToList()
                                select new SelectListItem()
                                {
                                    Value = u.ID.ToString(),
                                    Text = u.Name
                                }).ToList();
            ViewBag.UserCatList = usercategorylist;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserAdd(User user)
        {
            if (ModelState.IsValid)
            {
                if (dbContext.User.Where(u => u.UserName == user.UserName).Any())
                {
                    //Do what do u need to do...
                    ViewBag.Message1 = "UserName taken";
                }
                else if (dbContext.User.Where(u => u.Mail == user.Mail).Any())
                {
                    //Do what do u need to do...
                    ViewBag.Message2 = "This email address is registered";
                }
                else
                {
                    dbContext.User.Add(user);
                    dbContext.SaveChanges();
                    return RedirectToAction("Users");
                }

            }
            List<SelectListItem> usercategorylist = new List<SelectListItem>();
            usercategorylist = (from u in dbContext.UserCategory.ToList()
                                select new SelectListItem()
                                {
                                    Value = u.ID.ToString(),
                                    Text = u.Name
                                }).ToList();
            ViewBag.UserCatList = usercategorylist;
            return View();
        }

        public ActionResult UserRemove(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = dbContext.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("UserRemove")]
        [ValidateAntiForgeryToken]
        public ActionResult UserRemove(int id)
        {
            User user = dbContext.User.Find(id);
            dbContext.User.Remove(user);
            dbContext.SaveChanges();
            return RedirectToAction("Users");
        }

        [AppAuthorize(1)]
        public ActionResult UserEdit(int? id) //users viewinden gelen id'e gore kullanici duzenle
        {
            List<SelectListItem> usercategorylist = new List<SelectListItem>();
            usercategorylist = (from u in dbContext.UserCategory.ToList()
                                select new SelectListItem()
                                {
                                    Value = u.ID.ToString(),
                                    Text = u.Name
                                }).ToList();
            ViewBag.UserCatList = usercategorylist;
            List<User> list = dbContext.User.ToList();
            User dnm = dbContext.User.Where(k => k.Id == id).SingleOrDefault();
            User model = new User()
            {
                Id = dnm.Id,
                Name = dnm.Name,
                Surname = dnm.Surname,
                Mail = dnm.Mail,
                Phone = dnm.Phone,
                UserName = dnm.UserName,
                Password = dnm.Password,
                UserCategoryID = dnm.UserCategoryID
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult UserEdit(User m, int? id)
        {
            var usermodel = (from u in dbContext.User.ToList()
                       where u.Id != id
                       select new User()
                       {
                           Id = u.Id,
                           Name = u.Name,
                           Surname = u.Surname,
                           Mail = u.Mail,
                           Phone = u.Phone,
                           UserName = u.UserName,
                           Password = u.Password,
                           UserCategoryID = u.UserCategoryID,
                           ResetPasswordCode = u.ResetPasswordCode
                       }).ToList();

            if (ModelState.IsValid)
            {
                if (usermodel.Where(u => u.UserName == m.UserName).Any())
                {
                    //Do what do u need to do...
                    ViewBag.Message1 = "UserName taken";
                }
                else if (usermodel.Where(u => u.Mail == m.Mail).Any())
                {
                    //Do what do u need to do...
                    ViewBag.Message2 = "This email address is registered";
                }
                else
                {
                    List<User> list = dbContext.User.ToList();
                    User dnm = dbContext.User.Where(k => k.Id == m.Id).SingleOrDefault();
                    dnm.Name = m.Name;
                    dnm.Surname = m.Surname;
                    dnm.Mail = m.Mail;
                    dnm.Phone = m.Phone;
                    dnm.UserName = m.UserName;
                    dnm.Password = m.Password;
                    dnm.UserCategoryID = m.UserCategoryID;
                    dbContext.SaveChanges();
                    ViewBag.sonuc = "Update Record";
                    return RedirectToAction("Users");
                }
            }
            List<SelectListItem> usercategorylist = new List<SelectListItem>();
            usercategorylist = (from u in dbContext.UserCategory.ToList()
                                select new SelectListItem()
                                {
                                    Value = u.ID.ToString(),
                                    Text = u.Name
                                }).ToList();
            ViewBag.UserCatList = usercategorylist;
            List<User> listt = dbContext.User.ToList();
            User x = dbContext.User.Where(k => k.Id == id).SingleOrDefault();
            User model = new User()
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Mail = x.Mail,
                Phone = x.Phone,
                UserName = x.UserName,
                Password = x.Password,
                UserCategoryID = x.UserCategoryID
            };
            return View(model);
        }

        [AppAuthorize(1)]
        public ActionResult Report1(int selectedId = 0) //Personellere Gelen Ziyaretçiler->once personel secilip, personele gore ziyaretci listesi
                                                        //dropdowndan secilecek olan userID-> selectedId
        {
            AdminTopModel model = new AdminTopModel();

            if (selectedId == 0) //dropdownlist secili degil iken status tamamı gelir
            {
                model.SelectedId = 0;
                model.StatusList = (from u in dbContext.Status.ToList()
                                    join e in dbContext.User.ToList() on u.EmployeeID equals e.Id
                                    join v in dbContext.Visitor.ToList() on u.VisitorID equals v.ID
                                    select new ReportVisitoryModel()
                                    {
                                        ID = u.ID,
                                        Name = u.Name,
                                        Description = u.Description,
                                        EmployeeName = e.Name,
                                        VisitorName = v.Name,
                                        EndDate = u.EndDate
                                    }).OrderByDescending(x => x.ID).ToList();
                model.UserList = (from u in dbContext.User.ToList()
                                  where u.UserCategoryID == 2
                                  select new SelectListItem()
                                  {
                                      Value = u.Id.ToString(),
                                      Text = u.Name
                                  }).ToList();
            }
            else
            {
                model.SelectedId = selectedId; //dropdownlistten secilen deger modelimdeki SelectedId ye esit
                model.StatusList = (from u in dbContext.Status.ToList()
                                    join e in dbContext.User.ToList() on u.EmployeeID equals e.Id
                                    join v in dbContext.Visitor.ToList() on u.VisitorID equals v.ID
                                    where u.EmployeeID == selectedId //secilen deger status tablosundaki employeeId ye esit
                                    select new ReportVisitoryModel()
                                    {
                                        ID = u.ID,
                                        Name = u.Name,
                                        Description = u.Description,
                                        EmployeeName = e.Name,
                                        VisitorName = v.Name,
                                        EndDate = u.EndDate
                                    }).OrderByDescending(x => x.ID).ToList();
                model.UserList = (from u in dbContext.User.ToList()
                                  where u.UserCategoryID == 2 //sadece employee gelir
                                  select new SelectListItem()
                                  {
                                      Value = u.Id.ToString(),
                                      Text = u.Name
                                  }).ToList();
            }
            return View(model);
        }

        [AppAuthorize(1)]
        public ActionResult Report2() //hangi personele toplam kaç adet ziyaretci gelmis - how many visitors to which employee
        {
            List<AdminViewModel> model = new List<AdminViewModel>();
            model = dbContext.Visitor.GroupBy(u => u.EmployeeID).Select(g => new AdminViewModel() { EmployeeId = g.Key, VisitorCount = g.Count() }).ToList();
            var dnm = (from u in model.ToList() //dnm-> ustteki modelde Id ye karsılık gelen user Name icerir
                       join e in dbContext.User.ToList() on u.EmployeeId equals e.Id
                       select new AdminViewModel()
                       {
                           EmployeeId = u.EmployeeId,
                           VisitorCount = u.VisitorCount,
                           EmployeeName = e.Name
                       }).ToList();
            return View(dnm);
        }

        //[AppAuthorize(1)]
        //public ActionResult Report()
        //{
        //    return View();
        //}
    }
}