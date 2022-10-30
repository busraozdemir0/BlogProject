using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Admin
{
    public class AdminProfile : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Context context = new Context();
            var userName = User.Identity.Name;
            var adminRoleId = context.Roles.Where(x => x.Name == "Admin").Select(y => y.Id).FirstOrDefault();
            var adminKulaniciId = context.UserRoles.Where(x => x.RoleId == adminRoleId).Select(y => y.UserId).FirstOrDefault();
            var adminNameSurname = context.Users.Where(x => x.Id == adminKulaniciId).Select(y => y.NameSurname).FirstOrDefault();
            ViewBag.NameSurname = adminNameSurname;
            var adminResimYol = context.Users.Where(x => x.Id == adminKulaniciId).Select(y => y.ImagePath).FirstOrDefault();
            ViewBag.ResimYol = adminResimYol;

            return View();
        }
    }
}
