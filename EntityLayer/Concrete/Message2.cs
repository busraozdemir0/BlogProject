using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Message2
    {
        [Key]
        public int MessageID { get; set; }
        public string Subject { get; set; }
        public string MessageDetails { get; set; }
        public DateTime MessageDate { get; set; }
        public bool MessageStatus { get; set; }
        public int SenderUserId { get; set; }
        public string SenderUserEmail { get; set; }
        public int ReceiverUserId { get; set; }
        public string ReceiverUserEmail { get; set; }
        public AppUser SenderUser { get; set; }
        public AppUser ReceiverUser { get; set; }
    }
}
