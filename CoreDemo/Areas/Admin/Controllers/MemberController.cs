using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class MemberController : Controller
    {
        private readonly RoleManager<AppRole> _rolemanager;

        public MemberController(RoleManager<AppRole> rolemanager)
        {
            _rolemanager = rolemanager;
        }
        public IActionResult Index()
        {
            Context context = new Context();
            var roleId = _rolemanager.Roles.Where(x => x.Name != "Admin").Select(y => y.Id).FirstOrDefault();
            var roleIdint = Convert.ToInt32(roleId);
            var userid = context.UserRoles.Where(x => x.RoleId == roleIdint).Select(y => y.UserId).ToList();
            var kisiler = context.Users.Where(x => userid.Contains(x.Id)).ToList();
            return View(kisiler);
        }
        public IActionResult MemberDelete(int id)
        {
            Context context = new Context();
            var userId = context.Users.Find(id);
            context.Users.Remove(userId);
            context.SaveChanges();
            return RedirectToAction("Index", "Member");
        }
    }
}
