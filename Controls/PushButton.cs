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
    ///     <MyNamespace:PushButton/>
    ///
    /// </summary>
    public class PushButton : Control
    {
        private const int TIME = 300;
        private Color _defaultColor = Colors.Red;
        private Color _feedbackColor = Colors.Green;
        private Ellipse _ellipse;

        static PushButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PushButton), new FrameworkPropertyMetadata(typeof(PushButton)));
        }

        public PushButton()
        {
            this.MouseUp += PushButton_MouseUp;
        }

        void PushButton_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AnimateEllipseColor(_defaultColor,_feedbackColor);
            Beep();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _ellipse = GetTemplateChild("ellipse") as Ellipse;
        }

        private void Beep()
        {
            Console.Beep();
        }

        private void AnimateEllipseColor(Color from, Color to)
        {
            ColorAnimation colorChangeAnimation = new ColorAnimation();
            colorChangeAnimation.From = from;
            colorChangeAnimation.To = to;
            TimeSpan span = new TimeSpan(0, 0, 0, 0, TIME);
            colorChangeAnimation.Duration = span;

            PropertyPath colorTargetPath = new PropertyPath("(Ellipse.Fill).(SolidColorBrush.Color)");
            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(colorChangeAnimation, _ellipse);
            Storyboard.SetTargetProperty(colorChangeAnimation, colorTargetPath);

            ColorAnimation colorChangeAnimation2 = new ColorAnimation();
            colorChangeAnimation2.From = to;
            colorChangeAnimation2.To = from;
            colorChangeAnimation2.Duration = span;
            colorChangeAnimation2.BeginTime = span;

            Storyboard.SetTarget(colorChangeAnimation2, _ellipse);
            Storyboard.SetTargetProperty(colorChangeAnimation2, colorTargetPath);

            sb.Children.Add(colorChangeAnimation);
            sb.Children.Add(colorChangeAnimation2);
            sb.Begin();
        }
    }
}
