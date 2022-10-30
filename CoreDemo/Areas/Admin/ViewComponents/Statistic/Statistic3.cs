using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.ViewComponents.Statistic
{
    public class Statistic3:ViewComponent
    {
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            ViewBag.kullanicis = c.Users.Count();
            ViewBag.begeni = c.Blogs.Take(1).Select(x => x.Begeni_Sayisi).FirstOrDefault();
            ViewBag.blog = c.Blogs.Count();
            ViewBag.bildirim = c.Notifications.Count();
            ViewBag.iletisim = c.Message2s.Count();
            ViewBag.abone = c.NewsLetters.Count();
           

            return View();
        }
    }
}
