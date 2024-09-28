using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using EleCho.WpfSuite;
using EleCho.WpfSuite.ValueConverters;
using OpenCvSharp;

namespace PerspectiveTest.Converters
{
    public class MatrixToStringConverter : ValueConverterBase<MatrixToStringConverter, Mat, string>
    {
        public override string? Convert(Mat value, Type targetType, object? parameter, CultureInfo culture)
        {
            StringBuilder sb = new();

            int rows = value.Rows;
            int cols = value.Cols;

            for (int r = 0; r < rows; r++)
            {
                if (r == 0)
                {
                    sb.Append('[');
                }
                else
                {
                    sb.Append(' ');
                }

                for (int c = 0; c < cols; c++)
                {
                    sb.Append(' ');
                    sb.AppendFormat("{0:0.00}", value.Get<double>(r, c));

                    if (c != cols - 1)
                    {
                        sb.Append(',');
                    }
                }

                if (r != rows - 1)
                {
                    sb.Append(';');
                    sb.Append('\n');
                }
            }

            sb.Append(' ');
            sb.Append(']');

            return sb.ToString();
        }
    }
}
