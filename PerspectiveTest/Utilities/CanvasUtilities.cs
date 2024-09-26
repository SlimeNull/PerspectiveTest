using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PerspectiveTest.Utilities
{
    internal static class CanvasUtilities
    {
        public static void MoveHorizontal(UIElement element, double offset)
        {
            if (Canvas.GetLeft(element) is double left &&
                !double.IsNaN(left))
            {
                Canvas.SetLeft(element, left + offset);

                if (Canvas.GetRight(element) is double right &&
                    !double.IsNaN(right))
                {
                    Canvas.SetRight(element, right - offset);
                }
            }
            else if (Canvas.GetRight(element) is double right &&
                !double.IsNaN(right))
            {
                Canvas.SetRight(element, right - offset);
            }
            else
            {
                Canvas.SetLeft(element, offset);
            }
        }

        public static void MoveVertical(UIElement element, double offset)
        {
            if (Canvas.GetTop(element) is double top &&
                !double.IsNaN(top))
            {
                Canvas.SetTop(element, top + offset);

                if (Canvas.GetBottom(element) is double bottom &&
                    !double.IsNaN(bottom))
                {
                    Canvas.SetBottom(element, bottom - offset);
                }
            }
            else if (Canvas.GetBottom(element) is double bottom &&
                !double.IsNaN(bottom))
            {
                Canvas.SetBottom(element, bottom - offset);
            }
            else
            {
                Canvas.SetTop(element, offset);
            }
        }
    }
}
