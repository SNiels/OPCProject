using CustomControls;
using Microsoft.Expression.Interactivity.Layout;
using OPCDashboard.Controls;
using OPCDashboard.ViewModels;
using OPCLib.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OPCDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DashboardVM _vm;
        private List<OPCCSV> _opcCSVS = new List<OPCCSV>();
        private string _csvPath = AppDomain.CurrentDomain.BaseDirectory + @"data/";

        public MainWindow()
        {
            InitializeComponent();
            _vm = this.DataContext as DashboardVM;
            _vm.PropertyChanged += _vm_PropertyChanged;
            //ReadCSV();
        }

        private void ReadCSV()
        {
            string csvPath = GetPath();
            if (File.Exists(csvPath))
            {
                _opcCSVS.Clear();
                _opcCSVS.AddRange(OPCCSV.GetOPCCSVS(csvPath));
            }
        }

        private string GetPath()
        {
            return _csvPath + _vm.SelectedServer.Server.ToString() + ".csv";
        }

        void _vm_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string name =e.PropertyName;
            switch (name)
            {
                case "SelectedServer":
                    if (_vm.SelectedServer != null)
                    {
                        ReadCSV();
                    }
                    break;
                case "PreviewSelectedServer":
                    if (_vm.SelectedServer != null)
                    {
                        WriteCSV();
                    }
                    break;
                case "Lamps":
                    ShowLamps();
                    break;
                case "Heathers":
                    ShowHeathers();
                    break;
            }
        }

        private void ShowHeathers()
        {
            IEnumerable<CCHeather> oldHeathers = rootGrid.Children.OfType<CCHeather>().ToList();
            foreach (CCHeather heather in oldHeathers)
            {
                heather.MouseDoubleClick -= lamp_MouseDoubleClick;
                rootGrid.Children.Remove(heather);
            }

            IEnumerable<Heather> heathers = _vm.Heathers;
            if (heathers == null) return;

            foreach (Heather heather in heathers)
            {
                CCHeather heatherControl = new CCHeather(heather);//(wrapper, _vm.SelectedServer, _vm.Client);
                rootGrid.Children.Add(heatherControl);
                heatherControl.Height = 30;
                heatherControl.Width = 30;
                heatherControl.HorizontalAlignment = HorizontalAlignment.Left;
                heatherControl.VerticalAlignment = VerticalAlignment.Top;
                heatherControl.RenderTransform = new TranslateTransform();
                heatherControl.Background = new SolidColorBrush(Colors.Blue);

                MouseDragElementBehavior beh = new MouseDragElementBehavior();
                beh.Attach(heatherControl);
                heatherControl.MouseDoubleClick += heather_MouseDoubleClick;
            }

            PositionControls(rootGrid.Children.OfType<CCHeather>());
        }

        private void heather_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CCHeather heatherControl = sender as CCHeather;
            _vm.DetailsControl = new HeatherDetailView(new HeatherDetailVM(heatherControl.Heather));
        }

        private void ShowLamps()
        {
            IEnumerable<CCLamp> oldLamps = rootGrid.Children.OfType<CCLamp>().ToList();
            foreach (CCLamp lamp in oldLamps)
            {
                lamp.MouseDoubleClick -= lamp_MouseDoubleClick;
                rootGrid.Children.Remove(lamp);
            }
            var lamps = _vm.Lamps;
            if (lamps == null) return;
            foreach (OPCNodeWrapper wrapper in lamps)
            {
                CCLamp lamp = new CCLamp(wrapper, _vm.SelectedServer, _vm.Client);
                rootGrid.Children.Add(lamp);
                lamp.Height = 30;
                lamp.Width = 30;
                lamp.HorizontalAlignment = HorizontalAlignment.Left;
                lamp.VerticalAlignment = VerticalAlignment.Top;
                lamp.RenderTransform = new TranslateTransform();

                MouseDragElementBehavior beh = new MouseDragElementBehavior();
                beh.Attach(lamp);
                lamp.MouseDoubleClick += lamp_MouseDoubleClick;
            }
            PositionControls(rootGrid.Children.OfType<CCLamp>());
        }

        void lamp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _vm.SelectedNode = (sender as CCLamp).NodeWrapper;
            _vm.DetailsControl = new LampDetailView(new LampDetailVM(_vm.SelectedNode));
        }

        private void PositionControls(IEnumerable<IDraggableOPCNode> nodes)
        {
            foreach (IDraggableOPCNode node in nodes)
            {
                OPCCSV opccsv = _opcCSVS.Find(n => n.NodeName == node.NodeName);
                if (opccsv!=null)
                {
                    node.X = opccsv.X;
                    node.Y = opccsv.Y;
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (_vm.SelectedServer != null)
            {
                WriteCSV();
            }
            _vm.Client.UnsubscribeAllItems();
            _vm.Client.Dispose();
        }

        private void WriteCSV()
        {
            OPCCSV.WriteCSV(rootGrid.Children.OfType<IDraggableOPCNode>(), GetPath());
        }
    }
}
