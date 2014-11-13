using OPCDashboard.ViewModels;
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
using CustomControls;

namespace OPCDashboard.Controls
{
    /// <summary>
    /// Interaction logic for LampDetailView.xaml
    /// </summary>
    public partial class LampDetailView : UserControl
    {
        private LampDetailVM _vm;
        public LampDetailView(LampDetailVM vm)
        {
            InitializeComponent();
            _vm = vm;
            this.DataContext = vm;
        }
    }
}
