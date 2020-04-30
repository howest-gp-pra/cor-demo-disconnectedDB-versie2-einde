using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisconnectedVersie2.LIB.Entities
{
    public class AddressType
    {
        public string ID { get; set; }
        public string Soort { get; set; }
        public override string ToString()
        {
            return Soort;
        }
    }
}
