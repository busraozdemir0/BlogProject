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

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class CommentController : Controller
    {
        CommentManager cm = new CommentManager(new EfCommentRepository());
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult PartialAddComment()
        {
            return PartialView();
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult PartialAddComment(Comment comment)
        {

            var username = User.Identity.Name;
            Context c = new Context();
            comment.CommentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            comment.CommentStatus = true;
            comment.CommentUserName = username;

            cm.CommentAdd(comment);
            return RedirectToAction("BlogReadAll", "Blog", new { id = comment.BlogID });

        }
        //[HttpGet]
        //public PartialViewResult PartialAddComment()
        //{

        //    return PartialView();
        //}
        //[HttpPost]
        //public IActionResult PartialAddComment(Comment p)
        //{



        //    p.CommentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        //    p.CommentStatus = true;

        //    cm.CommentAdd(p);
        //    return PartialView();
        //}
        //[HttpPost]
        //public PartialViewResult PartialAddComment(Comment p)
        //{
        //    p.CommentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
        //    p.CommentStatus = true;
        //    p.BlogID = 2;
        //    cm.CommentAdd(p);
        //    return PartialView();
        //}
        public PartialViewResult CommentListByBlog(int id)
        {
            var values = cm.GetList(id);
            return PartialView(values);
        }
    }
}
