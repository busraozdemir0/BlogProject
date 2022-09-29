using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace CoreDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        CategoryManager cm = new CategoryManager(new EfCategoryRepository());
        public IActionResult Index(int page=1)
        {
            var values = cm.GetList().ToPagedList(page, 3);
            return View(values);
        }
        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category p)
        {          
            CategoryValidator cv = new CategoryValidator();
            ValidationResult results = cv.Validate(p);
            if (results.IsValid)
            {
                p.CategoryStatus = true;              
                cm.TAdd(p);
                return RedirectToAction("Index", "Category");
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
        [HttpGet]
        public IActionResult CategoryUpdate(int id)
        {
            var kategoriList = cm.TGetById(id);
            return View(kategoriList);
        }
        [HttpPost]
        public IActionResult CategoryUpdate(Category category)
        {
            cm.TUpdate(category);
            return RedirectToAction("Index", "Category");
        }
        public IActionResult CategoryDelete(int id)
        {
            var value = cm.TGetById(id);
            cm.TDelete(value);
            return RedirectToAction("Index");
        }
        public IActionResult CategoryActive(int id)
        {
            Context c = new Context();
            var active = c.Categories.Find(id);
            active.CategoryStatus = true;
            c.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
        public IActionResult CategoryPassive(int id)
        {
            Context c = new Context();
            var passive = c.Categories.Find(id);
            passive.CategoryStatus = false;
            c.SaveChanges();
            return RedirectToAction("Index", "Category");
        }
    }
}
