﻿using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class NotificationController : Controller
    {
        NotificationManager nm = new NotificationManager(new EfNotificationRepository());
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult AllNotification()
        {
            Context c = new Context();
            var userName = User.Identity.Name;
            var name = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imgyol = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();

            ViewBag.adsoyad = name;
            ViewBag.isim = userName;
            ViewBag.yol = imgyol;
            var values = nm.GetList();
            return View(values);
        }
        public IActionResult NotificationDetails(int id)
        {
            Context c = new Context();
            var userName = User.Identity.Name;
            var name = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imgyol = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();

            ViewBag.adsoyad = name;
            ViewBag.isim = userName;
            ViewBag.yol = imgyol;

            var notification = nm.TGetById(id);
            return View(notification);
        }

    }
}
