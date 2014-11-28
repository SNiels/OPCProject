using OPCDashboard.ViewModels;
using OPCLib.Models;
using System;
using System.Collections.Generic;
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

namespace OPCDashboard.Controls
{
    /// <summary>
    /// Interaction logic for HeatherDetailControl.xaml
    /// </summary>
    public partial class HeatherDetailView : UserControl
    {

        public HeatherDetailView()
        {
            InitializeComponent();
        }

        public HeatherDetailView(HeatherDetailVM vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}
