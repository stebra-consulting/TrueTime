using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrueTime.Models;

namespace TrueTime.Controllers
{
    public class UserProjectController : Controller
    {
        // GET: UserProject
        public ActionResult Index()
        {
            UserProject2 u = RefreshScreen();
            
            return View(u);
        }
#if false

        // GET: UserProject/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserProject/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserProject/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProject/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserProject/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: UserProject/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserProject/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
#endif
#if false
        public ActionResult List()
        {
            InformationAccess ia = new InformationAccess();
            List<AzureUserProject> la;
            List<UserProject> lu = new List<UserProject>();
            string consultant = string.Empty;

            ia.Initialize();

            if (Session["LoginName"] != null)
                consultant = (string)Session["LoginName"];

            la = ia.GetUserProjects(consultant);

            if (la != null)
            {
                foreach(AzureUserProject a in la)
                {
                    UserProject up = new UserProject();

                    up.fromAzure(a);

                    lu.Add(up);
                }
            }
            return View(lu.OrderBy(p => p.PartitionKey));
        }
#endif
      
        /// <summary>
        /// Helper function that calculates the number of consultant projects and all projects
        /// </summary>
        UserProject2 RefreshScreen()
        {
            UserProject2 u = new UserProject2();
            InformationAccess ia = new InformationAccess();

            ia.Initialize();

            u.ConsultantProjects = ia.GetUserProjects((string)Session["LoginName"]);
            u.AllProjects = ia.GetAllProjects();

            return u;
        }

        /// <summary>
        /// Removes a project from the consultant's set of projects
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> LeftButton(UserProject2 up, string returnUrl)
        {
            InformationAccess ia = new InformationAccess();

            ia.Initialize();

            bool x = await ia.DeleteUserProject((string)Session["LoginName"], up.SelectedProject);
            return Index();
        }

        /// <summary>
        /// Adds a project to the consultant's set of projects. The function should never remove
        /// anything from up.AllProjects!
        /// </summary>
        [HttpPost]
        public ActionResult RightButton(UserProject2 up, string returnUrl)
        {
            //add if not already exists
            /*
            AzureUserProject aup = new AzureUserProject();

            aup.PartitionKey = up.SelectedProject;
            aup.RowKey = (string)Session["LoginName"];
            aup.Deleted = false;
            up.ConsultantProjects.Add(aup);
            */
            return Index();
        }

        bool RemoveUserFromProject(string projectName, string userName)
        {
            return true;
        }
    }
}
