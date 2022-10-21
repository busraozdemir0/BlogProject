using BusinessLayer.Concrete;
using CoreDemo.Areas.Admin.Models;
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
            p.SenderUserId = userId;
            p.ReceiverUserId = c.Users.Where(x => x.Email == p.ReceiverUserEmail).Select(y => y.Id).FirstOrDefault();
            p.ReceiverUserEmail= c.Users.Where(x => x.Email == p.ReceiverUserEmail).Select(y => y.Email).FirstOrDefault();
            p.MessageDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            p.MessageStatus = true;
            mm.TAdd(p);
            return RedirectToAction("SendBox"); ;
        }
        public IActionResult MessageDetail(int id)
        {
            var mesajSayisi = c.Contacts.Count().ToString();
            ViewBag.gelenMesajSayisi = mesajSayisi;

            var gidenMesajSayisi = c.Message2s.Count().ToString();
            ViewBag.gidenMesajSayisi = gidenMesajSayisi;

            var mesaj = c.Contacts.Find(id);
            return View(mesaj);
        }
        public IActionResult SendMessageDetail(int id)
        {
            var mesajSayisi = c.Contacts.Count().ToString();
            ViewBag.gelenMesajSayisi = mesajSayisi;

            var gidenMesajSayisi = c.Message2s.Count().ToString();
            ViewBag.gidenMesajSayisi = gidenMesajSayisi;

            var mesaj = c.Message2s.Find(id);
            return View(mesaj);
        }
        [HttpPost]
        public IActionResult ContactMessageDelete(List<MesajlarDto> mesaj)
        {
            Context context = new Context();
            List<MesajlarDto> silinecekMesajlar = mesaj.Where(x => x.Status == true).ToList();
            foreach(var item in silinecekMesajlar)
            {
                var id = item.CheckboxId;
                var silinecekMesaj = c.Contacts.Find(id);

                context.Contacts.Remove(silinecekMesaj);
                context.SaveChanges();
            }
            return Ok("Başarılı");        
        }
        [HttpPost]
        public IActionResult MessageDelete(List<MesajlarDto> mesaj)
        {
            Context context = new Context();
            List<MesajlarDto> silinecekMesajlar = mesaj.Where(x => x.Status == true).ToList();
            foreach (var item in silinecekMesajlar)
            {
                var id = item.CheckboxId;
                var silinecekMesaj = c.Message2s.Find(id);

                mm.TDelete(silinecekMesaj);
            }
            return Ok("Başarılı");

        }
    }
}
