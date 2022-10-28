using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class SifremiUnuttum
    {
        [Key]
        public int Id { get; set; }
        public string Mail { get; set; }
        public string RandomKod { get; set; }
        public DateTime Tarih { get; set; }
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public SifremiUnuttumMailTut SifremiUnuttumMailTut { get; set; }

    }
}
