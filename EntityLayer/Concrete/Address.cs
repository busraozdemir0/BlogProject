using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
   public class Address
    {
        [Key]
        public int AdresId { get; set; }
        public string AdresTitle { get; set; }
        public string Adres { get; set; }
        public string Mail { get; set; }
        public string TelNo { get; set; }

    }
}
