using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NewsLetterController : Controller
    {
        public IActionResult Index()
        {
            Context context = new Context();
            var list = context.NewsLetters.ToList();
            return View(list);
        }
        public IActionResult UyeSil(int id)
        {
            Context context = new Context();
            var uyeId = context.NewsLetters.Find(id);
            context.NewsLetters.Remove(uyeId);
            context.SaveChanges();
            return RedirectToAction("Index","NewsLetter");
        }
    }
}
