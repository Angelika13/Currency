using Currency.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Currency.Controllers
{
    public class HomeController : Controller
    {
        EFDbContext context = new EFDbContext();
        WebClient client = new WebClient();

        public Valuta GetValuteByCode(string valutaCode)
        {
            //функция загружает нужную валюту с сайта 
            try
            {
                var xml = client.DownloadString("http://www.cbr.ru/scripts/XML_valFull.asp");
                XDocument xdoc = XDocument.Parse(xml);
                var el = xdoc.Element("Valuta").Elements("Item").Where(x => x.Attribute("ID").Value == valutaCode);
                Valuta valuta = context.Valutas.Where(v => v.Code == valutaCode).First();
                if (valuta == null)
                {
                    valuta = new Valuta
                    {
                        Code = valutaCode,
                        Name = el.Elements("Name").FirstOrDefault().Value,
                        EngName = el.Elements("EngName").FirstOrDefault().Value,
                        Nominal = Convert.ToInt32(el.Elements("Nominal").FirstOrDefault().Value),
                        ISO_Num_Code = Convert.ToInt32(el.Elements("ISO_Num_Code").FirstOrDefault().Value),
                        ISO_Char_Code = el.Elements("ISO_Char_Code").FirstOrDefault().Value
                    };
                    context.Entry(valuta).State = EntityState.Added;
                    context.SaveChanges();
                }
                return valuta;
            }
            catch
            {
                return null;
            }
        }

        public ValCurs LoadCursFromCbrByDate(DateTime dateTime, string valutaCode)
        {
            try
            {
                var xml = client.DownloadString("http://www.cbr.ru/scripts/XML_daily.asp?date_req=" + dateTime.ToString("dd.MM.yyyy"));
                XDocument xdoc = XDocument.Parse(xml);
                var el = xdoc.Element("ValCurs").Elements("Valute").Where(x => x.Attribute("ID").Value == valutaCode);
                Valuta valuta = GetValuteByCode(valutaCode);
                if (valuta != null)
                {
                    ValCurs valCurs = new ValCurs
                    {
                        CursDate = dateTime,
                        Value = Convert.ToDecimal(el.Elements("Value").FirstOrDefault().Value),
                        ValutaId = valuta.Id
                    };
                    context.Entry(valCurs).State = EntityState.Added;
                    context.SaveChanges();
                    return valCurs;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public decimal GetValCursByDate(DateTime dateTime, string valutaCode)
        {
            ValCurs valCurs = context.ValCurs.Where(v => v.CursDate == dateTime).Where(v => v.Valuta.Code == valutaCode).FirstOrDefault();
            if (valCurs == null)
            {
                valCurs = LoadCursFromCbrByDate(dateTime, valutaCode);
            }
            if (valCurs != null)
            {
                return valCurs.Value;
            }
            return 0;
        }
        public ActionResult Index()
        {
            string strCode = "R01235";
            return View(new CursShow {Curs = GetValCursByDate(DateTime.Now, strCode), Str = strCode });
        }
    }
}