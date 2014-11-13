using OPCLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OPCLib.Controls
{
    public class TreeViewOPCNode:TreeViewItem
    {
        private OPCNodeWrapper _node;

        public OPCNodeWrapper NodeWrapper
        {
            get
            {
                return _node;
            }
        }
        public TreeViewOPCNode(OPCNodeWrapper node)
        {
            _node = node;
            this.Header = node.ToString();
            BuildSubNodes();
        }

        private void BuildSubNodes()
        {
            foreach(OPCNodeWrapper node in _node.OPCNodeWrappers)
            {
                Items.Add(new TreeViewOPCNode(node));
            }
        }
    }
}
