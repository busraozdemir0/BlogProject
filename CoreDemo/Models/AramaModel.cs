using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class AramaModel
    {
        public string AramaKey { get; set; }
        public List<About> Abouts { get; set; }
        public List<Address> Addresses { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Category> Categories { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Contact> Contacts { get; set; }
        public List<Message2> Message2s { get; set; }
        public List<NewsLetter> NewsLetters { get; set; }
        public List<Notification> Notifications { get; set; }

    }
}
