using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.ViewComponents.Address
{
    public class AddressIletisimPage : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            Context context = new Context();
            var values = context.Addresses.FirstOrDefault();
            return View(values);
        }
    }
}
