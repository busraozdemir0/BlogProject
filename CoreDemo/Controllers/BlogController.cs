using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        Context c = new Context();
        private readonly IWebHostEnvironment _webHost;
        public BlogController(IWebHostEnvironment webHost)
        {
            _webHost = webHost;
        }

        public IActionResult Index()
        {
            var values = bm.GetBlogListWithCategory();
            return View(values);
        }
        // [AllowAnonymous]
        public IActionResult BlogReadAll(int id)
        {
            ViewBag.id = id; //İD'leri gönderebilmek için
            var values = bm.GetBlogByID(id);
            return View(values);
        }
        public IActionResult BlogListByWriter()
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var writerID = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            var values = bm.GetListWithCategoryByWriterBm(writerID);
            return View(values);
        }
        [HttpGet]
        public IActionResult BlogAdd()
        {
            //DropDownList ile kategorileri listeleyip bu listeden seçebilmek için

            List<SelectListItem> categoryvalues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }
                                                 ).ToList();
            ViewBag.list = categoryvalues;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> BlogAdd(Blog p)
        {
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userid = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            //var writerID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault();
            BlogValidator bv = new BlogValidator();
            ValidationResult results = bv.Validate(p);
            if (results.IsValid)
            {
                p.BlogStatus = true;
                //p.BlogCreateDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                p.AppUserId = userid;

                string wwwRootPath = _webHost.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(p.BlogImage.FileName);
                string extension = Path.GetExtension(p.BlogImage.FileName);
                p.BlogImageYol = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/BlogImageFiles/", filename);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await p.BlogImage.CopyToAsync(filestream);
                }
                bm.TAdd(p);
                return RedirectToAction("BlogListByWriter", "Blog");
            }
            else
            {
                foreach (var item in results.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);

                }
            }
            return View();
        }
        public IActionResult DeleteBlog(int id)
        {
            var blogvalue = bm.TGetById(id);
            bm.TDelete(blogvalue);
            return RedirectToAction("BlogListByWriter");
        }
        [HttpGet]
        public IActionResult EditBlog(int id)
        {
            var blogvalue = bm.TGetById(id);
            List<SelectListItem> categoryvalues = (from x in cm.GetList()
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryID.ToString()
                                                   }
                                                 ).ToList();
            ViewBag.list = categoryvalues;
            return View(blogvalue);
        }
        [HttpPost]
        public async Task<IActionResult> EditBlog(Blog p)
        { 
            var userName = User.Identity.Name;
            var userMail = c.Users.Where(x => x.UserName == userName).Select(y => y.Email).FirstOrDefault();
            var userid = c.Users.Where(x => x.UserName == userName).Select(y => y.Id).FirstOrDefault();
            //var writerID = c.Writers.Where(x => x.WriterMail == userMail).Select(y => y.WriterID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                p.AppUserId = userid;
                //p.BlogCreateDate = DateTime.Parse(DateTime.Now.ToShortDateString()); 
                p.BlogStatus = true;
                string wwwRootPath = _webHost.WebRootPath;
                string filename = Path.GetFileNameWithoutExtension(p.BlogImage.FileName);
                string extension = Path.GetExtension(p.BlogImage.FileName);
                p.BlogImageYol = filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/BlogImageFiles/", filename);
                using (var filestream = new FileStream(path, FileMode.Create))
                {
                    await p.BlogImage.CopyToAsync(filestream);
                }
                bm.TUpdate(p);
            }
            return RedirectToAction("BlogListByWriter");
        }

    }
}
