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

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "GetWCFNodeValue/{itemId}")]
        object GetWCFNodeValue(string itemID);

        [WebGet(ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "SetWCFNodeValue/{itemId}/{value}")]
        object SetWCFNode(string itemID, string value);
    }
}
