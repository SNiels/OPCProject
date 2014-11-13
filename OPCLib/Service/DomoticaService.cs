using OPCLib.Models;
using SharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace OPCLib.Service
{
    [ServiceBehavior(InstanceContextMode=InstanceContextMode.PerSession,ConcurrencyMode=ConcurrencyMode.Multiple)]
    public class DomoticaService:IDomoticaService
    {
        public DomoticaService()
        {

        }
        public IEnumerable<WCFNode> GetWCFNodes()
        {
            if (OPCServerWrapper.SelectedServer == null) return null;
            return OPCServerWrapper.SelectedServer.WCFNodes;
        }
    }
}
