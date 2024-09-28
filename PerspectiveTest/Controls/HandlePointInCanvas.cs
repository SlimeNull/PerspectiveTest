using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PerspectiveTest.Helpers;
using PerspectiveTest.Utilities;

namespace PerspectiveTest.Controls
{
    class HandlePointInCanvas : ContentControl
    {
        private Point _mouseStartPosition;
        private CanvasItemPosition _startPosition;

        public bool LimitInParent
        {
            get { return (bool)GetValue(LimitInParentProperty); }
            set { SetValue(LimitInParentProperty, value); }
        }

        public static readonly DependencyProperty LimitInParentProperty =
            DependencyProperty.Register(nameof(LimitInParent), typeof(bool), typeof(HandlePointInCanvas), new PropertyMetadata(true));



        static HandlePointInCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HandlePointInCanvas), new FrameworkPropertyMetadata(typeof(HandlePointInCanvas)));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _mouseStartPosition = e.GetPosition(null);
            _startPosition = CanvasItemPosition.CreateFrom(this);
            
            CaptureMouse();

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            ReleaseMouseCapture();

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (IsMouseCaptured)
            {
                var currentPosition = e.GetPosition(null);
                var offset = currentPosition - _mouseStartPosition;

                if (Keyboard.IsKeyDown(Key.LeftShift))
                {
                    if (Math.Abs(offset.X) > Math.Abs(offset.Y))
                    {
                        offset.Y = 0;
                    }
                    else
                    {
                        offset.X = 0;
                    }
                }

                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    var distance = offset.Length;

                    offset.X = Math.Sign(offset.X) * offset.Length;
                    offset.Y = Math.Sign(offset.Y) * offset.Length;
                }

                var position = _startPosition.Offset(offset);

                if (LimitInParent && Parent is FrameworkElement parentElement)
                {
                    position = position.Limit(parentElement.ActualWidth, parentElement.ActualHeight);
                }

                position.ApplyTo(this);
            }

            base.OnMouseMove(e);
        }
    }
}
