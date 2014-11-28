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
        private bool IsServerNull
        {
            get
            {
                return SelectedServer == null;
            }
        }
        private OPCServerWrapper SelectedServer
        {
            get
            {
                return OPCServerWrapper.SelectedServer;
            }
        }
        public DomoticaService()
        {

        }
        public IEnumerable<WCFNode> GetWCFNodes()
        {
            var test = IsServerNull ? null : SelectedServer.WCFNodes;
            return test;
        }


        public object GetWCFNodeValue(string itemID)
        {
            return IsServerNull ? null : SelectedServer.GetOPCNodeValue(itemID);
        }

        public object SetWCFNode(string itemID, string value)
        {
            return IsServerNull ? null : SelectedServer.SetOPCNodeValue(itemID, value);
        }
    }
}
