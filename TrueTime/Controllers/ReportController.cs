using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrueTime;
namespace TrueTime.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report()
        {
            InformationAccess ia = new InformationAccess();

            ia.Initialize();

            //List<ConsultantTime> ct = ia.GetConsultantTimePerWeek(DateTime.Now);
            return View(); //there is no view yet; just dry-running functionality
        }
    }
}
