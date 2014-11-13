using GalaSoft.MvvmLight.Command;
using OPCLib.Controls;
using OPCLib.Models;
using OpcLabs.EasyOpc.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OPCLib.ViewModels
{
    public class NodeBrowserVM:BaseVM
    {
        const int UPDATERATE = 500;
        private EasyDAClient _client;
        private OPCServerWrapper _selectedServer;
        private OPCNodeWrapper _selectedNode;
        private  int _subscriptionTicket = -1;
        private object _selectedNodeValue;

        public IEnumerable<OPCServerWrapper> Servers
        { 
            get{
                return OPCServerWrapper.GetOPCServerWrappers();
            }
        }

        public OPCServerWrapper SelectedServer
        {
            get { return _selectedServer; }
            set { 
                _selectedServer = value;
                OnPropertyChanged("SelectedServer");
                OnPropertyChanged("Nodes");
            }
        }

        public IEnumerable<TreeViewOPCNode> Nodes { 
            get {
                if (SelectedServer == null)
                {
                    return null;
                }
                return SelectedServer.GetOPCNodeWrappers().Select(
                    node=>new TreeViewOPCNode(node)
                    );
            } 
        }

        public OPCNodeWrapper SelectedNode
        {
            get { return _selectedNode; }
            set {
                if(_subscriptionTicket!=-1){
                    _client.UnsubscribeItem(_subscriptionTicket);
                }
                if (value.IsLeaf)
                {
                    _subscriptionTicket = _client.SubscribeItem(Environment.MachineName, SelectedServer.Server.ServerClass, value.ItemId, UPDATERATE);
                    SelectedNodeValue = value.Value;
                }
                else
                {
                    _subscriptionTicket = -1;
                }
                _selectedNode = value;
                OnPropertyChanged("SelectedNode");
                OnPropertyChanged("IsSelectedNodeLeaf");
                OnPropertyChanged("SelectedNodeValue");
            }
        }

        public bool IsSelectedNodeLeaf { 
            get {
                if (SelectedNode == null) return false;
                return SelectedNode.IsLeaf;
            } 
        }

        public object SelectedNodeValue { 
            get {
                if (!IsSelectedNodeLeaf)
                {
                    return null;
                }
                return _selectedNodeValue;
            }
            set
            {
                if (!IsSelectedNodeLeaf)
                {
                    return;
                }
                _selectedNodeValue = value;
            }
        }

        public ICommand UpdateNodeValueCommand
        {
            get
            {
                return new RelayCommand(UpdateNodeValue);
            }
        }

        private void UpdateNodeValue()
        {
            if (SelectedNode != null)
            {
                SelectedNode.Value = _selectedNodeValue;
            }
        }

        public NodeBrowserVM()
        {
            _client = new OpcLabs.EasyOpc.DataAccess.EasyDAClient();
            OPCServerWrapper.Client = _client;
            _client.ItemChanged += client_ItemChanged;
        }

        void client_ItemChanged(object sender, EasyDAItemChangedEventArgs e)
        {
            SelectedNodeValue = SelectedNode.Value;
            OnPropertyChanged("SelectedNodeValue");
        }

    }
}
