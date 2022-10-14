using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Writer
{
    public class WriterMessageNotification:ViewComponent
    {
        Message2Manager mm = new Message2Manager(new EfMessage2Repository());
        Context c = new Context();
        public IViewComponentResult Invoke()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userID = c.Users.Where(x => x.UserName== userName).Select(y => y.Id).FirstOrDefault();
            var values = mm.GetInboxListByWriter(userID);

            var messages = c.Message2s.Where(x=>x.ReceiverID==userID).Count();
            ViewBag.mesajSayisi = messages;

            return View(values);
        }
    }
}
