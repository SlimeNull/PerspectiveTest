using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace PerspectiveTest.Models
{
    [ObservableObject]
    public partial class RefPoint
    {
        public RefPoint() { }

        public RefPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        [ObservableProperty]
        private int _x;

        [ObservableProperty]
        private int _y;
    }
}
