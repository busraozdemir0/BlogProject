
using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Blog
{
    public class WriterLastBlog:ViewComponent
    {
        BlogManager bm = new BlogManager(new EfBlogRepository());
        public IViewComponentResult Invoke()
        {
            Context context = new Context();
            var user = User.Identity.Name;
            var userId = context.Users.Where(x => x.UserName == user).Select(y => y.Id).FirstOrDefault();
            var values = bm.GetBlogListByWriter(userId);
            return View(values);
        }
    }
}
