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

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminMessageController : Controller
    {
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        Context c = new Context();
        public IActionResult Inbox()
        {
            //var userName = User.Identity.Name;
            //var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            //var userId = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();
            //var values = mm.GetInboxListByWriter(userId);
            var mesajSayisi = c.Contacts.Count().ToString();
            ViewBag.gelenMesajSayisi = mesajSayisi;

            var gidenMesajSayisi = c.Message2s.Count().ToString();
            ViewBag.gidenMesajSayisi = gidenMesajSayisi;

            var values = c.Contacts.ToList();
            return View(values);
        }
        public IActionResult SendBox()
        {
            var mesajSayisi = c.Contacts.Count().ToString();
            ViewBag.gelenMesajSayisi = mesajSayisi;

            var gidenMesajSayisi = c.Message2s.Count().ToString();
            ViewBag.gidenMesajSayisi = gidenMesajSayisi;

            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userId = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();
            var values = mm.GetSendBoxListByWriter(userId);
            return View(values);
        }
        [HttpGet]
        public IActionResult ComposeMessage()
        {
            var mesajSayisi = c.Contacts.Count().ToString();
            ViewBag.gelenMesajSayisi = mesajSayisi;

            var gidenMesajSayisi = c.Message2s.Count().ToString();
            ViewBag.gidenMesajSayisi = gidenMesajSayisi;

            return View();
        }
        [HttpPost]
        public IActionResult ComposeMessage(Message2 p)
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userId = c.Users.Where(x => x.Email == userMail).Select(y => y.Id).FirstOrDefault();
            p.SenderID = userId;
            p.ReceiverID = 2;
            p.MessageDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            p.MessageStatus = true;
            mm.TAdd(p);
            return RedirectToAction("SendBox"); ;
        }
    }
}
