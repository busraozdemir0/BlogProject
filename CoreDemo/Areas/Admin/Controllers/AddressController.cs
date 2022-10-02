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

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AddressController : Controller
    {        
        AddressManager addressManager = new AddressManager(new EfAddressRepository());
        public IActionResult Index()
        {
            Context context = new Context();
            var adres = context.Addresses.FirstOrDefault();
            return View(adres);
        }
        [HttpGet]
        public IActionResult AddressUpdate(int id)
        {
            var adresId = addressManager.TGetById(id);
            return View(adresId);
        }
        [HttpPost]
        public IActionResult AddressUpdate(Address address)
        {
            addressManager.TUpdate(address);
            return RedirectToAction("Index", "Address");
        }
    }
}
