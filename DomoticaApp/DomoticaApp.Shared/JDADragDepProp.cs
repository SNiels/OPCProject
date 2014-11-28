using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
//using System.Windows.Media;

namespace JDADragBehavior
    {
    public class JDADragDepProp
        {
        protected UIElement _uie;
        protected Point _pntMouseDown;
        protected Boolean _bMoveFlag = false;
        //private Point _pntOriginalLocation;
        private Point _pntMoveStartLocation;
        protected UIElement _parent;	//visual parent van het te verzetten uie
        private TranslateTransform _tt = new TranslateTransform();	//Default blijven we staan

        public static Boolean GetJDADrag(DependencyObject obj)
            {
            return (Boolean)obj.GetValue(JDADragProperty);
            }

        public static void SetJDADrag(DependencyObject obj, Boolean value)
            {
            obj.SetValue(JDADragProperty, value);
            }

        // Using a DependencyProperty as the backing store for JDADrag.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty JDADragProperty =
            DependencyProperty.RegisterAttached("JDADrag", typeof(Boolean), typeof(JDADragDepProp),
            new PropertyMetadata(false, JDADragChanged));

        private static void JDADragChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
            {
            UIElement uie = target as UIElement;
            if (!(Boolean) e.NewValue) return;
            if (uie == null) return;

            //in uitleg misschien starten met andere klasse
            JDADragDepProp drag = new JDADragDepProp();		// (JDADragBehavior.JDADragDepProp)e.NewValue;
            drag.uie = uie;
            }

        public JDADragDepProp()
            {
            }

        protected UIElement uie
            {
            get { return _uie; }
            set
                {
                _uie = value;
                _parent = VisualTreeHelper.GetParent(_uie) as UIElement;
                if (_parent == null) return;

                _uie.AddHandler(UIElement.PointerPressedEvent, new PointerEventHandler (uie_PointerPressed),true);
                _uie.AddHandler(UIElement.PointerMovedEvent, new PointerEventHandler(_uie_PointerMoved), true);
                _uie.AddHandler(UIElement.PointerReleasedEvent, new PointerEventHandler(_uie_PointerReleased), true);
                _uie.RenderTransform = _tt;
                }
            }

        void _uie_PointerReleased(object sender, PointerRoutedEventArgs e)
            {
            _bMoveFlag = false;
            (sender as UIElement).ReleasePointerCapture(e.Pointer);
            }

        void _uie_PointerMoved(object sender, PointerRoutedEventArgs e)
            {
            if (_parent == null) return;
            if (!_bMoveFlag) return;
            Point pntMouseMove = e.GetCurrentPoint(sender as UIElement).Position;
            double deltaX = pntMouseMove.X - _pntMouseDown.X;
            double deltaY = pntMouseMove.Y - _pntMouseDown.Y;

            _tt.X += deltaX;
            _tt.Y += deltaY;
            }

        void uie_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
            {
            UIElement uie = (UIElement)sender;
            uie.CapturePointer(e.Pointer);
            _pntMouseDown =  e.GetCurrentPoint(sender as UIElement).Position;
            _pntMoveStartLocation = new Point(_tt.X, _tt.Y);
            _bMoveFlag = true;
            }

        }
    }