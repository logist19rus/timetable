using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raspWeb.Models
{
    public class EveryPara
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public int number { get; set; }
        public string Group { get; set; }
        public string Prepod { get; set; }
        public string Predmet { get; set; }
        public string Kabinet { get; set; }
        public int subGroup { get; set; }
    }
}
