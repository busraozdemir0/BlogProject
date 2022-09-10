using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreDemo.Models
{
    public class UserAddViewModel
    {
        public int userId { get; set; }
        public string namesurname { get; set; }
        public string username { get; set; }
        public string about { get; set; }
        public string mail { get; set; }
        public string password { get; set; }
        public string image_yol { get; set; }
        [NotMapped]
        public IFormFile image { get; set; }
    }
}
