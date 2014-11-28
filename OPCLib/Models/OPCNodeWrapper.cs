using OpcLabs.EasyOpc.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPCLib.Models
{
    public class OPCNodeWrapper
    {
        private DANodeElement _node;
        private Type _valueType;
        public OPCServerWrapper Server { get; set; }

        public bool IsLeaf { 
            get {
                return _node.IsLeaf;
            } 
        }

        public bool IsRoot
        {
            get
            {
                return _node.IsRoot;
            }
        }

        public bool HasChildren
        {
            get
            {
                return _node.HasChildren;
            }
        }

        public String Name
        {
            get
            {
                return _node.Name;
            }
            set
            {
                _node.Name = value;
            }
        }

        public object Value
        {
            get
            {
                if (!IsLeaf)
                    return null;
                object value = Server.GetOPCNodeValue(_node.ItemId);
                return value;
            }
            set
            {
                if (_valueType == null)
                {
                   _valueType=Value.GetType();
                }
                if (_valueType == typeof(bool))
                {
                    Boolean waarde;
                    if (Boolean.TryParse(value.ToString(), out waarde))
                    {
                        Server.SetOPCNodeValue(_node.ItemId, waarde);
                    }
                    else
                    {
                        throw new Exception("Ongeldige boolean waarde: " + value.ToString());
                    }
                }
                else if (_valueType == typeof(string))
                {
                    Server.SetOPCNodeValue(_node.ItemId, value.ToString());
                }
                else if (_valueType == typeof(double))
                {
                    Double waarde;
                    if (Double.TryParse(value.ToString(), out waarde))
                    {
                        Server.SetOPCNodeValue(_node.ItemId, waarde);
                    }
                    else
                    {
                        throw new Exception("Ongeldige Double waarde: " + value.ToString());
                    }
                }
                else if (_valueType == typeof(float))
                {
                    float waarde;
                    if (float.TryParse(value.ToString(), out waarde))
                    {
                        Server.SetOPCNodeValue(_node.ItemId, waarde);
                    }
                    else
                    {
                        throw new Exception("Ongeldige float waarde: " + value.ToString());
                    }
                }
                else if (_valueType == typeof(int))
                {
                    int waarde;
                    if (int.TryParse(value.ToString(), out waarde))
                    {
                        Server.SetOPCNodeValue(_node.ItemId, waarde);
                    }
                    else
                    {
                        throw new Exception("Ongeldige int waarde: " + value.ToString());
                    }
                }
                else if (_valueType == typeof(byte))
                {
                    Byte waarde;
                    if (Byte.TryParse(value.ToString(), out waarde))
                    {
                        Server.SetOPCNodeValue(_node.ItemId, waarde);
                    }
                    else
                    {
                        throw new Exception("Ongeldige Byte waarde: " + value.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("Unsupported Type");
                }
            }
        }

        public IEnumerable<OPCNodeWrapper> OPCNodeWrappers
        {
            get
            {
                if (!HasChildren)
                {
                    return new List<OPCNodeWrapper>();
                }
                return OPCServerWrapper.Client.BrowseNodes(Environment.MachineName, Server.Server.ServerClass, _node.ItemId, new DANodeFilter())
                    .Where(node => !node.Name.StartsWith("_"))
                    .Select(node => new OPCNodeWrapper(node, Server));
            }
        }

        public string ItemId { get { return _node.ItemId; } }

        public OPCNodeWrapper(DANodeElement node,OPCServerWrapper server)
        {
            _node = node;
            Server = server;
        }

        public override string ToString()
        {
            return _node.Name;
        }
    }
}
