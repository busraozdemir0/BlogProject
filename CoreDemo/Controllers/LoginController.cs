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
        public IActionResult SifremiUnuttum(SifremiUnuttum sifre)
        {
            Context context = new Context();
            var kisiMail = context.Users.Where(x => x.Email == sifre.Mail).Select(y => y.Email).FirstOrDefault();
            var kisiId = context.Users.Where(x => x.Email == kisiMail).Select(y => y.Id).FirstOrDefault();

            var kontrol = context.SifremiUnuttums.Where(x => kisiMail.Contains(x.Mail)).FirstOrDefault();
            if (kontrol != null)
            {
                var kisi = context.SifremiUnuttums.Where(x => x.Mail == kisiMail).Select(y => y.Id).FirstOrDefault();
                var kisibul = context.SifremiUnuttums.Find(kisi);
                context.SifremiUnuttums.Remove(kisibul); //Eğer aynı kişi tekrar şifremi unuttum talebinde bulunuyorsa sifremiunuttum tablosunda maili olacağı için o kaydı silip sonraki adımlarda tekrar ekliyoruz.
                context.SaveChanges();

                Random random = new Random();
                var sayi = random.Next(1000, 9999);
                sifre.RandomKod = sayi.ToString();
                sifre.Tarih = DateTime.Now;
                sifre.AppUserId = kisiId;
                context.SifremiUnuttums.Add(sifre);
                context.SaveChanges();
            }
            else
            {
                Random random = new Random();
                var sayi = random.Next(1000, 9999);
                sifre.RandomKod = sayi.ToString();
                sifre.Tarih = DateTime.Now;
                sifre.AppUserId = kisiId;
                context.SifremiUnuttums.Add(sifre);
                context.SaveChanges();
            }

            if (kisiMail == sifre.Mail)
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("swggerx@gmail.com"));
                email.To.Add(MailboxAddress.Parse(kisiMail));
                email.Subject = "Onay Kodu";
                email.Body = new TextPart(TextFormat.Plain) { Text = sifre.RandomKod };

                //send eMail
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("swggerx@gmail.com", "tltmlfchqcjelvtx");
                smtp.Send(email);
                smtp.Disconnect(true);
            }

            SifremiUnuttumMailTut tut = new SifremiUnuttumMailTut();
            tut.Mail = sifre.Mail;
            tut.SifremiUnuttumId = sifre.Id;
            context.SifremiUnuttumMailTuts.Add(tut);
            context.SaveChanges();
            HttpContext.Session.SetString("Mail", kisiMail);
            return RedirectToAction("OnayKodu", "Login");

        }
        public IActionResult OnayKodu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult OnayKodu(OnayKoduDto onay)
        {
            Context context = new Context();
            var kod = onay.OnayKodu;
            var kisiOnayEslesme = context.SifremiUnuttums.Where(x => kod.Contains(x.RandomKod)).FirstOrDefault().ToString();
            if (true)
            {
                return RedirectToAction("YeniSifre", "Login");
            }
            else
            {
                return View();
            }

        }
        public IActionResult YeniSifre()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> YeniSifre(YeniSifreDto yeniSifre)
        {
            Context context = new Context();
            var mail = HttpContext.Session.GetString("Mail");
            var userName = context.Users.Where(x => x.Email == mail).Select(y => y.UserName).FirstOrDefault();
            AppUser kisi = await _userManager.FindByNameAsync(userName);

            if(yeniSifre.Sifre==yeniSifre.SifreYeniden)
            {
                kisi.PasswordHash = _userManager.PasswordHasher.HashPassword(kisi, yeniSifre.Sifre);
                IdentityResult result = await _userManager.UpdateAsync(kisi);
                return RedirectToRoute(new { action = "Index", controller = "Login" });
                //return RedirectToRoute(new { action = "Index", controller = "Login",area="Admin" });
            }
            else
            {
                ViewBag.HataMesaji = "Şifreler Uyuşmuyor";
                return View();
            }          
        }
    }
}
