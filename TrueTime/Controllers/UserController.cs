using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueTime.Models;

namespace TrueTime.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Models.User aUser, string returnUrl)
        {
            InformationAccess ia = new InformationAccess();
            AzureUser au = new AzureUser();
            ia.Initialize();

            aUser.toAzure(au);
            bool ok = await ia.InsertUpdateUser(au);
            return View("CreateUserResultView");
        }

        //public ActionResult Create()
        public ActionResult List()
        {
            InformationAccess ia = new InformationAccess();

            ia.Initialize();
            
            List<AzureUser> la = ia.GetAllUsers(UserType.Consultant);
            List<User> ul = new List<User>();
            foreach (AzureUser a in la)
            {
                User u = new Models.User();

                u.fromAzure(a);

                ul.Add(u);
            }
            return View(ul);
        }
    }
}