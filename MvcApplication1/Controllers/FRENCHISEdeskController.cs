using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Management;
using System.Drawing.Printing;
using System.IO;
using System.Drawing;


namespace MvcApplication1.Controllers
{
    public class FRENCHISEdeskController : Controller
    {
        public ActionResult Index()
        {
            Session["USERID"] = "";
            Session["USERNAME"] = "";
            return View();
        }

        [HttpPost]
        public ActionResult Index(FRENCHISE USERLOG)
        {
            var unm = USERLOG.FUID;
            var upass = USERLOG.PASSWORD;
            using (pm_admindbEntities1 log = new pm_admindbEntities1())
            {
                var userdt = log.FRENCHISEs.Where(x => x.FUID == unm && x.PASSWORD == upass).FirstOrDefault();
                if (userdt == null)
                {
                    ViewBag.ERRORMSG = "USERNAME OR PASSWORD IS INCCORECT";
                    return View();
                }
                else
                {
                    Session["USERID"] = userdt.FUID;
                    Session["USERNAME"] = "PM" + userdt.FUID;
                    return View("DASHBOARD");
                }
            }
        }

        public ActionResult DASHBOARD()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }
            return View();
        }

        public ActionResult CRATECLIENT()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }

            var sv = Session["USERID"];

            pm_admindbEntities1 log = new pm_admindbEntities1();
            var userdt = log.FRENCHISEs.ToList();
            List<USERTBL> usrlist = log.USERTBLs.ToList();
            return View(usrlist);
        }

        [HttpPost]
        public JsonResult CRATECLIENTadd(string UUNAMEtxt, string USERNAMEtxt, string PASSWORDtxt, string CONTACTNOtxt)
        {
            USERTBL cf = new USERTBL();
            if (Session["USERID"] != null)
            {
                cf.FUID = Convert.ToString(Session["USERID"]);
            }
            cf.CONTACTNO = CONTACTNOtxt;
            cf.UUNAME = UUNAMEtxt;
            cf.PASSWORD = PASSWORDtxt;
            cf.USERNAME = USERNAMEtxt;

            pm_admindbEntities1 log = new pm_admindbEntities1();
            log.USERTBLs.Add(cf);
            log.SaveChanges();
            return Json(cf);
        }

        public ActionResult BALTRANSFERCLIENT()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }

            pm_admindbEntities1 log = new pm_admindbEntities1();
            List<FUTXN> FUTXNlist = log.FUTXNs.OrderByDescending(x => x.TXNID).ToList();
            List<USERTBL> usrlist = log.USERTBLs.ToList();
            ViewData["Usrlist"] = log.USERTBLs.ToList();

            return View(FUTXNlist);
        }

        [HttpPost]
        public JsonResult BALTRANSFERCLIENTtransfer(string uid, string tt, string txnaamt, string timetxt, string datetxt)
        {
            FUTXN ca = new FUTXN();
            if (Session["USERID"] != null)
            {
                ca.FUID = Convert.ToInt32(Session["USERID"]);
            }
            ca.UID = Convert.ToInt32(uid);
            ca.TXNDATE = datetxt;
            ca.TXNTIME = timetxt;

            if (tt == "1")
            {
                ca.DEBIT = txnaamt;
                ca.CREDIT = "0";
            }
            else
            {
                ca.CREDIT = txnaamt;
                ca.DEBIT = "0";
            }
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var usrbal = log.USERBALs.Where(x => x.UID == ca.UID).FirstOrDefault();
            if (usrbal == null)
            {
                USERBAL cfrbal = new USERBAL();
                cfrbal.UID = ca.UID;
                if (tt == "1")
                {
                    var bal = Convert.ToInt32(txnaamt);
                    var wbal = Convert.ToInt32(cfrbal.BALANCE);
                    wbal = wbal - bal;
                    cfrbal.BALANCE = Convert.ToString(wbal);
                }
                else
                {
                    var bal = Convert.ToInt32(txnaamt);
                    var wbal = Convert.ToInt32(cfrbal.BALANCE);
                    wbal = wbal + bal;
                    cfrbal.BALANCE = Convert.ToString(wbal);
                }
                log.USERBALs.Add(cfrbal);
            }
            else
            {
                if (tt == "1")
                {
                    var bal = Convert.ToInt32(txnaamt);
                    var wbal = Convert.ToInt32(usrbal.BALANCE);
                    wbal = wbal - bal;
                    usrbal.BALANCE = Convert.ToString(wbal);
                }
                else
                {
                    var bal = Convert.ToInt32(txnaamt);
                    var wbal = Convert.ToInt32(usrbal.BALANCE);
                    wbal = wbal + bal;
                    usrbal.BALANCE = Convert.ToString(wbal);
                }
            }

            log.FUTXNs.Add(ca);
            log.SaveChanges();




            return Json(ca);
        }

        public ActionResult Transactionreport()
        {
            int fuid = Convert.ToInt32(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            ViewData["frenchisereports"] = log.FRENCHISEREPORTs.Where(x => x.FUID==fuid).OrderByDescending(x => x.FRID).ToList();
            List<FRENCHISEREPORT> frrlist = log.FRENCHISEREPORTs.OrderByDescending(x => x.FRID).ToList();

            return View(frrlist);
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
        public JsonResult programtime(string todaytimenow)
        {
            GAMESLOT gsc1 = new GAMESLOT();
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var timesstring = todaytimenow;
            var timehr = timesstring.Substring(0, 2);
            var timemin = timesstring.Substring(3,2);
            var ampm = timesstring.Substring(9, 2);
            var ampm2 = timesstring.Substring(9, 2);
            var timehrint = Convert.ToInt32(timehr);
            var TIME24 = Convert.ToInt32(timehr);
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


            if (ampm == "AM")
            {
                TIME24 = timehrint + 1;
                if (TIME24 == 13)
                {
                    timehrint = timehrint - 12;

                    if (timehrint == 0)
                    {
                        timehrint = 12;
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
            gsc1.GSENDTIME = str1;

            gsc1.GSTIME = timehr;

            var frid = Convert.ToInt32(Session["USERID"]);
            var userdt2 = log.FRENCHISEBALs.Where(x => x.FUID == frid).FirstOrDefault();

            if (userdt2 != null)
            {
                gsc1.GSID = Convert.ToInt32(userdt2.BALANCE);
            }

            return Json(gsc1);
        }

        [HttpPost]
        public JsonResult purchaseprocess(string isProgramBet,string totalAmount,string multiplyAmount,string draw_time,string today_date,string time,string Shree,string Vashikaran,string Sudarshan, string Vastu,string Planet,string Love, string Tara, string Grah, string Matsya, string Meditation)
        {

            //We use the ObjectQuery to get the list of configured printers
            System.Management.ObjectQuery oquery = new System.Management.ObjectQuery("SELECT * FROM Win32_Printer");
            System.Management.ManagementObjectSearcher mosearcher = new System.Management.ManagementObjectSearcher(oquery);
            System.Management.ManagementObjectCollection moc = mosearcher.Get();

            if (Session["USERID"] == null)
            {
                string str1try = "session_close";
                return Json(str1try);
            }
            else
            {
                var getfuid = Convert.ToInt32(Session["USERID"]);
                Recipt r = new Recipt();
                pm_admindbEntities1 log = new pm_admindbEntities1();
                var newdthr = draw_time.Substring(0, 5);
                var newdtampm = draw_time.Substring(5, 3);
                draw_time = newdthr + ":00" + newdtampm;
                var gpc = log.GAMEPOOLs.Where(x => x.GSENDTIME == draw_time && x.GPDATE == today_date).FirstOrDefault();
                var fbalck = log.FRENCHISEBALs.Where(x => x.FUID == getfuid).FirstOrDefault();
                var balamt = Convert.ToUInt32(fbalck.BALANCE);
                var tickamt = Convert.ToUInt32(multiplyAmount);
                

                if (tickamt > balamt)
                {
                    string str1try = "Ticket Generated Successfully";
                    return Json(str1try);
                }
                else
                {
                    if (gpc != null)
                    {
                                               

                        FRENCHISEREPORT frm = new FRENCHISEREPORT();

                        var fr = log.FRENCHISEREPORTs.Where(x => x.FUID == getfuid && x.DATE == today_date).FirstOrDefault();

                        if (fr == null)
                        {
                            var sumfrmobcb = balamt - tickamt;
                            frm.FUID = getfuid;
                            frm.DATE = today_date;
                            frm.OB = fbalck.BALANCE;
                            frm.AR = "0";
                            frm.AP = Convert.ToString(tickamt);
                            frm.AW = "0";
                            frm.CB = Convert.ToString(sumfrmobcb);
                            frm.OCPL = "0";


                            var futxnlistc = log.FUTXNs.Where(x => x.FUID == getfuid && x.TXNDATE == today_date).ToList();
                            if (futxnlistc == null)
                            {
                                frm.MCC = "0";
                                frm.MCD = "0";
                            }
                            else
                            {
                                var sumofmcc = futxnlistc.Sum(x => Convert.ToInt32(x.CREDIT));
                                var sumofmcd = futxnlistc.Sum(x => Convert.ToInt32(x.DEBIT));
                                frm.MCC = Convert.ToString(sumofmcc);
                                frm.MCD = Convert.ToString(sumofmcd);
                            }

                            frm.MP = "0";
                            frm.MW = "0";
                            frm.MCPL = "0";
                            log.FRENCHISEREPORTs.Add(frm);
                        }
                        else
                        {
                            var apv = Convert.ToUInt32(fr.AP);
                            var awv = Convert.ToInt32(fr.AW);
                            apv = apv + tickamt;
                            int ocpl = Convert.ToInt32(apv) - awv;
                            fr.OCPL = Convert.ToString(ocpl);
                            fr.AP = Convert.ToString(apv);

                            var cbv = Convert.ToUInt32(fr.CB);
                            cbv = cbv - tickamt;
                            fr.CB = Convert.ToString(cbv);

                            var futxnlistc = log.FUTXNs.Where(x => x.FUID == getfuid && x.TXNDATE == today_date).ToList();
                            if (futxnlistc == null)
                            {
                                fr.MCC = "0";
                                fr.MCD = "0";
                            }
                            else
                            {
                                var sumofmcc = futxnlistc.Sum(x => Convert.ToInt32(x.CREDIT));
                                var sumofmcd = futxnlistc.Sum(x => Convert.ToInt32(x.DEBIT));
                                fr.MCC = Convert.ToString(sumofmcc);
                                fr.MCD = Convert.ToString(sumofmcd);
                            }
                        }

                        var tsc = log.TOTALSALEs.Where(x => x.GPSENDTIME == gpc.GSENDTIME && x.GPDATE == gpc.GPDATE).FirstOrDefault();

                        if(tsc != null)
                        {
                            int v1,v2,v3,v4,v5,v6,v7,v8,v9,v10;

                            if (Shree == "")
                            {
                                v1 = 0;
                            }
                            else
                            {
                                v1 = Convert.ToInt32(Shree);
                            }

                            if (Sudarshan == "")
                            {
                                v3 = 0;
                            }
                            else
                            {
                                v3 = Convert.ToInt32(Sudarshan);
                            }

                            if (Vastu == "")
                            {
                                v4 = 0;
                            }
                            else
                            {
                                v4 = Convert.ToInt32(Vastu);
                            }

                            if (Vashikaran == "")
                            {
                                v2 = 0;
                            }
                            else
                            {
                                v2 = Convert.ToInt32(Vashikaran);
                            }

                            if (Planet == "")
                            {
                                v5 = 0;
                            }
                            else
                            {
                                v5 = Convert.ToInt32(Planet);
                            }

                            if (Love == "")
                            {
                                v6 = 0;
                            }
                            else
                            {
                                v6 = Convert.ToInt32(Love);
                            }

                            if (Tara == "")
                            {
                                v7 = 0;
                            }
                            else
                            {
                                v7 = Convert.ToInt32(Tara);
                            }

                            if (Grah == "")
                            {
                                v8 = 0;
                            }
                            else
                            {
                                v8 = Convert.ToInt32(Grah);
                            }

                            if (Matsya == "")
                            {
                                v9 = 0;
                            }
                            else
                            {
                                v9 = Convert.ToInt32(Matsya);
                            }

                            if (Meditation == "")
                            {
                                v10 = 0;
                            }
                            else
                            {
                                v10 = Convert.ToInt32(Meditation);
                            }
                            


                            int tv1 = Convert.ToInt32(tsc.Y1);
                            int tv2 = Convert.ToInt32(tsc.Y2);
                            int tv3 = Convert.ToInt32(tsc.Y3);
                            int tv4 = Convert.ToInt32(tsc.Y4);
                            int tv5 = Convert.ToInt32(tsc.Y5);
                            int tv6 = Convert.ToInt32(tsc.Y6);
                            int tv7 = Convert.ToInt32(tsc.Y7);
                            int tv8 = Convert.ToInt32(tsc.Y8);
                            int tv9 = Convert.ToInt32(tsc.Y9);
                            int tv10 = Convert.ToInt32(tsc.Y10);

                            v1 = v1 + tv1;
                            v2 = v2 + tv2;
                            v3 = v3 + tv3;
                            v4 = v4 + tv4;
                            v5 = v5 + tv5;
                            v6 = v6 + tv6;
                            v7 = v7 + tv7;
                            v8 = v8 + tv8;
                            v9 = v9 + tv9;
                            v10 = v10 + tv10;

                            tsc.Y1 = Convert.ToString(v1);
                            tsc.Y2 = Convert.ToString(v2);
                            tsc.Y3 = Convert.ToString(v3);
                            tsc.Y4 = Convert.ToString(v4);
                            tsc.Y5 = Convert.ToString(v5);
                            tsc.Y6 = Convert.ToString(v6);
                            tsc.Y7 = Convert.ToString(v7);
                            tsc.Y8 = Convert.ToString(v8);
                            tsc.Y9 = Convert.ToString(v9);
                            tsc.Y10 = Convert.ToString(v10);

                        }
                        r.FUID = getfuid;
                        r.RTYPE = "AGENT";
                        r.Y1 = Shree;
                        r.Y2 = Vashikaran;
                        r.Y3 = Sudarshan;
                        r.Y4 = Vastu;
                        r.Y5 = Planet;
                        r.Y6 = Love;
                        r.Y7 = Tara;
                        r.Y8 = Grah;
                        r.Y9 = Matsya;
                        r.Y10 = Meditation;
                        r.GSID = Convert.ToString(gpc.GSID);
                        r.GSTIME = gpc.GSTIME;
                        r.GSDATE = gpc.GPDATE;
                        r.RGTIME = time;
                        r.RVAL = tickamt.ToString();
                        r.RWINVAL = "";
                        r.RSTATUS = "No Ticket Win";

                        var sum = balamt - tickamt;
                        fbalck.BALANCE = Convert.ToString(sum);

                        log.Recipts.Add(r);
                        log.SaveChanges();

                        var fuidvarnew = Convert.ToInt32(getfuid);
                        var newr = log.Recipts.Where(x => x.FUID == fuidvarnew && x.GSDATE == r.GSDATE && x.RGTIME == r.RGTIME).FirstOrDefault();
                        var newgs = log.GAMESLOTs.Where(x => x.GSTIME == newr.GSTIME).FirstOrDefault();
                        newr.RSTATUS = "Ticket Generated Successfully";

                        var newGSThr = newgs.GSTIME;
                        var NEWAMPM = newGSThr.Substring(9, 2);
                        newGSThr = newGSThr.Substring(0, 5);

                        var newGSThr1 = newgs.GSENDTIME;
                        var NEWAMPM1 = newGSThr1.Substring(9, 2);
                        newGSThr1 = newGSThr1.Substring(0, 5);

                        newr.GSTIME = newGSThr + " TO " + newGSThr1 + " " + NEWAMPM1;
                        newr.RGTIME = newr.GSDATE + " " + newr.RGTIME;
                        string rnmbr = Convert.ToString(newr.RID);
                        string rnmbrnew = Convert.ToString(newr.RID);
                        for (int i = 1; i <= 8; i++)
                        {
                            var crnmbr = rnmbrnew.Count();
                            if (crnmbr < i)
                            {
                                rnmbrnew = "0" + rnmbrnew;
                            }
                        }
                        newr.RWINVAL = rnmbrnew;
                        newr.GSID = totalAmount;
                        return Json(newr);

                    }
                    else
                    {
                        string str1try = "GAME IS NOT ONLINE TODAY";
                        return Json(str1try);
                    }
                }
            }            
        }

        public ActionResult purchasedetails()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }

            int fuid = Convert.ToInt32(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            ViewData["PurchaseDetails"] = log.Recipts.OrderByDescending(x => x.RID).ToList();
            List<Recipt> rlist = log.Recipts.OrderByDescending(x => x.RID).ToList();

            return View(rlist);
        }

        [HttpPost]
        public JsonResult cancelticket(string gsendtime)
        {
            if (Session["USERID"] == null)
            {
                return Json("Nihove");
            }
            string strfuid = Convert.ToString(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var fuidvarnew = Convert.ToInt32(strfuid);

            string gphr = gsendtime.Substring(0, 5);
            string gpampm = gsendtime.Substring(5, 3);
            gsendtime = gsendtime.Substring(0, 5) + ":00" + gsendtime.Substring(5, 3);

            var gsdata = log.GAMESLOTs.Where(x => x.GSENDTIME == gsendtime).FirstOrDefault();

            if (gsdata != null)
            {

                var gstime = gsdata.GSTIME;


                var newr = log.Recipts.Where(x => x.FUID == fuidvarnew && x.GSTIME == gstime).OrderByDescending(x => x.RID).FirstOrDefault();
                if (newr != null)
                {
                    if (newr.RSTATUS == "Cancelled ticket")
                    {
                        return Json("Recipt Already Cancelld !");
                    }
                    else
                    {
                        newr.RSTATUS = "Cancelled ticket";

                        //effects in TOTALSALE
                        int gsid = Convert.ToInt32(newr.GSID);
                        var tsc = log.TOTALSALEs.Where(x => x.GPDATE == newr.GSDATE && x.GSID == gsid).FirstOrDefault();
                        if (tsc != null)
                        {
                            if (newr.Y1 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y1);
                                int tv1 = Convert.ToInt32(tsc.Y1);
                                tv1 = tv1 - v1;
                                tsc.Y1 = Convert.ToString(tv1);
                            }

                            if (newr.Y2 != "")
                            {
                                int v2 = Convert.ToInt32(newr.Y2);
                                int tv2 = Convert.ToInt32(tsc.Y2);
                                tv2 = tv2 - v2;
                                tsc.Y2 = Convert.ToString(tv2);
                            }

                            if (newr.Y3 != "")
                            {
                                int v3 = Convert.ToInt32(newr.Y3);
                                int tv3 = Convert.ToInt32(tsc.Y3);
                                tv3 = tv3 - v3;
                                tsc.Y1 = Convert.ToString(tv3);
                            }

                            if (newr.Y4 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y4);
                                int tv1 = Convert.ToInt32(tsc.Y4);
                                tv1 = tv1 - v1;
                                tsc.Y4= Convert.ToString(tv1);
                            }

                            if (newr.Y5 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y5);
                                int tv1 = Convert.ToInt32(tsc.Y5);
                                tv1 = tv1 - v1;
                                tsc.Y5 = Convert.ToString(tv1);
                            }

                            if (newr.Y6 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y6);
                                int tv1 = Convert.ToInt32(tsc.Y6);
                                tv1 = tv1 - v1;
                                tsc.Y6 = Convert.ToString(tv1);
                            }

                            if (newr.Y7 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y7);
                                int tv1 = Convert.ToInt32(tsc.Y7);
                                tv1 = tv1 - v1;
                                tsc.Y7 = Convert.ToString(tv1);
                            }

                            if (newr.Y8 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y8);
                                int tv1 = Convert.ToInt32(tsc.Y8);
                                tv1 = tv1 - v1;
                                tsc.Y8 = Convert.ToString(tv1);
                            }

                            if (newr.Y9 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y9);
                                int tv1 = Convert.ToInt32(tsc.Y9);
                                tv1 = tv1 - v1;
                                tsc.Y9 = Convert.ToString(tv1);
                            }

                            if (newr.Y10 != "")
                            {
                                int v1 = Convert.ToInt32(newr.Y10);
                                int tv1 = Convert.ToInt32(tsc.Y10);
                                tv1 = tv1 - v1;
                                tsc.Y10 = Convert.ToString(tv1);
                            }
                        }

                        //effects in frenchisereport and bal
                        var fr = log.FRENCHISEREPORTs.Where(x => x.FUID == fuidvarnew && x.DATE == newr.GSDATE).FirstOrDefault();
                        var fb = log.FRENCHISEBALs.Where(x => x.FUID == fuidvarnew).FirstOrDefault();
                        int fbbal = Convert.ToInt32(fb.BALANCE);
                        int frap = Convert.ToInt32(fr.AP);
                        int frcb = Convert.ToInt32(fr.CB);
                        int tckval = Convert.ToInt32(newr.RVAL);
                        fbbal = fbbal + tckval;
                        frap = frap - tckval;
                        frcb = frcb - tckval;
                        fb.BALANCE = Convert.ToString(fbbal);
                        fr.AP = Convert.ToString(frap);
                        fr.CB = Convert.ToString(frcb);


                        log.SaveChanges();
                        return Json("Recipt Cancelld !");
                    }
                }
                else
                {
                    return Json("No Recipt To Cancel !");
                }
            }
            else
            {
                return Json("GAME IS NOT ONLINE !");
            }

        }

        [HttpPost]
        public JsonResult reprintticket(string draw_time)
        {
            if (Session["USERID"] == null)
            {
                return Json("Nihove");
            }
            
            string strfuid = Convert.ToString(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var fuidvarnew = Convert.ToInt32(strfuid);

            string gphr = draw_time.Substring(0, 5);
            string gpampm = draw_time.Substring(5, 3);
            draw_time = draw_time.Substring(0, 5) + ":00" + draw_time.Substring(5, 3);

            var gsdata = log.GAMESLOTs.Where(x => x.GSENDTIME == draw_time).FirstOrDefault();
            var crstr = "Cancelled ticket";

            if (gsdata != null)
            {
                var gstime = gsdata.GSTIME;
                var newr = log.Recipts.Where(x => x.FUID == fuidvarnew && x.GSTIME == gstime && x.RSTATUS != crstr).OrderByDescending(x => x.RID).FirstOrDefault();
                if (newr != null)
                {
                    var newgs = log.GAMESLOTs.Where(x => x.GSTIME == newr.GSTIME).FirstOrDefault();
                    newr.RSTATUS = "Ticket Generated Successfully";

                    var newGSThr = newgs.GSTIME;
                    var NEWAMPM = newGSThr.Substring(9, 2);
                    newGSThr = newGSThr.Substring(0, 5);

                    var newGSThr1 = newgs.GSENDTIME;
                    var NEWAMPM1 = newGSThr1.Substring(9, 2);
                    newGSThr1 = newGSThr1.Substring(0, 5);

                    newr.GSTIME = newGSThr + " TO " + newGSThr1 + " " + NEWAMPM1;
                    newr.RGTIME = newr.GSDATE + " " + newr.RGTIME;
                    string rnmbr = Convert.ToString(newr.RID);
                    string rnmbrnew = Convert.ToString(newr.RID);
                    for (int i = 1; i <= 8; i++)
                    {
                        var crnmbr = rnmbrnew.Count();
                        if (crnmbr < i)
                        {
                            rnmbrnew = "0" + rnmbrnew;
                        }
                    }
                    newr.RWINVAL = rnmbrnew;
                    newr.GSID = newr.RVAL;
                    return Json(newr);
                }
                else
                {
                    return Json("No Recipt To Print !");
                }
            }
            else
            {
                return Json("GAME IS NOT ONLINE TODAY");
            }
        }

        [HttpPost]
        public JsonResult getwiningyantra(string todaydatestr, string gsendtime, string yesterday)
        {
            if (Session["USERID"] == null)
            {
                return Json("Nihove");
            }
            pm_admindbEntities1 log = new pm_admindbEntities1();

            string gphr = gsendtime.Substring(0, 5);
            string gpampm = gsendtime.Substring(5, 3);
            gsendtime = gsendtime.Substring(0, 5) + ":00" + gsendtime.Substring(5, 3);
            int gsidaddvar = 0;
            if (gsendtime == "11:15:00 PM")
            {
                gsidaddvar = 1;
                gsendtime = "11:00:00 PM";
            }

            var gsdata = log.GAMESLOTs.Where(x => x.GSENDTIME == gsendtime).FirstOrDefault();
            GAMESLOT gsdata2 = new GAMESLOT();
            string returmdata="";
            string returmdata2="";
            string yname ="";
            int gsid = 7;

            if (gsdata != null)
            {
                gsid = gsid + gsdata.GSID + gsidaddvar;
                gsid = gsid - 1;
            }

            var winyantradata = log.GAMEPOOLs.Where(x => x.GPDATE == todaydatestr || x.GPDATE == yesterday && x.YID != null).OrderBy(x => x.GPID).Take(118);

            Session["gsidvar"] = Convert.ToString(gsid);
            Session["todaydatestr"] = todaydatestr;
            Session["yesterday"] = yesterday;

                winyantradata.Where(x => x.YID != null).Skip(52).Take(gsid).ToList().ForEach(x =>
                {
                    string gsendtimesend = x.GSENDTIME.Substring(0, 5) + x.GSENDTIME.Substring(8);

                    returmdata = returmdata +  "<div class=\"yantra_detail\">"+
                        "<div class=\"yantra_info\">"+
                        "<div class=\"yantra_text\">" + x.GPDATE + "<br>" + gsendtimesend + "<br>" + "</div>" + "<div class=\"yantra_image\">";

                    if(x.YID == 1 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\" style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_1.jpg\"style=\"width: 52px; height: 52px;\"  alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG1.jpg";
                        yname ="Shree";
                    }

                    if(x.YID == 2 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_2.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG2.png";
                        yname = "Vashikaran";
                    }

                    if(x.YID == 3 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_3.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG3.png";
                        yname = "Sudarshan";
                    }

                    if(x.YID == 4 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_4.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG4.png";
                        yname ="Vastu";
                    }

                    if(x.YID == 5 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_5.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG5.JPG";
                        yname ="Planet";
                    }

                    if(x.YID == 6 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_6.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG6.png";
                        yname ="Love";
                    }

                    if(x.YID == 7 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_7.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG7.png";
                        yname ="Tara";
                    }

                    if(x.YID == 8 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_8.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG8.png";
                        yname ="Grah";
                    }

                    if(x.YID == 9 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_9.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG9.png";
                        yname ="Matsya";
                    }

                    if(x.YID == 10 )
                    {
                        if (x.JACKPOT == "2")
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/jackpot.png\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        else
                        {
                            returmdata = returmdata + "<img src=\"/agent/img/yantra_img/prod_0.jpg\"style=\"width: 52px; height: 52px;\" alt=\"\">";
                        }
                        returmdata2 = "/agent/img/yantra_img/IMG0.jpg";
                        yname ="Meditation";
                    }

                    returmdata= returmdata +"</div>"+"</div>"+"<h4>"+ yname +"</h4>"+"</div>";
                
                });

                gsdata2.GSENDTIME = returmdata;
                gsdata2.GSTIME = returmdata2;

                return Json(gsdata2);
        }

        public ActionResult demo()
        {
            pm_admindbEntities1 log = new pm_admindbEntities1();
            List<FUTXN> FUTXNlist = log.FUTXNs.OrderByDescending(x => x.TXNID).ToList();
            List<USERTBL> usrlist = log.USERTBLs.ToList();
            ViewData["Usrlist"] = log.USERTBLs.ToList();

            return View(FUTXNlist);
        }

        public ActionResult demo2()
        {
            int fuid = Convert.ToInt32(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            ViewData["PurchaseDetails"] = log.Recipts.OrderByDescending(x => x.RID).ToList();
            List<Recipt> rlist = log.Recipts.OrderByDescending(x => x.RID).ToList();

            return View(rlist);
        }

        [HttpPost]
        public JsonResult winprocess(string todaydatestr, string gsendtime)
        {
            if (Session["USERID"] == null)
            {
                return Json("Nihove");
            }

            string strfuid = Convert.ToString(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var fuidvarnew = Convert.ToInt32(strfuid);

            string gphr = gsendtime.Substring(0, 5);
            string gpampm = gsendtime.Substring(5, 3);
            gsendtime = gsendtime.Substring(0, 5) + ":00" + gsendtime.Substring(5, 3);

            var gsdata = log.GAMESLOTs.Where(x => x.GSENDTIME == gsendtime).FirstOrDefault();
            var gsdata2 = gsdata;
            if (gsdata != null)
            {
                var gsid = gsdata.GSID;
                var winyantradata = log.GAMEPOOLs.Where(x => x.GPDATE == todaydatestr && x.GSID == gsid).FirstOrDefault();

                if(winyantradata.YID != null)
                {
                    var jack = winyantradata.JACKPOT;
                    if(winyantradata.YID == 1)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y1 != "" ).ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                            {
                                var y1val = Convert.ToInt32(x.Y1);
                                int rwinval;
                                if(jack == "1")
                                {
                                   rwinval = y1val * 100;
                                }
                                else
                                {
                                    rwinval = (y1val * 2) * 100;
                                }
                                x.RSTATUS = "Ticket win but not scan";
                                x.RWINVAL = Convert.ToString(rwinval);

                                if(x.UUID != null)
                                {
                                    var txt = Convert.ToDateTime(todaydatestr);
                                    var fmcdata = log.USERBALs.Where(u =>u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                    var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                    ubal = ubal + rwinval;
                                    x.RSTATUS = "Complete ticket scan";

                                    var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                    int mp = Convert.ToInt32(frdata.MP);
                                    int mw = Convert.ToInt32(frdata.MW);
                                    int mcpl = 0;
                                    mw = mw + rwinval;
                                    mcpl = mw - mp;
                                    frdata.MW = Convert.ToString(mw);
                                    frdata.MCPL = Convert.ToString(mcpl);
                                }

                            });
                        

                    }
                    else if(winyantradata.YID == 2)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y2 != "" ).ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y2);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 3)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y3 != "").ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y3);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 4)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y4 != "").ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y4);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 5)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y5 != "" ).ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y5);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 6)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y6 != "").ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y6);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 7)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y7 != "" ).ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y7);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 8)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y8 != "").ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y8);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 9)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y9 != "" ).ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y9);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }
                    else if (winyantradata.YID == 10)
                    {
                        var reciptlist = log.Recipts.Where(x => x.GSDATE == todaydatestr && x.Y10 != "").ToList();

                        reciptlist.Where(x => x.GSDATE == todaydatestr && x.GSTIME == winyantradata.GSTIME && x.RSTATUS != "Cancelled ticket" && x.GSID == Convert.ToString(winyantradata.GSID)).ToList().ForEach(x =>
                        {
                            var y1val = Convert.ToInt32(x.Y10);
                            int rwinval;
                            if (jack == "1")
                            {
                                rwinval = y1val * 100;
                            }
                            else
                            {
                                rwinval = (y1val * 2) * 100;
                            }
                            x.RSTATUS = "Ticket win but not scan";
                            x.RWINVAL = Convert.ToString(rwinval);

                            if (x.UUID != null)
                            {
                                var txt = Convert.ToDateTime(todaydatestr);
                                var fmcdata = log.USERBALs.Where(u => u.FUID == fuidvarnew && u.UID == x.UUID).FirstOrDefault();
                                var ubal = Convert.ToInt32(fmcdata.BALANCE);
                                ubal = ubal + rwinval;
                                x.RSTATUS = "Complete ticket scan";

                                var frdata = log.FRENCHISEREPORTs.Where(fr => fr.FUID == fuidvarnew && fr.DATE == todaydatestr).FirstOrDefault();
                                int mp = Convert.ToInt32(frdata.MP);
                                int mw = Convert.ToInt32(frdata.MW);
                                int mcpl = 0;
                                mw = mw + rwinval;
                                mcpl = mw - mp;
                                frdata.MW = Convert.ToString(mw);
                                frdata.MCPL = Convert.ToString(mcpl);
                            }

                        });

                    }

                    log.SaveChanges();

                    return Json(gsendtime);
                }
                return Json(gsendtime);
            }
            else
            {
                return Json(gsendtime);
            }
        }

        [HttpPost]
        public JsonResult ticketscaning(string data, string today_date)
        {
            if (Session["USERID"] == null)
            {
                return Json("Nihove");
            }
            string strfuid = Convert.ToString(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var fuidvarnew = Convert.ToInt32(strfuid);
            
            var fbalck = log.FRENCHISEBALs.Where(x => x.FUID == fuidvarnew).FirstOrDefault();
            var balamt = Convert.ToInt32(fbalck.BALANCE);

            var rid = Convert.ToInt32(data);
            rid = rid - 1;
            rid = rid + 1;

            var rdata = log.Recipts.Where(x => x.RID == rid && x.FUID == fuidvarnew).FirstOrDefault();
            if (rdata != null)
            {

                var newgs = log.GAMESLOTs.Where(x => x.GSTIME == rdata.GSTIME).FirstOrDefault();

                if (rdata.RSTATUS == "No Ticket Win")
                {
                    return Json("No Ticket Win");
                }
                else if (rdata.RSTATUS == "Ticket win but not scan")
                {

                    var gsid = Convert.ToInt32(rdata.GSID);

                    var gpdata = log.GAMEPOOLs.Where(x => x.GSID == gsid && x.GPDATE == rdata.GSDATE).FirstOrDefault();

                    balamt = balamt + Convert.ToInt32(rdata.RWINVAL);

                    FRENCHISEREPORT frm = new FRENCHISEREPORT();

                    var fr = log.FRENCHISEREPORTs.Where(x => x.FUID == fuidvarnew && x.DATE == today_date).FirstOrDefault();

                    if (fr == null)
                    {
                        var sumfrmobcb = balamt;
                        frm.FUID = fuidvarnew;
                        fbalck.BALANCE = Convert.ToString(sumfrmobcb);
                        frm.DATE = today_date;
                        frm.OB = fbalck.BALANCE;
                        frm.AR = "0";
                        frm.AP = "0";
                        frm.AW = rdata.RWINVAL;
                        frm.CB = Convert.ToString(sumfrmobcb);
                        frm.OCPL = rdata.RWINVAL;


                        var futxnlistc = log.FUTXNs.Where(x => x.FUID == fuidvarnew && x.TXNDATE == today_date).ToList();
                        if (futxnlistc == null)
                        {
                            frm.MCC = "0";
                            frm.MCD = "0";
                        }
                        else
                        {
                            var sumofmcc = futxnlistc.Sum(x => Convert.ToInt32(x.CREDIT));
                            var sumofmcd = futxnlistc.Sum(x => Convert.ToInt32(x.DEBIT));
                            frm.MCC = Convert.ToString(sumofmcc);
                            frm.MCD = Convert.ToString(sumofmcd);
                        }

                        frm.MP = "0";
                        frm.MW = "0";
                        frm.MCPL = "0";
                        rdata.RSTATUS = "Complete ticket scan";
                        log.FRENCHISEREPORTs.Add(frm);
                        log.SaveChanges();
                    }
                    else
                    {
                        var sumfrmobcb = balamt;
                        fbalck.BALANCE = Convert.ToString(sumfrmobcb);
                        var awv = Convert.ToInt32(fr.AW);
                        awv = awv + Convert.ToInt32(rdata.RWINVAL);
                        fr.AW = Convert.ToString(awv);
                        var apv = Convert.ToInt32(fr.AP);
                        int ocpl = apv - awv;
                        fr.OCPL = Convert.ToString(ocpl);

                        var cbv = Convert.ToInt32(fr.CB);
                        cbv = cbv + Convert.ToInt32(rdata.RWINVAL);
                        fr.CB = Convert.ToString(cbv);

                        var futxnlistc = log.FUTXNs.Where(x => x.FUID == fuidvarnew && x.TXNDATE == today_date).ToList();
                        if (futxnlistc == null)
                        {
                            fr.MCC = "0";
                            fr.MCD = "0";
                        }
                        else
                        {
                            var sumofmcc = futxnlistc.Sum(x => Convert.ToInt32(x.CREDIT));
                            var sumofmcd = futxnlistc.Sum(x => Convert.ToInt32(x.DEBIT));
                            fr.MCC = Convert.ToString(sumofmcc);
                            fr.MCD = Convert.ToString(sumofmcd);
                        }

                        rdata.RSTATUS = "Complete ticket scan";
                        log.SaveChanges();
                    }

                    var yval = gpdata.YID;

                    if (yval == 1)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y1) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y1;
                        }
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 2)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y2) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y2;
                        }
                        rdata.Y1 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 3)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y3) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y3;
                        }
                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 4)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y4) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y4;
                        }

                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 5)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y5) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y5;
                        }

                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 6)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y6) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y6;
                        }
                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 7)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y7) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y7;
                        }
                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 8)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y8) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y8;
                        }
                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y9 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 9)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y9) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y9;
                        }
                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y10 = "";
                    }
                    if (yval == 10)
                    {
                        if (gpdata.JACKPOT == "2")
                        {
                            var jsccount = Convert.ToInt32(rdata.Y10) * 2;
                            rdata.RVAL = Convert.ToString(jsccount);
                        }
                        else
                        {
                            rdata.RVAL = rdata.Y10;
                        }

                        rdata.Y1 = "";
                        rdata.Y2 = "";
                        rdata.Y3 = "";
                        rdata.Y4 = "";
                        rdata.Y5 = "";
                        rdata.Y6 = "";
                        rdata.Y7 = "";
                        rdata.Y8 = "";
                        rdata.Y9 = "";
                    }



                    var newGSThr = newgs.GSTIME;
                    var NEWAMPM = newGSThr.Substring(9, 2);
                    newGSThr = newGSThr.Substring(0, 5);

                    var newGSThr1 = newgs.GSENDTIME;
                    var NEWAMPM1 = newGSThr1.Substring(9, 2);
                    newGSThr1 = newGSThr1.Substring(0, 5);

                    rdata.GSTIME = newGSThr + " TO " + newGSThr1 + " " + NEWAMPM1;
                    rdata.RGTIME = rdata.GSDATE + " " + rdata.RGTIME;
                    string rnmbr = Convert.ToString(rdata.RID);
                    string rnmbrnew = Convert.ToString(rdata.RID);
                    for (int i = 1; i <= 8; i++)
                    {
                        var crnmbr = rnmbrnew.Count();
                        if (crnmbr < i)
                        {
                            rnmbrnew = "0" + rnmbrnew;
                        }
                    }
                    rdata.RWINVAL = rnmbrnew;

                    return Json(rdata);
                }
                else if (rdata.RSTATUS == "Complete ticket scan")
                {
                    return Json("Complete ticket scan");
                }
                else
                {
                    return Json("Cancelled ticket");
                }
            }
            else
            {
                return Json("empty");
            }
        }

        [HttpPost]
        public JsonResult getreportdata(string datetxt)
        {
            FRENCHISEREPORT frdt = new FRENCHISEREPORT();
            pm_admindbEntities1 log = new pm_admindbEntities1();
            string strfuid = Convert.ToString(Session["USERID"]);
            int strfuidint = Convert.ToInt32(strfuid);
            var fr = log.FRENCHISEREPORTs.Where(x => x.FUID == strfuidint && x.DATE == datetxt).FirstOrDefault();
            if (fr != null)
            {
                return Json(fr);
            }
            else
            {
                return Json(frdt);
            }
        }

        public ActionResult LUCKYDRAW()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }

            int gsid = Convert.ToInt32(Session["gsidvar"]);
            string todaydatestr = Convert.ToString(Session["todaydatestr"]);
            string yesterday = Convert.ToString(Session["yesterday"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();

            var winyantradata = log.GAMEPOOLs.Where(x => x.GPDATE == todaydatestr || x.GPDATE == yesterday && x.YID != null).OrderBy(x => x.GPID).Take(118);


            List<GAMEPOOL> gplist = winyantradata.Where(x => x.YID != null).Skip(52).Take(gsid).OrderByDescending(x => x.GPID).ToList();
            return View(gplist);
        }

        public ActionResult ChangePassword()
        {
            if (Session["USERID"] == null)
            {
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public JsonResult changepass(string passtext, string upass)
        {
            string strfuid = Convert.ToString(Session["USERID"]);
            pm_admindbEntities1 log = new pm_admindbEntities1();
            var fuidvarnew = Convert.ToInt32(strfuid);

            var userdt = log.FRENCHISEs.Where(x => x.FUID == fuidvarnew && x.PASSWORD == upass).FirstOrDefault();
            if (userdt != null)
            {
                userdt.PASSWORD = passtext;
                log.SaveChanges();
                return Json("changed");
            }
            else
            {
                return Json("notchanged");
            }
        }

    }
}
