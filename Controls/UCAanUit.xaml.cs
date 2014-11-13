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
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UCAanUit : UserControl
    {
        private const double RATIO = 0.2;
        private const int TIME = 200;
        private Color colorOn = Colors.Green;
        private Color colorOff = Colors.Red;
        private bool _isON;

        public bool IsOn
        {
            get { return _isON; }
            set { _isON = value;
            AnimateSlider();
            }
        }

        public UCAanUit()
        {
            InitializeComponent();
            this.MouseUp += UCAanUit_MouseUp;
            this.Loaded += UCAanUit_Loaded;
        }

        void UCAanUit_Loaded(object sender, RoutedEventArgs e)
        {
            InitCanvas();
        }

        void UCAanUit_MouseUp(object sender, MouseButtonEventArgs e)
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
            AnimateColor(colorOff, colorOn);
        }

        private void AnimateColorToOff()
        {
            AnimateColor(colorOn, colorOff);
        }

        private void AnimateColor(Color from, Color to)
        {
            ColorAnimation colorChangeAnimation = new ColorAnimation();
            colorChangeAnimation.From = from;
            colorChangeAnimation.To = to;
            colorChangeAnimation.Duration = new TimeSpan(0, 0, 0, 0, TIME); ;

            PropertyPath colorTargetPath = new PropertyPath("(Canvas.Background).(SolidColorBrush.Color)");
            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, cnv);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);
            sb.Children.Add(colorChangeAnimation);
            sb.Begin();
        }

        private void AnimateSliderToOn()
        {
            AnimateCanvasLeft(0, this.ActualWidth - cnv.ActualWidth);
            
        }
        private void AnimateSliderToOff()
        {
            AnimateCanvasLeft(Canvas.GetLeft(cnv),0);
        }

        private void AnimateCanvasLeft(double from, double to)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = from;
            animation.To = to;
            animation.Duration = new TimeSpan(0, 0, 0, 0, TIME);
            Storyboard.SetTarget(animation, cnv);
            Storyboard.SetTargetProperty(animation, new PropertyPath("Left"));
            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin(this);
        }


        private void InitCanvas()
        {
            cnv.Width = this.ActualWidth - (this.ActualWidth*RATIO);
            cnv.Height = this.ActualHeight;
            cnv.Background = new SolidColorBrush(colorOff);
        }
    }
}
