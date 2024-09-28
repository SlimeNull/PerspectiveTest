using System.Collections.ObjectModel;
using OpenCvSharp;

namespace TestConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mat m = new Mat<float>(3, 1);
            double value = m.Get<double>(0, 0);
        }
    }
}
