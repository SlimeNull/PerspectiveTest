using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using EleCho.WpfSuite;
using OpenCvSharp;

namespace PerspectiveTest.Converters
{
    public class MatrixToStringConverter : ValueConverterBase<MatrixToStringConverter, Mat, string>
    {
        public override string? Convert(Mat? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value?.ToString();
        }
    }
}
