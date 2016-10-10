using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TrueTime.Controllers
{
    public class ProjectController : Controller
    {
        public ActionResult Create()
        {
            if (Session["LoginStatus"] == null ||
                (LoginStatus)Session["LoginStatus"] != LoginStatus.LoggedInAsAdministrator)
            {
                Session["LoginStatus"] = LoginStatus.IllegalLoginType;
                return View("CreateProjectResult");
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Models.Project aProject, string returnUrl)
        {
            InformationAccess ia = new InformationAccess();

            ia.Initialize();
            
            AzureProject ap = new AzureProject();

            aProject.toAzure(ap);

            bool bOk = await ia.InsertUpdateProject(ap);

            Session["ProjectOK"] = bOk;
            return View("CreateProjectResult");
        }
    }
}
