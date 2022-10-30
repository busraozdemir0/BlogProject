using CoreDemo.Models;
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
    public class WidgetController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Arama(string q)
        {
            Context context = new Context();
            var viewModel = new AramaModel();
            viewModel.AramaKey = q;

            if (!string.IsNullOrEmpty(q))
            {
                var blog = context.Blogs.Where(x => x.BlogTitle!.Contains(q)).ToList();
                var kategori = context.Categories.Where(x => x.CategoryName!.Contains(q)).ToList();
                var mesajgelen = context.Contacts.Where(x => x.ContactUserName!.Contains(q)).ToList();
                var mesajgiden = context.Message2s.Where(x => x.MessageDetails!.Contains(q)).ToList();
                var bildirim = context.Notifications.Where(x => x.NotificationTitle!.Contains(q)).ToList();
                var yorum = context.Comments.Where(x => x.CommentUserName!.Contains(q)).ToList();
                var üyeler = context.Users.Where(x => x.UserName!.Contains(q)).ToList();
                var aboneler = context.NewsLetters.Where(x => x.Mail!.Contains(q)).ToList();
                var roller = context.Roles.Where(x => x.Name!.Contains(q)).ToList();
                var hakkimizda = context.Abouts.Where(x => x.AboutTitle!.Contains(q)).ToList();
                var iletisim = context.Addresses.Where(x => x.AdresTitle!.Contains(q)).ToList();

                viewModel.Blogs = blog;
                viewModel.Categories = kategori;
                viewModel.Contacts = mesajgelen;
                viewModel.Message2s = mesajgiden;
                viewModel.Notifications = bildirim;
                viewModel.Comments = yorum;
                viewModel.Users = üyeler;
                viewModel.NewsLetters = aboneler;
                viewModel.Roles = roller;
                viewModel.Abouts = hakkimizda;
                viewModel.Addresses = iletisim;

            }

            return View(viewModel);
        }
    }
}
