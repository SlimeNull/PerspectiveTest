using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerspectiveTest.Helpers
{
    public class DependentValue<TDependency, T>
    {
        private readonly Func<TDependency, T> _factory;

        private bool _firstCreated = false;
        private TDependency? _dependency;
        private T? _createdValue;

        public DependentValue(Func<TDependency, T> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            _factory = factory;
        }

        public T Get(TDependency dependency, out bool updated)
        {
            updated = false;
            if (!_firstCreated ||
                !Object.Equals(dependency, _dependency))
            {
                _firstCreated = true;
                _dependency = dependency;
                _createdValue = _factory.Invoke(dependency);
                updated = true;
            }

            return _createdValue!;
        }

        public T Get(TDependency dependency) => Get(dependency, out _);
    }

    public class DependentValue<TDependency1, TDependency2, T>
    {
        private readonly Func<TDependency1, TDependency2 , T> _factory;

        private bool _firstCreated = false;
        private TDependency1? _dependency1;
        private TDependency2? _dependency2;
        private T? _createdValue;

        public DependentValue(Func<TDependency1, TDependency2, T> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            _factory = factory;
        }

        public T Get(
            TDependency1 dependency1,
            TDependency2 dependency2,
            out bool updated)
        {
            updated = false;
            if (!_firstCreated ||
                !Object.Equals(dependency1, _dependency1) ||
                !Object.Equals(dependency2, _dependency2))
            {
                _firstCreated = true;
                _dependency1 = dependency1;
                _dependency2 = dependency2;
                _createdValue = _factory.Invoke(dependency1, dependency2);
                updated = true;
            }

            return _createdValue!;
        }

        public T Get(
            TDependency1 dependency1,
            TDependency2 dependency2) 
            => Get(dependency1, dependency2, out _);
    }

    public class DependentValue<TDependency1, TDependency2, TDependency3, TDependency4, TDependency5, TDependency6, TDependency7, TDependency8, T>
    {
        private readonly Func<TDependency1, TDependency2, TDependency3, TDependency4, TDependency5, TDependency6, TDependency7, TDependency8, T> _factory;

        private bool _firstCreated = false;
        private TDependency1? _dependency1;
        private TDependency2? _dependency2;
        private TDependency3? _dependency3;
        private TDependency4? _dependency4;
        private TDependency5? _dependency5;
        private TDependency6? _dependency6;
        private TDependency7? _dependency7;
        private TDependency8? _dependency8;
        private T? _createdValue;

        public DependentValue(Func<TDependency1, TDependency2, TDependency3, TDependency4, TDependency5, TDependency6, TDependency7, TDependency8, T> factory)
        {
            ArgumentNullException.ThrowIfNull(factory);

            _factory = factory;
        }

        public T Get(
            TDependency1 dependency1,
            TDependency2 dependency2,
            TDependency3 dependency3,
            TDependency4 dependency4,
            TDependency5 dependency5,
            TDependency6 dependency6,
            TDependency7 dependency7,
            TDependency8 dependency8)
        {
            if (!_firstCreated ||
                !Object.Equals(dependency1, _dependency1) ||
                !Object.Equals(dependency2, _dependency2) ||
                !Object.Equals(dependency3, _dependency3) ||
                !Object.Equals(dependency4, _dependency4) ||
                !Object.Equals(dependency5, _dependency5) ||
                !Object.Equals(dependency6, _dependency6) ||
                !Object.Equals(dependency7, _dependency7) ||
                !Object.Equals(dependency8, _dependency8))
            {
                _firstCreated = true;
                _dependency1 = dependency1;
                _dependency2 = dependency2;
                _dependency3 = dependency3;
                _dependency4 = dependency4;
                _dependency5 = dependency5;
                _dependency6 = dependency6;
                _dependency7 = dependency7;
                _dependency8 = dependency8;
                _createdValue = _factory.Invoke(dependency1, dependency2, dependency3, dependency4, dependency5, dependency6, dependency7, dependency8);
            }

            return _createdValue!;
        }
    }
}
