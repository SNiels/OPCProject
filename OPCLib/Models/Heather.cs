using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCLib.Models
{
    public class Heather
    {
        public string Name { get; set; }

        public OPCNodeWrapper AutoReadNode { get; set; }
        public OPCNodeWrapper AutoWriteNode { get; set; }
        public OPCNodeWrapper IsBurningReadNode { get; set; }
        public OPCNodeWrapper IsBurningWriteNode { get; set; }
        public OPCNodeWrapper PreferedReadNode { get; set; }
        public OPCNodeWrapper PreferedWriteNode { get; set; }
        public OPCNodeWrapper CurrentReadNode { get; set; }
        public OPCNodeWrapper CurrentWriteNode { get; set; }
    }
}
