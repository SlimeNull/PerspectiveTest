using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PerspectiveTest.Geometry
{
    public record struct Quad
    {
        public Quad(Point leftTop, Point rightTop, Point rightBottom, Point leftBottom)
        {
            LeftTop = leftTop;
            RightTop = rightTop;
            RightBottom = rightBottom;
            LeftBottom = leftBottom;
        }

        public Point LeftTop { get; }
        public Point RightTop { get; }
        public Point RightBottom { get; }
        public Point LeftBottom { get; }
    }
}
