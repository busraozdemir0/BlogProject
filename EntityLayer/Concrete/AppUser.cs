using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class AppUser:IdentityUser<int>
    {
        public string NameSurname { get; set; }
        public string UserAbout { get; set; }
        public string ImagePath { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [NotMapped]
        public List<Blog> Blogs { get; set; }
        public virtual ICollection<Message2> UserSender { get; set; }
        public virtual ICollection<Message2> UserReceiver { get; set; }

    }
}
