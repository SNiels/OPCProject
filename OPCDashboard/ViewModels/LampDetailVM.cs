using GalaSoft.MvvmLight.Command;
using OPCLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OPCDashboard.ViewModels
{
    public class LampDetailVM:BaseVM
    {
        private bool _value;
        private string _name;
        public OPCNodeWrapper Node { get; set; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public bool Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
        public ICommand UpdateLampCommand
        {
            get
            {
                return new RelayCommand(UpdateLamp);
            }
        }
        public LampDetailVM()
        {

        }
        public LampDetailVM(OPCNodeWrapper node)
        {
            Node = node;
            Name = node.Name;
            Value =(bool) node.Value;
        }

        private void UpdateLamp()
        {
            Node.Name = Name;
            Node.Value = Value;
        }
    }
}
