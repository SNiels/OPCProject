using OpcLabs.EasyOpc.DataAccess;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomControls
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Controls"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:Controls;assembly=Controls"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CCLamp/>
    ///
    /// </summary>
    public class CCLamp : Control, IDraggableOPCNode
    {
        static CCLamp()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CCLamp), new FrameworkPropertyMetadata(typeof(CCLamp)));
        }


        private const int TIME = 200;
        const int UPDATERATE = 1000;
        private OPCNodeWrapper _node;
        private OPCServerWrapper _server;
        private EasyDAClient _client;
        private Ellipse _ellipse;
        private Color _colorOn = Colors.Yellow;
        private Color _colorOff = Colors.Gray;
        private bool _isOn;
        private int _ticket = -1;

        public OPCNodeWrapper NodeWrapper
        {
            get
            {
                return _node;
            }
            private set
            {
                _node = value;
                if (_ticket != -1)
                {
                    _client.UnsubscribeItem(_ticket);
                }
                _ticket = _client.SubscribeItem(Environment.MachineName, _server.Server.ServerClass, value.ItemId, UPDATERATE);
            }
        }

        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                _isOn = value;
                if (value) AnimateColorToOn();
                else AnimateColorToOff(); ;
            }
        }

        public String NodeName
        {
            get
            {
                return NodeWrapper.Name;
            }
            set
            {
                NodeWrapper.Name = value;
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

        public double Y {
            get
            {
                return TranslateTransform.Y;
            }
            set
            {
                TranslateTransform.Y = value;
            }
        }

        public CCLamp(OPCNodeWrapper node, OPCServerWrapper server, EasyDAClient client)
        {
            _server=server;
            _client = client;
            NodeWrapper = node;
            client.ItemChanged += client_ItemChanged;
            this.Loaded += CCLamp_Loaded;
        }

        void CCLamp_Loaded(object sender, RoutedEventArgs e)
        {
            IsOn = IsOn;
        }

        void client_ItemChanged(object sender, EasyDAItemChangedEventArgs e)
        {
            if (e.Handle == _ticket)
            {
                IsOn = (bool)_node.Value;
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ellipse = GetTemplateChild("ellipse") as Ellipse;
        }

        private void AnimateColorToOn()
        {
            AnimateColor(_colorOff, _colorOn);
        }

        private void AnimateColorToOff()
        {
            AnimateColor(_colorOn, _colorOff);
        }

        private void AnimateColor(Color from, Color to)
        {
            ColorAnimation colorChangeAnimation = new ColorAnimation();
            colorChangeAnimation.From = from;
            colorChangeAnimation.To = to;
            colorChangeAnimation.Duration = new TimeSpan(0, 0, 0, 0, TIME); ;

            PropertyPath colorTargetPath = new PropertyPath("(Ellipse.Fill).(SolidColorBrush.Color)");
            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, _ellipse);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            sb.Children.Add(colorChangeAnimation);
            try
            {
                sb.Begin();
            }catch(Exception){

            }
        }

        
    }
}
