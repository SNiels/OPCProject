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
    ///     <MyNamespace:CCAanUit/>
    ///
    /// </summary>
    public class CCAanUit : Control
    {
        private const double RATIO = 0.2;
        private const int TIME = 200;
        private Color _colorOn = Colors.Green;
        private Color _colorOff = Colors.Red;
        private bool _isON;
        private Canvas _rootCanvas;
        private Canvas _cnv;

        public bool IsOn
        {
            get { return _isON; }
            set
            {
                _isON = value;
                AnimateSlider();
            }
        }
        static CCAanUit()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CCAanUit), new FrameworkPropertyMetadata(typeof(CCAanUit)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rootCanvas=GetTemplateChild("rootCanvas") as Canvas;
            _cnv=GetTemplateChild("cnv") as Canvas;
            this.MouseUp += CCAanUit_MouseUp;
            this.Loaded += CCAanUit_Loaded;
        }

        void CCAanUit_Loaded(object sender, RoutedEventArgs e)
        {
            InitCanvas();
        }

        void CCAanUit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IsOn = !IsOn;
        }

        private void AnimateSlider()
        {
            if (IsOn)
            {
                AnimateSliderToOn();
                AnimateColorToOn();
            }
            else
            {
                AnimateSliderToOff();
                AnimateColorToOff();
            }
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

            PropertyPath colorTargetPath = new PropertyPath("(Canvas.Background).(SolidColorBrush.Color)");
            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, _cnv);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            sb.Children.Add(colorChangeAnimation);
            sb.Begin();
        }

        private void AnimateSliderToOn()
        {
            AnimateCanvasLeft(0, this.ActualWidth - _cnv.ActualWidth);
            
        }
        private void AnimateSliderToOff()
        {
            AnimateCanvasLeft(Canvas.GetLeft(_cnv), 0);
        }

        private void AnimateCanvasLeft(double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new TimeSpan(0, 0, 0, 0, TIME);
            Storyboard.SetTarget(animation, _cnv);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Left"));
            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin(this);
        }


        private void InitCanvas()
        {
            _cnv.Width = this.ActualWidth - (this.ActualWidth * RATIO);
            _cnv.Height = this.ActualHeight;
            _cnv.Background = new SolidColorBrush(_colorOff);
        }
    }
}
