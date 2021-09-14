using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace MvcApplication1.Controllers
{
    public class ADMINController : Controller
    {
        public ActionResult Index()
        {
            Session["USERID"] = "";
            Session["USERNAME"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(ADMINUSER USERLOG)
        {
            var unm = USERLOG.USERNAME;
            var upass = USERLOG.PASSWORD;
            using (pm_admindbEntities1 log = new pm_admindbEntities1())
            {
                var userdt = log.ADMINUSERs.Where(x => x.USERNAME == unm && x.PASSWORD == upass).FirstOrDefault();
                if (userdt == null)
                {
                    ViewBag.ERRORMSG = "USERNAME OR PASSWORD IS INCCORECT";
                    return View();
                }
                else
                {
                    Session["USERID"] = userdt.AUID;
                    Session["USERNAME"] = userdt.USERNAME;
                    return View("DASHBOARD");
                }
            }
        }

        public ActionResult DASHBOARD()
        {
            if (Session["USERID"] ==null)
            {
                return View("Index");
            }
            return View();
        }
        
        public ActionResult CRATEFRENCHISE()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }

            pm_admindbEntities1 log = new pm_admindbEntities1();
            List<FRENCHISE> frelist = log.FRENCHISEs.ToList();
            return View(frelist);
        }

        [HttpPost]
        public JsonResult CRATEFRENCHISEadd(string FUNAMEtxt, string USERNAMEtxt, string PASSWORDtxt, string CONTACTNOtxt)
        {
            FRENCHISE cf = new FRENCHISE();
            if (Session["USERID"] != null)
            {
                cf.AUID = Convert.ToInt32(Session["USERID"]);
            }
            cf.CONTACTNO = CONTACTNOtxt;
            cf.FUNAME = FUNAMEtxt;
            cf.PASSWORD = PASSWORDtxt;
            cf.USERNAME = USERNAMEtxt;

            pm_admindbEntities1 log = new pm_admindbEntities1();
            log.FRENCHISEs.Add(cf);
            log.SaveChanges();
            return Json(cf);
        }

        public ActionResult BALTRANSFERFRENCHISE()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }

            pm_admindbEntities1 log = new pm_admindbEntities1();
            List<AFTXN> AFTXNlist = log.AFTXNs.ToList();
            List<FRENCHISE> frelist = log.FRENCHISEs.ToList();
            ViewData["Frelist"] = log.FRENCHISEs.ToList();

            return View(AFTXNlist);
        }

        [HttpPost]
        public JsonResult BALTRANSFERFRENCHISEtransfer(string fuid, string tt, string txnaamt, string timetxt, string datetxt)
        {
            AFTXN ca = new AFTXN();
            int wbal;
            int frfbal;
            if (Session["USERID"] != null)
            {
                ca.AUID = Convert.ToInt32(Session["USERID"]);
            }
            ca.FUID = Convert.ToInt32(fuid);
            ca.TXNDATE = datetxt;
            ca.TXNTIME = timetxt;

            if (tt == "1")
            {
                ca.DEBIT = txnaamt;
            }
            else
            {
                ca.CREDIT = txnaamt;
            }

            pm_admindbEntities1 log = new pm_admindbEntities1();
            var frbal = log.FRENCHISEBALs.Where(x => x.FUID == ca.FUID).FirstOrDefault();

            if (frbal == null)
            {
                FRENCHISEBAL cfrbal = new FRENCHISEBAL();
                cfrbal.FUID = ca.FUID;
                if (tt == "1")
                {
                    var bal = Convert.ToInt32(txnaamt);
                    wbal = 0;
                    frfbal = 0;
                    wbal = wbal - bal;
                    cfrbal.BALANCE = Convert.ToString(wbal);
                }
                else
                {
                    var bal = Convert.ToInt32(txnaamt);
                    wbal = 0;
                    frfbal = 0;
                    wbal = wbal + bal;
                    cfrbal.BALANCE = Convert.ToString(wbal);
                }
                log.FRENCHISEBALs.Add(cfrbal);
            }
            else
            {
                if (tt == "1")
                {
                    var bal = Convert.ToInt32(txnaamt);
                    wbal = Convert.ToInt32(frbal.BALANCE);
                    frfbal = Convert.ToInt32(frbal.BALANCE);
                    wbal = wbal - bal;
                    frbal.BALANCE = Convert.ToString(wbal);
                }
                else
                {
                    var bal = Convert.ToInt32(txnaamt);
                    wbal = Convert.ToInt32(frbal.BALANCE);
                    frfbal = Convert.ToInt32(frbal.BALANCE);
                    wbal = wbal + bal;
                    frbal.BALANCE = Convert.ToString(wbal);
                }
            }

            log.AFTXNs.Add(ca);
            log.SaveChanges();

            var frport = log.FRENCHISEREPORTs.Where(x => x.FUID == ca.FUID && x.DATE == datetxt).FirstOrDefault();
            if (frport == null)
            {
                FRENCHISEREPORT frportnew = new FRENCHISEREPORT();
                frportnew.FUID = ca.FUID;
                frportnew.DATE = datetxt;
                frportnew.OB = Convert.ToString(frfbal);
                
                if (tt == "1")
                {
                    txnaamt = "-" + txnaamt;
                }

                frportnew.AR = txnaamt;
                frportnew.AP = "0";
                frportnew.AW = "0";
                frportnew.CB = Convert.ToString(wbal);
                frportnew.OCPL = "0";
                frportnew.MCCB = "0";
                frportnew.MCC = "0";
                frportnew.MCD = "0";
                frportnew.MP = "0";
                frportnew.MW = "0";
                frportnew.MCPL = "0";

                log.FRENCHISEREPORTs.Add(frportnew);

            }
            else
            {
                int cb = Convert.ToInt32(frport.CB);
                int ar = Convert.ToInt32(frport.AR);
                if (tt == "1")
                {
                    cb = cb - Convert.ToInt32(txnaamt);
                    ar = ar - Convert.ToInt32(txnaamt);
                    txnaamt = "-" + txnaamt;
                    frport.AR = Convert.ToString(ar);
                    frport.CB = Convert.ToString(cb);
                }
                else
                {
                    cb = cb + Convert.ToInt32(txnaamt);
                    ar = ar + Convert.ToInt32(txnaamt);
                    frport.AR = Convert.ToString(ar);
                    frport.CB = Convert.ToString(cb);
                }
            }
            log.SaveChanges();

            return Json(ca);
        }

        public ActionResult VIEWFRENCHISEBAL()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }
            pm_admindbEntities1 log = new pm_admindbEntities1();
            List<FRENCHISEBAL> frelist = log.FRENCHISEBALs.ToList();
            return View(frelist);
        }

        [HttpPost]
        public JsonResult creategamepool(string todaydate)
        {
            GAMEPOOL gpc = new GAMEPOOL();
            TOTALSALE tsc = new TOTALSALE();
            string MSG = "";
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var userdt = log.GAMEPOOLs.Where(x => x.GPDATE == todaydate).FirstOrDefault();
            var userdt2 = log.TOTALSALEs.Where(x => x.GPDATE == todaydate).FirstOrDefault();

            if(userdt != null)
            {
                MSG = "GAMEPOOLS ARE ALREADY CREATED !";
            }
            else
            {
                var gslastid = log.GAMESLOTs.OrderByDescending(p => p.GSID).FirstOrDefault();
                var gslid = gslastid.GSID;
                gslid = gslid + 1;
                for (int i = 1; i < gslid; i++)
                {
                    var gsdata = log.GAMESLOTs.Where(x => x.GSID == i).FirstOrDefault();
                    gpc.GSID = gsdata.GSID;
                    gpc.GSTIME = gsdata.GSTIME;
                    gpc.GSENDTIME = gsdata.GSENDTIME;
                    gpc.GPDATE = todaydate;
                    log.GAMEPOOLs.Add(gpc);
                    log.SaveChanges();
                }
                MSG = "GAMEPOOLS ARE CREATED SUCCESFULLY !";
            }

            if (userdt2 != null)
            {
                MSG = "GAMEPOOLS ARE ALREADY CREATED !";
            }
            else
            {
                var gslastid = log.GAMESLOTs.OrderByDescending(p => p.GSID).FirstOrDefault();
                var gslid = gslastid.GSID;
                gslid = gslid + 1;
                for (int i = 1; i < gslid; i++)
                {
                    var gpcdata = log.GAMEPOOLs.Where(x => x.GSID == i && x.GPDATE == todaydate).FirstOrDefault();
                    tsc.GSID = gpcdata.GSID;
                    tsc.GPID = gpcdata.GPID;
                    tsc.GPSTIME = gpcdata.GSTIME;
                    tsc.GPSENDTIME = gpcdata.GSENDTIME;
                    tsc.GPDATE = todaydate;
                    tsc.Y1 = "0";
                    tsc.Y10 = "0";
                    tsc.Y2 = "0";
                    tsc.Y3 = "0";
                    tsc.Y4 = "0";
                    tsc.Y5 = "0";
                    tsc.Y6 = "0";
                    tsc.Y7 = "0";
                    tsc.Y8 = "0";
                    tsc.Y9 = "0";
                    log.TOTALSALEs.Add(tsc);
                    log.SaveChanges();
                }
                MSG = "GAMEPOOLS ARE CREATED SUCCESFULLY !";
            }

            return Json(MSG);
        }

        public ActionResult PROGRAM()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public JsonResult programtime(string todaytimenow, string todaydatenow)
        {
            GAMESLOT gsc1 = new GAMESLOT();
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var timesstring = todaytimenow;
            var timehr = timesstring.Substring(0, 2);
            var timemin = timesstring.Substring(3, 2);
            var ampm = timesstring.Substring(9, 2);
            var ampm2 = timesstring.Substring(9, 2);
            var timehrint = Convert.ToInt32(timehr);
            var timeminint = Convert.ToInt32(timemin);

            if (timeminint == 00 || timeminint > 00 && timeminint < 15)
            {
                timeminint = 15;
            }
            else if (timeminint == 15 || timeminint > 15 && timeminint < 30)
            {
                timeminint = 30;
            }
            else if (timeminint == 30 || timeminint > 30 && timeminint < 45)
            {
                timeminint = 45;
            }
            else if (timeminint == 45 || timeminint > 45)
            {
                timeminint = 00;
                timehrint = timehrint + 1;
            }

            int TIME24;

            if (ampm == "AM")
            {
                TIME24 = timehrint + 1;
                
                if(TIME24 == 13)
                {
                    if (timehrint == 12)
                    {
                        timehrint = 12;
                    }
                    else
                    {
                        timehrint = timehrint - 12;
                    }
                    if (ampm == "PM")
                    {
                        ampm = "AM";
                    }
                    else
                    {
                        ampm = "PM";
                    }
                }
            }
            else
            {
                if (timehrint == 13)
                {
                    timehrint = timehrint - 12;
                }
                ampm = "PM";
            }



            timemin = Convert.ToString(timeminint);
            if (1 == timemin.Length)
            {
                timemin = "0" + timemin;
            }
            timehr = Convert.ToString(timehrint);
            if (1 == timehr.Length)
            {
                timehr = "0" + timehr;
            }
            var str1 = timehr + ":" + timemin + " " + ampm;
            timehr = timehr + ":" + timemin + ":00 " + ampm;

            var userdt = log.GAMESLOTs.Where(x => x.GSENDTIME == timehr).FirstOrDefault();
            var tsc = log.TOTALSALEs.Where(x => x.GPSENDTIME == timehr && x.GPDATE == todaydatenow).FirstOrDefault();
            gsc1.GSENDTIME = str1;

            if (tsc != null)
            {
                var gpid = tsc.GPID;
                gpid = gpid - 1;
                var pgc = log.GAMEPOOLs.Where(x => x.GPID == gpid).FirstOrDefault();
                var yid = Convert.ToString(pgc.YID);
                var jck = pgc.JACKPOT;

                string strviewbag = "";
                if (yid == "1")
                {
                    strviewbag = "SHREE";
                }
                if (yid == "2")
                {
                    strviewbag = "Vashikaran";
                }
                if (yid == "3")
                {
                    strviewbag = "Sudarshan";
                }
                if (yid == "4")
                {
                    strviewbag = "Vastu";
                }
                if (yid == "5")
                {
                    strviewbag = "Planet";
                }
                if (yid == "6")
                {
                    strviewbag = "Love";
                }
                if (yid == "7")
                {
                    strviewbag = "Tara";
                }
                if (yid == "8")
                {
                    strviewbag = "Grah";
                }
                if (yid == "9")
                {
                    strviewbag = "Matsya";
                }
                if (yid == "10")
                {
                    strviewbag = "Meditation";
                }
                if (jck == "2")
                {
                    strviewbag = strviewbag + " With Jackpot";
                }

                tsc.GPDATE = strviewbag +" at "+pgc.GSENDTIME;


                tsc.GPSTIME = tsc.GPSENDTIME.Substring(0, 5) + " " + tsc.GPSENDTIME.Substring(9, 2);
                gsc1.GSTIME = userdt.GSENDTIME.Substring(0, 5) + " " + userdt.GSENDTIME.Substring(9, 2);
                tsc.GPSENDTIME = str1;

                return Json(tsc);
            }
            else
            {
                TOTALSALE tsc1 = new TOTALSALE();
                tsc1.GPSTIME = timehr;
                gsc1.GSTIME = timehr;
                tsc1.GPSENDTIME = str1;

                return Json(tsc1);
            }
        }

        [HttpPost]
        public JsonResult openresult(string yid, string jck, string gpsendtimevr, string datetxt, string timerval)
        {
            int i = timerval.Length;
            var y = "";
            if (i == 5)
            {
                y = timerval.Substring(0, 2);
                timerval = timerval.Substring(3);
                i = Convert.ToInt32(timerval);
            }

            if (i <= 10 && y=="00")
            {
                return Json("have na khule");
            }
            else
            {
                pm_admindbEntities1 log = new pm_admindbEntities1();
                gpsendtimevr = gpsendtimevr.Substring(0, 5) + ":00" + gpsendtimevr.Substring(5);
                var gpcdata = log.GAMEPOOLs.Where(x => x.GSENDTIME == gpsendtimevr && x.GPDATE == datetxt).FirstOrDefault();

                if (gpcdata != null)
                {
                    if (gpcdata.GPRESULT == null)
                    {                        
                        gpcdata.GPRESULT = yid;
                        gpcdata.YID = Convert.ToInt32(yid);
                        gpcdata.JACKPOT = jck;
                        log.SaveChanges();
                        return Json("Opened Successfully");
                    }
                    else
                    {
                        return Json("Alredy Declared Successfully");
                    }
                }
                else
                {
                    return Json("GAME IS NOT ONLINE");
                }
            }

        }

        [HttpPost]
        public JsonResult autoopenresult(string yid, string jck, string gpsendtimevr, string datetxt, string timerval)
        {
            int i = timerval.Length;
            var y = "";
            if (i == 5)
            {
                y = timerval.Substring(0, 2);
                timerval = timerval.Substring(3);
                i = Convert.ToInt32(timerval);
            }

            pm_admindbEntities1 log = new pm_admindbEntities1();
            gpsendtimevr = gpsendtimevr.Substring(0, 5) + ":00" + gpsendtimevr.Substring(5);
            var gpcdata = log.GAMEPOOLs.Where(x => x.GSENDTIME == gpsendtimevr && x.GPDATE == datetxt).FirstOrDefault();

            if (gpcdata != null)
            {
                if (gpcdata.GPRESULT == null)
                {
                    gpcdata.GPRESULT = yid;
                    gpcdata.YID = Convert.ToInt32(yid);
                    gpcdata.JACKPOT = jck;
                    log.SaveChanges();
                    return Json("Opened Successfully");
                }
                else
                {
                    return Json("Alredy Declared Successfully");
                }
            }
            else
            {
                return Json("GAME IS NOT ONLINE");
            }

        }

    }
}
