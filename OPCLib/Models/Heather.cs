using OpcLabs.EasyOpc.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OPCLib.Models
{
    public class Heather:BasePropertyChanged
    {
        public const string ISAUTO = "IsAuto";
        public const string ISBURNING = "IsBurning";
        public const string PREFEREDHEATH = "PreferedHeath";
        public const string CURRENTHEATH = "CurrentHeath";

        private const int TIME = 200;
        const int UPDATERATE = 1000;
        private OPCServerWrapper _server;
        private EasyDAClient _client;
        private int _autoReadNodeTicket = -1;
        private int _isBurningReadNodeTicket = -1;
        private int _preferedReadNodeTicket = -1;
        private int _currentReadNodeTicket = -1;

        public string Name { get; set; }

        public OPCNodeWrapper AutoReadNode { get; set; }
        public OPCNodeWrapper AutoWriteNode { get; set; }
        public OPCNodeWrapper IsBurningReadNode { get; set; }
        public OPCNodeWrapper IsBurningWriteNode { get; set; }
        public OPCNodeWrapper PreferedReadNode { get; set; }
        public OPCNodeWrapper PreferedWriteNode { get; set; }
        public OPCNodeWrapper CurrentReadNode { get; set; }
        public OPCNodeWrapper CurrentWriteNode { get; set; }

        public bool IsAuto
        {
            get
            {
                return (bool)AutoReadNode.Value;
            }
            set
            {
                AutoWriteNode.Value = value;
            }
        }
        public bool IsBurning
        {
            get
            {
                return (bool)IsBurningReadNode.Value;
            }
            set
            {
                IsBurningWriteNode.Value = value;
            }
        }
        public int PreferedHeath
        {
            get
            {
                return (int)PreferedReadNode.Value;
            }
            set
            {
                PreferedWriteNode.Value = value;
            }
        }
        public int CurrentHeath
        {
            get
            {
                return (int)CurrentReadNode.Value;
            }
            set
            {
                CurrentWriteNode.Value = value;
            }
        }

        public Heather(EasyDAClient client,OPCServerWrapper server)
        {
            _client = client;
            _server = server;
            client.ItemChanged += client_ItemChanged;
        }

        public void ObserveChanges(){
             _autoReadNodeTicket = _client.SubscribeItem(Environment.MachineName, _server.Server.ServerClass, AutoReadNode.ItemId, UPDATERATE);
             _isBurningReadNodeTicket = _client.SubscribeItem(Environment.MachineName, _server.Server.ServerClass, IsBurningReadNode.ItemId, UPDATERATE);
             _preferedReadNodeTicket = _client.SubscribeItem(Environment.MachineName, _server.Server.ServerClass, PreferedReadNode.ItemId, UPDATERATE);
             _currentReadNodeTicket = _client.SubscribeItem(Environment.MachineName, _server.Server.ServerClass, CurrentReadNode.ItemId, UPDATERATE);
        }

        private void client_ItemChanged(object sender, EasyDAItemChangedEventArgs e)
        {
            int ticket = e.Handle;
            if (ticket == _autoReadNodeTicket)
            {
                OnPropertyChanged(ISAUTO);
            }
            else if (ticket == _isBurningReadNodeTicket)
            {
                OnPropertyChanged(ISBURNING);
            }
            else if (ticket == _preferedReadNodeTicket)
            {
                OnPropertyChanged(PREFEREDHEATH);
            }
            else if (ticket == _currentReadNodeTicket)
            {
                OnPropertyChanged(CURRENTHEATH);
            }
        }
    }
}
