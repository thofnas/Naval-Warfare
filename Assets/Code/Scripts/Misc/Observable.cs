using System;
using System.Collections.Generic;

namespace Misc
{
    public class Observable<T>
    {
        private T _value;

        public event Action<T> OnValueChanged;

        public T Value
        {
            get => _value;
            set
            {
                if (EqualityComparer<T>.Default.Equals(_value, value)) return;
                
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }

        public Observable(T initialValue = default)
        {
            _value = initialValue;
        }
    }
}