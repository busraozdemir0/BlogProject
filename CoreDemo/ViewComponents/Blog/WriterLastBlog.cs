
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
            //adminin 3 blogunu getirme
            var rolId = context.Roles.Where(x => x.Name == "Admin").Select(y=>y.Id).FirstOrDefault();
            var userId = context.UserRoles.Where(x => x.RoleId == rolId).Select(y => y.UserId).FirstOrDefault();
            var values = bm.GetBlogListByWriter(userId);
            return View(values);
        }
    }
}
