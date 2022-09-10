using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
   
    public class WriterAboutOnDashboard:ViewComponent
    {
        Context c = new Context();

        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            ViewBag.kullanici = userName;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var userMail = c.Users.Where(x => x.Id == userId).Select(y => y.Email).FirstOrDefault();
            ViewBag.mail = userMail;
            var userAbout = c.Users.Where(x => x.Id == userId).Select(y => y.UserAbout).FirstOrDefault();
            ViewBag.about = userAbout;
            var image = c.Users.Where(x => x.Id == userId).Select(y => y.ImagePath).FirstOrDefault();
            ViewBag.resim = image;
            var ad = c.Users.Where(x => x.Id == userId).Select(y => y.NameSurname).FirstOrDefault();
            ViewBag.adi = ad;
            return View(userId);
        }

    }
}
