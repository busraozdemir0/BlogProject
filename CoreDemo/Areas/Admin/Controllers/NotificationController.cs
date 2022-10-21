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
    public class NotificationController : Controller
    {
        NotificationManager notificationManager = new NotificationManager(new EfNotificationRepository());
        public IActionResult Index()
        {
            var bildirimList = notificationManager.GetList();
            return View(bildirimList);
        }
        [HttpGet]
        public IActionResult NotificationAdd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NotificationAdd(Notification notification)
        {
            notification.NotificationDate= Convert.ToDateTime(DateTime.Now.ToShortDateString());
            notificationManager.TAdd(notification);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult NotificationUpdate(int id)
        {
            var bildirimId = notificationManager.TGetById(id);
            return View(bildirimId);
        }
        [HttpPost]
        public IActionResult NotificationUpdate(Notification notification)
        {
            notification.NotificationDate = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            notificationManager.TUpdate(notification);
            return RedirectToAction("Index");
        }
        public IActionResult NotificationDelete(int id)
        {
            var bildirimId = notificationManager.TGetById(id);
            notificationManager.TDelete(bildirimId);
            return RedirectToAction("Index");
        }
        public IActionResult NotificationActive(int id)
        {
            Context context = new Context();
            var bildirimId = context.Notifications.Find(id);
            bildirimId.NotificationStatus = true;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult NotificationPassive(int id)
        {
            Context context = new Context();
            var bildirimId = context.Notifications.Find(id);
            bildirimId.NotificationStatus = false;
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
