using GalaSoft.MvvmLight.Command;
using OPCLib.Models;
using OpcLabs.EasyOpc.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;

namespace OPCDashboard.ViewModels
{
    public class DashboardVM:BaseVM
    {
        const int UPDATERATE = 1000;
        private EasyDAClient _client;
        private OPCServerWrapper _selectedServer;
        private OPCNodeWrapper _selectedNode;
        private UserControl _detailsControl;

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
                OnPropertyChanged("PreviewSelectedServer");
                _selectedServer = value;
                OPCServerWrapper.SelectedServer = value;
                OnPropertyChanged("SelectedServer");
                OnPropertyChanged("Lamps");
                OnPropertyChanged("Heathers");
            }
        }

        public EasyDAClient Client
        {
            get
            {
                return _client;
            }
        }

        public IEnumerable<OPCNodeWrapper> Lamps { 
            get {
                if (SelectedServer == null)
                {
                    return null;
                }
                return SelectedServer.GetLamps();
            } 
        }

        public IEnumerable<Heather> Heathers
        {
            get
            {
                if (SelectedServer == null)
                {
                    return null;
                }
                return SelectedServer.GetHeathers();
            }
        }

        public OPCNodeWrapper SelectedNode
        {
            get { return _selectedNode; }
            set {
                _selectedNode = value;
                OnPropertyChanged("SelectedNode");
            }
        }
        public UserControl DetailsControl
        {
            get
            {
                return _detailsControl;
            }
            set
            {
                _detailsControl = value;
                OnPropertyChanged("DetailsControl");
            }
        }

        public DashboardVM()
        {
            _client = new OpcLabs.EasyOpc.DataAccess.EasyDAClient();
            OPCServerWrapper.Client = _client;
        }
    }
}
