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

namespace CustomControls
{
    [TemplatePart(Name = "PART_border", Type = typeof(UIElement))]
    [TemplateVisualState(Name = "Off", GroupName = "HeatherStates")]
    [TemplateVisualState(Name = "On", GroupName = "HeatherStates")]
    public class CCHeather : Control, IDraggableOPCNode
    {
        private Heather _heather;
        static CCHeather()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CCHeather), new FrameworkPropertyMetadata(typeof(CCHeather)));
        }

        public string NodeName
        {
            get
            {
                return Heather.Name;
            }
            set
            {
                Heather.Name = value;
            }
        }
        public TranslateTransform TranslateTransform
        {
            get
            {
                return RenderTransform as TranslateTransform;
            }
        }

        public double X
        {
            get
            {
                return TranslateTransform.X;
            }
            set
            {
                TranslateTransform.X = value;
            }
        }

        public double Y
        {
            get
            {
                return TranslateTransform.Y;
            }
            set
            {
                TranslateTransform.Y = value;
            }
        }

        public Heather Heather
        {
            get
            {
                return _heather;
            }
            set
            {
                if (_heather != null)
                {
                    _heather.PropertyChanged -= HeatherPropertyChanged;
                }
                _heather = value;
                if (value != null)
                {
                    _heather.PropertyChanged += HeatherPropertyChanged;
                    _heather.ObserveChanges();
                }
            }
        }

        public CCHeather()
        {

        }

        public CCHeather(Heather heather)
        {
            Heather = heather;
        }

        private void HeatherPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            string prop = e.PropertyName;
            switch (prop)
            {
                case Heather.ISBURNING:
                    VisualStateManager.GoToState(this, Heather.IsBurning?"On":"Off", true);
                    break;
            }
        }
    }
}
