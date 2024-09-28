using System.Windows.Controls;
using System.Windows;

namespace PerspectiveTest.Helpers
{
    public record struct CanvasItemPosition
    {
        public double Left { get; set; }
        public double Right { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }

        public void LoadFrom(UIElement element)
        {
            Left = Canvas.GetLeft(element);
            Right = Canvas.GetRight(element);
            Top = Canvas.GetTop(element);
            Bottom = Canvas.GetBottom(element);
        }

        public void ApplyTo(UIElement element)
        {
            Canvas.SetLeft(element, Left);
            Canvas.SetRight(element, Right);
            Canvas.SetTop(element, Top);
            Canvas.SetBottom(element, Bottom);
        }

        public static CanvasItemPosition CreateFrom(UIElement element)
        {
            CanvasItemPosition result = new();
            result.LoadFrom(element);

            return result;
        }

        public CanvasItemPosition Offset(double x, double y)
        {
            CanvasItemPosition result = this;

            if (!double.IsNaN(result.Left))
            {
                result.Left += x;

                if (!double.IsNaN(result.Right))
                {
                    result.Right -= x;
                }
            }
            else if (!double.IsNaN(result.Right))
            {
                result.Right -= x;
            }
            else
            {
                result.Left = x;
            }

            if (!double.IsNaN(result.Top))
            {
                result.Top += y;

                if (!double.IsNaN(result.Bottom))
                {
                    result.Bottom -= y;
                }
            }
            else if (!double.IsNaN(result.Bottom))
            {
                result.Bottom -= y;
            }
            else
            {
                result.Top = y;
            }

            return result;
        }

        public CanvasItemPosition Limit(double width, double height)
        {
            CanvasItemPosition result = this;

            if (!double.IsNaN(result.Left))
            {
                result.Left = Math.Clamp(result.Left, 0, width);
            }

            if (!double.IsNaN(result.Right))
            {
                result.Right = Math.Clamp(result.Right, 0, width);
            }

            if (!double.IsNaN(result.Top))
            {
                result.Top = Math.Clamp(result.Top, 0, height);
            }

            if (!double.IsNaN(result.Bottom))
            {
                result.Bottom = Math.Clamp(result.Bottom, 0, height);
            }

            return result;
        }

        public CanvasItemPosition Offset(Vector offset)
        {
            return Offset(offset.X, offset.Y);
        }

        public CanvasItemPosition Limit(Size size)
        {
            return Limit(size.Width, size.Height);
        }
    }
}
