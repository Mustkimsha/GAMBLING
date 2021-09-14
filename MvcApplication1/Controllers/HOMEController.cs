using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcApplication1.Models;
using System.Security;

namespace MvcApplication1.Controllers
{
    public class HOMEController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(USERTBL USERLOG)
        {
            var unm = USERLOG.USERNAME;
            var upass = USERLOG.PASSWORD;
            using(pm_admindbEntities1 log = new pm_admindbEntities1())
            {
                var userdt = log.USERTBLs.Where(x => x.USERNAME==unm && x.PASSWORD==upass).FirstOrDefault();
                if (userdt == null)
                {
                    ViewBag.ERRORMSG = "USERNAME OR PASSWORD IS INCCORECT";
                    return View();
                }
                else
                {
                    Session["USERID"] = userdt.UID ;
                    Session["USERNAME"] = userdt.USERNAME;
                    return View("DASHBOARD");
                }
            }
        }

    }
}
