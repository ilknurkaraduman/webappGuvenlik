using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppGuvenlik.Helper.AppAuthorize;

namespace WebAppGuvenlik.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [AppAuthorize(1)]
        [AppAuthorize(2)]
        [AppAuthorize(3)]
        public ActionResult Index()
        {
            return View();
        }        
    }
}