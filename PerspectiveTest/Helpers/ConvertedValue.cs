using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerspectiveTest.Helpers
{
    public class ConvertedValue<TFrom, T> : INotifyPropertyChanging, INotifyPropertyChanged
        where TFrom : INotifyPropertyChanged
    {
        private readonly TFrom _from;
        private readonly Func<TFrom, T> _converter;
        private T _convertedValue;

        public T Value => _convertedValue;

        public ConvertedValue(TFrom from, Func<TFrom, T> converter)
        {
            ArgumentNullException.ThrowIfNull(converter);

            _from = from;
            _converter = converter;
            _convertedValue = converter.Invoke(from);

            from.PropertyChanged += From_PropertyChanged;
        }

        ~ConvertedValue()
        {
            _from.PropertyChanged -= From_PropertyChanged;
        }

        private void From_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            Update();
        }

        public void Update()
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(nameof(Value)));
            _convertedValue = _converter.Invoke(_from);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;
    }
}
