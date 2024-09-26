using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PerspectiveTest.Utilities;

namespace PerspectiveTest.Controls
{
    class HandlePointInCanvas : ContentControl
    {
        private Point _mouseLastPosition;

        static HandlePointInCanvas()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HandlePointInCanvas), new FrameworkPropertyMetadata(typeof(HandlePointInCanvas)));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            _mouseLastPosition = e.GetPosition(null);
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
                var offset = currentPosition - _mouseLastPosition;

                CanvasUtilities.MoveHorizontal(this, offset.X);
                CanvasUtilities.MoveVertical(this, offset.Y);

                _mouseLastPosition = currentPosition;
            }

            base.OnMouseMove(e);
        }
    }
}
