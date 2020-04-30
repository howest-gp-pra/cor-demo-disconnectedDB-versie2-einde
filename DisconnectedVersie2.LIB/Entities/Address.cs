using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedVersie2.LIB.Entities
{
    public class Address
    {
        public string ID { get; set; }
        public string Naam { get; set; }
        public string Adres { get; set; }
        public string Post { get; set; }
        public string Gemeente { get; set; }
        public string Land { get; set; }
        public string Soort_ID { get; set; }
        public override string ToString()
        {
            return Naam;
        }
    }
}
