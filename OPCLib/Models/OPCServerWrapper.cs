using OpcLabs.EasyOpc;
using OpcLabs.EasyOpc.DataAccess;
using SharedLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OPCLib.Models
{
    public class OPCServerWrapper
    {
        private const string CAMELCASEREGEX = @"(?=\p{Lu}\p{Ll})|(?<=\p{Ll})(?=\p{Lu})";
        private static EasyDAClient _client;

        public static EasyDAClient Client
        {
            get {
                if (_client == null)
                {
                    _client = new EasyDAClient();
                }
                return _client; }
            set { _client = value; }
        }

        public ServerElement Server { get; set; }

        public static OPCServerWrapper SelectedServer { get; set; }

        public IEnumerable<WCFNode> WCFNodes
        {
            get
            {
                 return  GetLeaves().Select(l => new WCFNode { ItemId = l.ItemId});
            }
        }

        public OPCServerWrapper(ServerElement element)
        {
            Server = element;
        }

        public override string ToString()
        {
            return Server.Description;
        }

        public static OPCServerWrapper GetOPCServerWrapper(string serverClass)
        {
            return GetOPCServerWrappers().Single(wrapper => wrapper.Server.ServerClass == serverClass);
        }

        public static IEnumerable<OPCServerWrapper> GetOPCServerWrappers()
        {
            return _client.BrowseServers(Environment.MachineName)
                .Select(server=>new OPCServerWrapper(server));
        }

        public OPCNodeWrapper GetOPCNodeWrapper(string itemId)
        {
            return GetOPCNodeWrappers().Single(wrapper => wrapper.ItemId == itemId);
        }

        public IEnumerable<OPCNodeWrapper> GetOPCNodeWrappers()
        {
            return _client.BrowseNodes(Environment.MachineName, Server.ServerClass, "", new DANodeFilter())
                .Where(node=>!node.ItemId.StartsWith("_"))
                .Select(node=>new OPCNodeWrapper(node,this));
        }

        public IEnumerable<OPCNodeWrapper> GetLeaves()
        {
            return GetOPCNodeWrappers().Select(n => GetLeaves(n)).SelectMany(x=>x);
        }

        private IEnumerable<OPCNodeWrapper> GetLeaves(OPCNodeWrapper node){
            IEnumerable<OPCNodeWrapper> nodes = node.OPCNodeWrappers;

            if (node.IsLeaf)
            {
                var res =new List<OPCNodeWrapper>();
                res.Add(node);
                return res ;
            }
            return nodes.Select(n=>GetLeaves(n)).SelectMany(x=>x);
        }

        public IEnumerable<OPCNodeWrapper> GetLamps()
        {
            return GetLeaves().Where(
                n => n.Name.StartsWith("lmp", StringComparison.CurrentCultureIgnoreCase) ||
                n.Name.StartsWith("lamp", StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<OPCNodeWrapper> GetHeatherNodes()
        {
            return GetLeaves().Where(
                n => n.Name.StartsWith("wrm", StringComparison.CurrentCultureIgnoreCase) ||
                n.Name.StartsWith("warm", StringComparison.CurrentCultureIgnoreCase));
        }

        public IEnumerable<Heather> GetHeathers()
        {
            Dictionary<String, Heather> dictionary = new Dictionary<string, Heather>();
            IEnumerable<OPCNodeWrapper> nodes = GetHeatherNodes();
            foreach (OPCNodeWrapper node in nodes)
            {
                String[] pieces = Regex.Split(node.Name, CAMELCASEREGEX);
                if (pieces.Length < 4) continue;
                string name = pieces[1];
                string prop = pieces[2];
                string readWrite = pieces[3];
                if (!dictionary.ContainsKey(name))
                {
                    dictionary.Add(name, new Heather(_client,this) { Name = name });
                }
                Heather heather = dictionary[name];
                switch (prop.ToLower())
                {
                    case "auto":
                        if (IsRead(readWrite))
                            heather.AutoReadNode = node;
                        else
                            heather.AutoWriteNode = node;
                        break;
                    case "brandt":
                        if (IsRead(readWrite))
                            heather.IsBurningReadNode = node;
                        else
                            heather.IsBurningWriteNode = node;
                        break;
                    case "gewenst":
                        if (IsRead(readWrite))
                            heather.PreferedReadNode = node;
                        else
                            heather.PreferedWriteNode = node;
                        break;
                    case "nu":
                        if (IsRead(readWrite))
                            heather.CurrentReadNode = node;
                        else
                            heather.CurrentWriteNode = node;
                        break;
                }
            }
            return dictionary.Values;
            
        }

        private bool IsRead(string readWrite)
        {
            switch (readWrite.ToLower())
            {
                case "read":
                    return true;
                default:
                    return false;
            }
        }

        public object GetOPCNodeValue(string itemID)
        {
            return _client.ReadItemValue(Environment.MachineName, Server.ServerClass, itemID);
        }

        public object SetOPCNodeValue(string itemID, object value)
        {
            _client.WriteItemValue(Environment.MachineName, Server.ProgId, itemID, value);
            return value;
        }
    }
}
