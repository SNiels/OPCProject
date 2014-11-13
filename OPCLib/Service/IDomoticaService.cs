using SharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace OPCLib.Service
{
    [ServiceContract]
    public interface IDomoticaService
    {
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        IEnumerable<WCFNode> GetWCFNodes();
    }
}
