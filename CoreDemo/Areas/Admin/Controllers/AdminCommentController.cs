using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminCommentController : Controller
    {
        CommentManager commentManager = new CommentManager(new EfCommentRepository());
        public IActionResult Index()
        {
            var values = commentManager.GetCommentWithBlog();
            return View(values);
        }
        public IActionResult CommentDelete(int id)
        {
            var delete = commentManager.TGetById(id);
            commentManager.TDelete(delete);
            return View("Index");
        }
        [HttpGet]
        public IActionResult CommentUpdate(int id)
        {
            var list = commentManager.TGetById(id);
            return View(list);
        }
        [HttpPost]
        public IActionResult CommentUpdate(Comment comment)
        {
            commentManager.TUpdate(comment);
            return View();
        }
    }
}
