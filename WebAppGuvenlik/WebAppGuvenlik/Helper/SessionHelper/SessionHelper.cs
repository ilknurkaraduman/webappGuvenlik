using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppGuvenlik.Helper.SessionHelper
{
    public class SessionHelper
    {

        //suanki kullanıcıyı bulmak için kullanılan properties tir.
        //kullanıcı null ise null döndürür.
        //kullanıcı acilacaksada gönderilen kullanıcıyı set eden 
        public static SessionUser CurrentUser
        {
            get
            {
                if (HttpContext.Current.Session["WebApp_CurrentUser"] == null)
                {
                    return null;
                }
                else
                {
                    return HttpContext.Current.Session["WebApp_CurrentUser"] as SessionUser;
                }
            }

            set
            {
                HttpContext.Current.Session["WebApp_CurrentUser"] = value;

            }
        }


        //kimlik dogrulama kısmında kullanıcı null değilse ve ıdsi sıfırdan buyukse.
        public static bool IsAuthenticated
        {
            get
            {
                //if (CurrentUser != null && !string.IsNullOrEmpty(CurrentUser.TID))
                if (CurrentUser != null && CurrentUser.Id > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        //kullanıcı adı ve sifreye gore-> kullanıcı kontrolu yapılıp, kullanıcı varsa ona göre geriye deger donduren method.
        public static SessionLoginResult Login(string userName, string userPassword)
        {
            GuvenlikDbContext dbContext = new GuvenlikDbContext();
            //kayıtlı kullanıcı var mı? yada aktif mi? username ve passworde barak bize kullanıcı döndüren fonksiyon. 
            //var existUser = new UserService().GetByUserNameAndPassword(userName, userPassword);
            var existUser = dbContext.User.Where(x => x.UserName == userName && x.Password == userPassword).FirstOrDefault();
            if (existUser == null)
            {
                return new SessionLoginResult(false, "Incorrect Username or Password");
            }

            SessionUser currentUser = new SessionUser();
            currentUser.Id = existUser.Id;
            currentUser.Name = existUser.Name;
            currentUser.Surname = existUser.Surname;
            currentUser.RoleId = existUser.UserCategoryID;
            currentUser.Password = existUser.Password;
            currentUser.Username = existUser.UserName;
            CurrentUser = currentUser;

            return new SessionLoginResult(true, "Successful Login");
        }

        //cıkıs yapıldıgında kullanılacak method.
        public static bool Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.RemoveAll();
            return true;
        }
    }
}