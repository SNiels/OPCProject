using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OPCLib.Models;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace OPCDashboard.ViewModels
{
    public class HeatherDetailVM:BaseVM
    {
        private string _name;
        private bool _isAuto;
        private bool _isBurning;
        private int _current;
        private int _prefered;

        public Heather Heather { get; set; }

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
        public bool IsAuto
        {
            get { return _isAuto; }
            set { 
                _isAuto = value;
                OnPropertyChanged("IsAuto");
            }
        }
        public bool IsBurning
        {
            get { return _isBurning; }
            set { 
                _isBurning = value;
                OnPropertyChanged("IsBurning");
            }
        }
        public int Prefered
        {
            get { return _prefered; }
            set { 
                _prefered = value;
                OnPropertyChanged("Prefered");
            }
        }
        public int Current
        {
            get { return _current; }
            set { 
                _current = value;
                OnPropertyChanged("Current");
            }
        }
        public ICommand UpdateHeatherCommand
        {
            get
            {
                return new RelayCommand(UpdateHeather);
            }
        }
        public HeatherDetailVM(){}
        public HeatherDetailVM(Heather heather)
        {
            Heather = heather;
            Name = heather.Name;
            IsAuto = (bool) heather.AutoReadNode.Value;
            IsBurning = (bool) heather.IsBurningReadNode.Value;
            Prefered = (int) heather.PreferedReadNode.Value;
            Current = (int) heather.CurrentReadNode.Value;
        }
        private void UpdateHeather()
        {
            Heather.Name = Name;
            Heather.AutoWriteNode.Value = IsAuto;
            Heather.IsBurningWriteNode.Value = IsBurning;
            Heather.PreferedWriteNode.Value = Prefered;
            Heather.CurrentWriteNode.Value = Current;
        }
    }
}
