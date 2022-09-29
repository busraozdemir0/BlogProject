using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AboutController : Controller
    {
        AboutManager aboutManager = new AboutManager(new EfAboutRepository());
        private readonly IWebHostEnvironment _webHost;
        public AboutController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }
        public IActionResult Index()
        {
            Context context = new Context();
            var aboutList = context.Abouts.FirstOrDefault();
            return View(aboutList);
        }
        [HttpGet]
        public IActionResult AboutUpdate(int id)
        {
            var aboutId = aboutManager.TGetById(id);
            return View(aboutId);
        }
        [HttpPost]
        public async Task<IActionResult> AboutUpdate(About about)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHost.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(about.AboutImage.FileName);
                string extension = Path.GetExtension(about.AboutImage.FileName);
                about.AboutImageYol = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/AboutImageFiles/", filename);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await about.AboutImage.CopyToAsync(filestream);
                }
                aboutManager.TUpdate(about);
                return RedirectToAction("AboutUpdate", "About");
            }
            return View();
        }
    }
}
