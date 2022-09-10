using BlogApiDemo.DataAccessLayer;
using BusinessLayer.Concrete;
using CoreDemo.Models;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterNavbar:ViewComponent
    {
        UserManager userManager = new UserManager(new EfUserRepository());
        Context c = new Context();
        public IViewComponentResult Invoke(UserAddViewModel model)
        {

            //var userName = User.Identity.Name;
            //var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            //var nameSurname = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            //var imagePath = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();
            //ViewBag.isim = nameSurname;
            //ViewBag.yol = imagePath;
            //model.image_yol = imagePath;
            //model.namesurname = nameSurname;
            return View();
        }
    }
}
