using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib.Models
{
    public class WCFNode
    {
        public string ItemId { get; set; }
        public override string ToString()
        {
            return ItemId;
        }

        public static async Task<IEnumerable<WCFNode>> GetWCFNodes()
        {
            HttpClient client = new HttpClient();
            Uri uri = new Uri(Properties.Resources.WCFServiceUri);
            String response = await client.GetStringAsync(uri);
            return ParseJSONToNodes(response);
        }

        public static IEnumerable<WCFNode> ParseJSONToNodes(String response)
        {
            IEnumerable<WCFNode> nodes;
            nodes = JsonConvert.DeserializeObject<IEnumerable<WCFNode>>(response);
            return nodes;
        }
    }
}
