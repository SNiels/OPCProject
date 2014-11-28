using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

// The Templated Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234235

namespace DomoticaApp
{

    [TemplatePart(Name = "ellipse", Type = typeof(Ellipse))]
    [TemplateVisualState(Name = "Off", GroupName = "LampStates")]
    [TemplateVisualState(Name = "On", GroupName = "LampStates")]
    public sealed class CCLamp : Control, IDraggableWCFNode
    {
        private bool _isOn;
        private Ellipse _ellipse;

        public WCFNode WCFNode { get; set; }

        public bool IsON
        {
            get { return _isOn; }
            set { _isOn = value;
            AnimateIsOn();
            }
        }


        public string NodeName
        {
            get
            {
                return WCFNode.Name;
            }
            set
            {
                WCFNode.Name = value;
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

        public TranslateTransform TranslateTransform
        {
            get
            {
                return RenderTransform as TranslateTransform;
            }
        }

        public CCLamp(WCFNode node)
        {
            WCFNode = node;
            this.DefaultStyleKey = typeof(CCLamp);
            this.Loaded += CCLamp_Loaded;
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ellipse = GetTemplateChild("ellipse") as Ellipse;
        }

        void CCLamp_Loaded(object sender, RoutedEventArgs e)
        {
            AnimateIsOn();
        }

        private void AnimateIsOn()
        {
            VisualStateManager.GoToState(this, IsON ? "On" : "Off", true);
        }
    }
}
