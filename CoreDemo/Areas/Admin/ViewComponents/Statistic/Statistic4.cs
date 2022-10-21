using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic4:ViewComponent
    {
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            ViewBag.ad = c.Users.Where(x => x.Id==userId).Select(y => y.NameSurname).FirstOrDefault();
            ViewBag.v2 = c.Users.Where(x => x.Id == userId).Select(y => y.ImagePath).FirstOrDefault();
            ViewBag.v3 = c.Users.Where(x => x.Id == userId).Select(z => z.UserAbout).FirstOrDefault();
            ViewBag.mail=c.Users.Where(x => x.Id == userId).Select(z => z.Email).FirstOrDefault();
            ViewBag.adres = c.Addresses.Select(y => y.Adres).FirstOrDefault();
            ViewBag.telefon = c.Addresses.Select(y => y.TelNo).FirstOrDefault();
            ViewBag.blogBegeni= c.Blogs.OrderByDescending(y=>y.BlogID).Take(1).Select(x => x.Begeni_Sayisi).FirstOrDefault();
            return View();
        }
    }
}
