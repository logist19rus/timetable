using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using raspWeb.Models;
using System.Text.Json;

namespace raspWeb.Controllers
{
    public class raspController : Controller
    {
        private AppDbCont db;
        public raspController(AppDbCont context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GiveMeLiveSyka(string Group, string nowDateStr)
        {
            if (HttpContext.Session.Keys.Contains("par"))
                HttpContext.Session.Remove("par");
            if (HttpContext.Session.Keys.Contains("idPara"))
                HttpContext.Session.Remove("idPara");

            DateTime nowDate = new DateTime(1111, 11, 11, 0, 0, 0);

            if (nowDateStr == null)
                nowDate = DateTime.Today;
            else
                nowDate = Convert.ToDateTime(nowDateStr);

            DateTime startWeek = nowDate.AddDays(-(int)nowDate.DayOfWeek + (int)DayOfWeek.Monday);
            HttpContext.Session.SetString("date", startWeek.ToString());

            List<EveryPara> paras = new List<EveryPara>();
            paras = await db.Paras.Where(x => x.Group.Contains(Group)).OrderBy(s => s.Day).ToListAsync();

            var tryPr = paras.Where(x => x.Day >= startWeek && x.Day <= startWeek.AddDays(6)).OrderBy(s => s.number).ToList();
            HttpContext.Session.SetString("par", JsonSerializer.Serialize<List<EveryPara>>(tryPr));
            EveryPara[,] masPar = new EveryPara[4, 7];
            foreach (var x in tryPr)
            {
                int day = (int)x.Day.DayOfWeek - 1;
                int pNum = x.number - 1;
                masPar[pNum, day] = x;
            }

            return PartialView(masPar);
        }

        [HttpPost]
        public IActionResult confirm()
        {
            List<EveryPara> pars = new List<EveryPara>();
            if (HttpContext.Session.Keys.Contains("par"))
            {
                var value = HttpContext.Session.GetString("par");
                pars = value == null ? default(List<EveryPara>) : JsonSerializer.Deserialize<List<EveryPara>>(value);
            }
            else
                return PartialView("Index", "Home");

            foreach(var s in pars)
            {
                if(s!=null)
                {
                    EveryPara ch = db.Paras.FirstOrDefault(x => x.Day.Date == s.Day.Date && x.number == s.number && s.Group == x.Group);
                    if (ch != null)
                    {
                        ch.Predmet = s.Predmet;
                        ch.Prepod = s.Prepod;
                        ch.Kabinet = s.Kabinet;
                    }
                    else
                        db.Paras.Add(s);
                }
            }
            db.SaveChanges();

            HttpContext.Session.Remove("par");
            HttpContext.Session.Remove("idPara");

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public void add(string Group, string kabinet, string prepod, string predmet, string idPara)
        {
            int dayNum = idPara[0] - '0';
            int parNum = idPara[1] - '0';
            DateTime startThisWeek = Convert.ToDateTime(HttpContext.Session.GetString("date"));

            EveryPara newPara = new EveryPara();
            newPara.Day = startThisWeek.AddDays(dayNum - 1);
            newPara.number = parNum;
            newPara.Kabinet = kabinet;
            newPara.Group = Group;
            newPara.Predmet = predmet;
            newPara.Prepod = prepod;

            List<EveryPara> pars = new List<EveryPara>();

            if (HttpContext.Session.Keys.Contains("par"))
            {
                var value = HttpContext.Session.GetString("par");
                pars = value == null ? default(List<EveryPara>) : JsonSerializer.Deserialize<List<EveryPara>>(value);
            }

            int chck = pars.FindIndex(x => x.Day.Date == newPara.Day.Date && x.number == newPara.number && x.Group == newPara.Group);
            if (chck == -1)
                pars.Add(newPara);
            else
            {
                pars[chck].Predmet = newPara.Predmet;
                pars[chck].Prepod = newPara.Prepod;
                pars[chck].Kabinet = newPara.Kabinet;
            }

            HttpContext.Session.SetString("par", JsonSerializer.Serialize<List<EveryPara>>(pars));
        }
    }
}