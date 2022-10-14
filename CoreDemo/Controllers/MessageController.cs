
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class MessageController : Controller
    {
        
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        Context c = new Context();
        public IActionResult InBox()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userID = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();

            var name = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imgyol = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();

            ViewBag.adsoyad = name;
            ViewBag.yol = imgyol;
            var values = mm.GetInboxListByWriter(userID);
            return View(values);
        }
        public IActionResult SendBox()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userID = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();
            var name = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imgyol = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();

            ViewBag.adsoyad = name;
            ViewBag.yol = imgyol;
            var values = mm.GetSendBoxListByWriter(userID);
            return View(values);
        }
        public IActionResult MessageDetails(int id)
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userID = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();
            var name = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imgyol = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();

            ViewBag.adsoyad = name;
            ViewBag.yol = imgyol;

            var value = mm.TGetById(id);            
            return View(value);
        }
        [HttpGet]
        public IActionResult SendMessage()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userID = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();
            var name = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imgyol = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();

            ViewBag.adsoyad = name;
            ViewBag.yol = imgyol;

            return View();
        }
        [HttpPost]
        public IActionResult SendMessage(Message2 p)
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userID = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            p.SenderUserId = userID;

            p.MessageStatus = true;
            p.MessageDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            mm.TAdd(p);
            return RedirectToAction("SendBox");
        }
        public IActionResult MessageDelete(int id)
        {
            var silinecekMesaj = mm.TGetById(id);
            mm.TDelete(silinecekMesaj);
            return View();
        }
    }
}
