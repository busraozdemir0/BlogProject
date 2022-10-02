using CoreDemo.Areas.Admin.Models;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class SettingsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _webHost;

        public SettingsController(UserManager<AppUser> userManager, IWebHostEnvironment webHost)
        {
            _userManager = userManager;
            _webHost = webHost;
        }

        [HttpGet]
        public IActionResult AdminSettings()
        {
            Context context = new Context();
            var username = User.Identity.Name;
            var userId = context.Users.Where(x => x.UserName == username).Select(y => y.Id).FirstOrDefault();
            var userName = context.Users.Where(x => x.UserName == username).Select(y => y.UserName).FirstOrDefault();
            var name = context.Users.Where(x => x.UserName == username).Select(y => y.NameSurname).FirstOrDefault();
            var sifre = context.Users.Where(x => x.UserName == username).Select(y => y.PasswordHash).FirstOrDefault();
            var email = context.Users.Where(x => x.UserName == username).Select(y => y.Email).FirstOrDefault();
            var imageyol = context.Users.Where(x => x.UserName == username).Select(y => y.ImagePath).FirstOrDefault();
            var wabout = context.Users.Where(x => x.UserName == username).Select(y => y.UserAbout).FirstOrDefault();

            UserUpdateModel model = new UserUpdateModel();
            model.userId = userId;
            model.namesurname = name;
            model.username = userName;
            model.mail = email;
            model.password = sifre;
            model.image_yol = imageyol;
            model.about = wabout;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AdminSettings(UserUpdateModel model)
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
                    UserAbout = model.about,
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

            return RedirectToAction("AdminSettings", "Settings");
        }
    }
}
