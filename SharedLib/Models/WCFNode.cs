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
        private string _name;

        public string ItemId { get; set; }
        [JsonIgnore]
        public string Name
        {
            get
            {
                if (_name == null)
                {
                    _name = ItemId.Split('.').Last();
                }
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public override string ToString()
        {
            return ItemId;
        }

        public static async Task<IEnumerable<WCFNode>> GetWCFNodes(Uri uri)
        {
            HttpClient client = new HttpClient();
            String response = await client.GetStringAsync(uri);
            return ParseJSONToNodes(response);
        }

        public static IEnumerable<WCFNode> ParseJSONToNodes(String response)
        {
            IEnumerable<WCFNode> nodes;
            nodes = JsonConvert.DeserializeObject<IEnumerable<WCFNode>>(response);
            return nodes;
        }

        public static IEnumerable<WCFNode> GetLamps(IEnumerable<WCFNode> nodes)
        {
            return nodes.Where(l => l.Name.StartsWith("lamp") || l.Name.StartsWith("lmp"));
        }

        public static async Task<string> GetWCFValue(Uri uri)
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync(uri);
        }
    }
}
