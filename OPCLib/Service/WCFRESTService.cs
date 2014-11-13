using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace OPCLib.Service
{
    public class WCFRESTService
    {
        public void StartService()
        {
            StartService(SharedLib.Properties.Resources.WCFServiceUri);
        }

        private void StartService(string uri)
        {
            Uri address = new Uri(@uri);
            WebServiceHost host = new WebServiceHost(typeof(DomoticaService), address);
            ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IDomoticaService),
                new WebHttpBinding(), "");
            ServiceDebugBehavior sdb = host.Description.Behaviors.Find<ServiceDebugBehavior>();
            sdb.HttpHelpPageEnabled = false;
            host.Open();
        }
    }
}
