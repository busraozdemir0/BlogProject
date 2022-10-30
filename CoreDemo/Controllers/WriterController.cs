using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using CoreDemo.Models;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [Authorize(Roles = "Admin,Yazar")]
    public class WriterController : Controller
    {
        UserManager userManager = new UserManager(new EfUserRepository());
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHost;

        public WriterController(IWebHostEnvironment webHost, UserManager<AppUser> userManager)
        {
            _webHost = webHost;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            Context c = new Context();
            var userName = User.Identity.Name;
            var nameSurname = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            ViewBag.isim = nameSurname;
            return View();
        }
        public IActionResult WriterProfile()
        {
            return View();
        }
        public IActionResult WriterMail()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult WriterNavbarPartial(UserAddViewModel model)
        {
            Context c = new Context();
            var userName = User.Identity.Name;
            var userId = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var nameSurname = c.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imagePath = c.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();
            ViewBag.isim = nameSurname;
            ViewBag.yol = imagePath;
            model.image_yol = imagePath;
            model.namesurname = nameSurname;
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult WriterFooterPartial()
        {
            return PartialView();
        }
        [HttpGet]
        public IActionResult WriterEditProfile()
        {
            Context context = new Context();
            var username = User.Identity.Name;
            var userid = context.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
            var userName = context.Users.Where(x => x.UserName == username).Select(y => y.UserName).FirstOrDefault();
            var name = context.Users.Where(x => x.UserName == username).Select(y => y.NameSurname).FirstOrDefault();
            var sifre = context.Users.Where(x => x.UserName == username).Select(y => y.PasswordHash).FirstOrDefault();
            var email = context.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var imageyol = context.Users.Where(x => x.UserName == username).Select(y => y.ImagePath).FirstOrDefault();
            var wabout = context.Users.Where(x => x.UserName == username).Select(y => y.UserAbout).FirstOrDefault();


            UserUpdateViewModel model = new UserUpdateViewModel();
            model.userId = userid;
            model.namesurname = name;
            model.username = userName;
            model.mail = email;
            model.password = sifre;
            model.image_yol = imageyol;
            model.about = wabout;

            ViewBag.adsoyad = name;
            ViewBag.isim = userName;
            ViewBag.yol = imageyol;

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> WriterEditProfile(UserUpdateViewModel model)
        {
            Context context = new Context();
            var username = User.Identity.Name;
            var userid = context.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
            model.userId = userid;
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHost.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(model.image.FileName);
                string extension = Path.GetExtension(model.image.FileName);
                model.image_yol = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/WriterImageFiles/", filename);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await model.image.CopyToAsync(filestream);
                }
                AppUser appUser = new AppUser
                {
                    Id = model.userId,
                    Image = model.image,
                    ImagePath = model.image_yol,
                    UserAbout=model.about,
                    PasswordHash=model.password
                    
                };

                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                user.NameSurname = model.namesurname;
                user.UserName = model.username;
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.password);
                user.Email = model.mail;
                user.UserAbout = model.about;
                user.ImagePath = model.image_yol;
                IdentityResult result = await _userManager.UpdateAsync(user);              
              
            }

            return RedirectToAction("Index", "Dashboard");
        }       
        public IActionResult Arama(string q)
        {
            Context context = new Context();
            var userName = User.Identity.Name;
            var userId = context.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var nameSurname = context.Users.Where(x => x.UserName == userName).Select(y => y.NameSurname).FirstOrDefault();
            var imagePath = context.Users.Where(x => x.UserName == userName).Select(y => y.ImagePath).FirstOrDefault();
            ViewBag.adsoyad = nameSurname;
            ViewBag.yol = imagePath;
            ViewBag.isim = userName;
            var viewModel = new AramaModel();
            viewModel.AramaKey = q;

            if (!string.IsNullOrEmpty(q))
            {
                var blog = context.Blogs.Where(x => x.BlogTitle!.Contains(q)).ToList();
                var gelenkutusu = context.Message2s.Where(x => x.MessageDetails!.Contains(q)).ToList();
                var bildirim = context.Notifications.Where(x => x.NotificationTitle!.Contains(q)).ToList();
                var kategori = context.Categories.Where(x => x.CategoryName!.Contains(q)).ToList();

                viewModel.Blogs = blog;
                viewModel.Message2s = gelenkutusu;
                viewModel.Notifications = bildirim;
                viewModel.Categories = kategori;

            }

            return View(viewModel);
        }

    }
}
