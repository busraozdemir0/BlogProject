using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class SifremiUnuttumMailTut
    {
        public int Id { get; set; }
        public string Mail { get; set; }
        public SifremiUnuttum SifremiUnuttum { get; set; }
        public int SifremiUnuttumId { get; set; }
    }
}
