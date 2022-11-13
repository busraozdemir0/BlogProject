using CoreDemo.Models;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using EntityLayer.Enums;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MimeKit;
using MimeKit.Text;
using System;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CoreDemo.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(UserSignInViewModel p)
        {
            Context context = new Context();
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(p.username, p.password, false, true);
                if (result.Succeeded)
                {
                    var name = context.Users.Where(x => x.UserName == p.username).Select(y => y.NameSurname).FirstOrDefault();
                    var userId = context.Users.Where(x => x.NameSurname == name).Select(y => y.Id).FirstOrDefault();

                    var userRoleId = context.UserRoles.Where(x => x.UserId == userId).Select(y => y.RoleId).FirstOrDefault();
                    var roleType = context.Roles.Where(x => x.Id == userRoleId).Select(y => y.RolType).FirstOrDefault();

                    if (roleType == (int)UserRoleTypeEnum.Admin)
                    {
                        return RedirectToRoute(new { action = "Index", controller = "Widget", area = "Admin" });
                    }
                    else if (roleType == (int)UserRoleTypeEnum.Yazar)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else if (roleType == (int)UserRoleTypeEnum.Üye)
                    {
                        return RedirectToAction("Index", "Blog");
                    }

                }
                else
                {
                    return RedirectToAction("Index", "Dashboard");
                    //return RedirectToAction("Index", "Login");
                }
            }
            return View();
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SifremiUnuttum()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SifremiUnuttum(SifremiUnuttumDto dto)
        {
            using (var context = new Context())
            {
                var kisiMail = context.Users.Where(x => x.Email == dto.Mail).Select(y => y.Email).FirstOrDefault().ToString();
                HttpContext.Session.SetString("kisiMail", kisiMail);
                if (kisiMail != null)
                {
                    Random random = new Random();
                    var sayi = random.Next(1000, 9999).ToString();
                    HttpContext.Session.SetString("OnayKodu", sayi);
                    var eMail = new MimeMessage();
                    eMail.From.Add(MailboxAddress.Parse("uiswagger@gmail.com"));
                    eMail.To.Add(MailboxAddress.Parse(kisiMail));
                    eMail.Subject = "Onay Kodu";
                    eMail.Body = new TextPart(TextFormat.Plain) { Text = sayi };

                    //send eMail
                    using var smtp = new SmtpClient();

                    smtp.ServerCertificateValidationCallback = ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true; //ssl sertifika doğrulama hatasını görmezden gelmesi için

                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("uiswagger@gmail.com", "ttwlzodhwqhpbpjy");
                    smtp.Send(eMail);
                    smtp.Disconnect(true);

                    return RedirectToAction("OnayKodu", "Login");
                }
                else
                {
                    return View();
                }
            }

        }
       
        public IActionResult OnayKodu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OnayKodu(SifremiUnuttumDto onay)
        {
            var kod = HttpContext.Session.GetString("OnayKodu");
            if (kod == onay.OnayKodu)
            {
                return RedirectToAction("YeniSifre", "Login");
            }
            else
            {
                ViewBag.hata = "Onay Kodu Hatalı";
            }
            return View();

        }
        public IActionResult YeniSifre()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> YeniSifre(SifremiUnuttumDto yeniSifre)
        {
            if (yeniSifre.Sifre == yeniSifre.SifreYeniden)
            {
                Context context = new Context();
                var kisimail = HttpContext.Session.GetString("kisiMail");
                AppUser kisi = await _userManager.FindByEmailAsync(kisimail);
                kisi.PasswordHash = _userManager.PasswordHasher.HashPassword(kisi, yeniSifre.Sifre);
                IdentityResult result = await _userManager.UpdateAsync(kisi);
                return RedirectToRoute(new { action = "Index", controller = "Login" });
            }
            else
            {
                ViewBag.hata = "Şifreler Uyuşmuyor!";
                return View();
            }
        }
    }
}
