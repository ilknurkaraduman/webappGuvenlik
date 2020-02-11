using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppGuvenlik.Models;
using System.Web.Mvc;
using Proje.MvcWeb.Models;
using WebAppGuvenlik.Helper.SessionHelper;
using System.Net.Mail;
using System.Net;
using WebAppGuvenlik.Models.Account;
using System.Web.Helpers;

namespace WebAppGuvenlik.Controllers
{
    public class AccountController : Controller
    {

        private GuvenlikDbContext dbContext = new GuvenlikDbContext();

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(model.UserName) || !string.IsNullOrEmpty(model.Password))
                {
                    SessionLoginResult sessionLoginResult = SessionHelper.Login(model.UserName, model.Password);        //Oturum Acma Isteğini Gerçekleştiriyoruz.
                    if (sessionLoginResult.IsSuccess)                                                                   //Oturumun Açılıp Açılmadığı Kontrolu.
                    {
                        //yonlendirme kısmını yazacagımız kısım
                        //role göre sayfalara yönlendirme.
                        if (SessionHelper.CurrentUser.RoleId == 1)
                        {
                            return RedirectToAction("Users", "Admin");
                        }
                        else if (SessionHelper.CurrentUser.RoleId == 2)
                        {
                            return RedirectToAction("ViewVisitors", "Employee");
                        }
                        else if (SessionHelper.CurrentUser.RoleId == 3)
                        {
                            return RedirectToAction("Visitors", "Security");
                        }
                        else
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    }
                    else
                    {
                        ViewBag.Error = sessionLoginResult.Message;
                        return View(model);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

            }
            return View(model); //Kullanicinin girdigi bilgiler tekrar karsisina gelir formu bastan doldurmaz.
        }

        // GET: Account
        public ActionResult Logout()
        {
            //var authManager = HttpContext.GetOwinContext().Authentication; // Olusturdugumuz cookieyi sistemden sildik.
            //authManager.SignOut();
            SessionHelper.Logout();
            return RedirectToAction("Login", "Account");
        }

        public ActionResult NotAuthorized()
        {
            return View();
        }





        //şifremi unuttum kısmı icin ekledikgim actionlar (+)ek olarak web.config'te mail setting tanımladım..

        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {
            string resetCode = Guid.NewGuid().ToString();
            var verifyUrl = "/Account/ResetPassword/" + resetCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);
            using (var context = new GuvenlikDbContext())
            {
                var getUser = (from s in context.User where s.Mail == EmailID select s).FirstOrDefault();
                if (getUser != null)
                {
                    getUser.ResetPasswordCode = resetCode;

                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 

                    context.Configuration.ValidateOnSaveEnabled = false;
                    context.SaveChanges();

                    var subject = "Password Reset Request";
                    var body = "Hi " + getUser.Name + ", <br/> You recently requested to reset your password for your account. Click the link below to reset it. " +

                         " <br/><br/><a href='" + link + "'>" + link + "</a> <br/><br/>" +
                         "If you did not request a password reset, please ignore this email or reply to let us know.<br/><br/> Thank you";

                    SendEmail(getUser.Mail, body, subject);

                    ViewBag.Message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    ViewBag.Message = "User doesn't exists.";
                    return View();
                }
            }

            return View();
        }

        private void SendEmail(string emailAddress, string body, string subject)
        {
            using (MailMessage mm = new MailMessage("ilknurkaraduman.7@gmail.com", emailAddress))
            {
                mm.Subject = subject;
                mm.Body = body;

                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("ilknurkaraduman.7@gmail.com", "webappguvenlik");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
        }

        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (var context = new GuvenlikDbContext())
            {
                var user = context.User.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            if (ModelState.IsValid)
            {
                using (var context = new GuvenlikDbContext())
                {
                    var user = context.User.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        //you can encrypt password here, we are not doing it
                        user.Password = model.NewPassword;
                        //make resetpasswordcode empty string now
                        user.ResetPasswordCode = "";
                        //to avoid validation issues, disable it
                        context.Configuration.ValidateOnSaveEnabled = false;
                        context.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "Something invalid";
            }
            ViewBag.Message = message;
            return View(model);
        }

    }
}