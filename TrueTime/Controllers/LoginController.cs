using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TrueTime.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Models.LoginUser model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            InformationAccess ia = new InformationAccess();

            ia.Initialize();

            AzureUser au = ia.GetUser(model.Username);

            Session["LoginStatus"] = (int)LoginStatus.NotLoggedIn;
            
            if (au != null && au.RowKey == model.Username && au.Pwd == model.Password)
            {
                if (au.TypeOfUser == (int)UserType.Consultant)
                    Session["LoginStatus"] = (int)LoginStatus.LoggedInAsConsultant;
                else if (au.TypeOfUser == (int)UserType.Administrator)
                    Session["LoginStatus"] = (int)LoginStatus.LoggedInAsAdministrator;
                else
                    Session["LoginStatus"] = (int)LoginStatus.IllegalLoginType;
            }
            
            return View("LoginResult");
        }
    }
}