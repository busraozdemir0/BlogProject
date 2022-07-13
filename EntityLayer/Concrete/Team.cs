using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
   public class Team
    {
        [Key]
        public int TeamID { get; set; }
        public string TeamName { get; set; }
        public virtual  ICollection<Match> HomeMatchs{ get; set; }
        public virtual ICollection<Match> AwayMatchs { get; set; }

    }
}
