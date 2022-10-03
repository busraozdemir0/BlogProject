using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    public class AboutController : Controller
    {
        AboutManager aboutManager = new AboutManager(new EfAboutRepository());
        public IActionResult Index()
        {
            Context context = new Context();
            var userName = User.Identity.Name;
            var userId = context.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var adSoyad = context.Users.Where(x => x.Id == userId).Select(y => y.NameSurname).FirstOrDefault();
            ViewBag.user = adSoyad;
            var values = aboutManager.GetList();
            return View(values);
        }
        public PartialViewResult SocialMediaAbout()
        {           
            return PartialView();
        }

    }
}
