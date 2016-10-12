using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueTime.Models;

namespace TrueTime.Controllers
{
    public class UserProjectController : Controller
    {
#if false
        // GET: UserProject
        public ActionResult Index()
        {
            return View();
        }

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
        public ActionResult List()
        {
            InformationAccess ia = new InformationAccess();
            List<AzureUserProject> la;
            List<UserProject> lu = new List<UserProject>();

            ia.Initialize();

            la = ia.GetAllProjects();

            foreach(AzureUserProject a in la)
            {
                UserProject up = new UserProject();

                up.fromAzure(a);

                lu.Add(up);
            }
            return View(lu.OrderBy(p => p.PartitionKey));
        }
    }
}
