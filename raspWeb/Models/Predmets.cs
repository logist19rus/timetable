using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace raspWeb.Models
{
    public class Predmets
    {
        public int Id { get; set; }
        public string PredmetName { get; set; }
    }

    public class LinkOfPredmets
    {
        public int Id { get; set; }
        public string Group { get; set; }
        public string Predmet { get; set; }
    }
}
