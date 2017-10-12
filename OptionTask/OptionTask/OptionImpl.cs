using System;

namespace OptionTask
{
    public struct Option<T>
    {
        private T _value;
        private bool _isNone;

        internal Option(T value, bool isNone)
        {
            _value = value;
            _isNone = isNone;
        }

        public bool IsSome() => !_isNone;
        public bool IsNone() => _isNone;

        public T Value()
        {
            if (!_isNone)
            {
                return _value;
            }
            else
            {
                throw new OptionException("Попытка запросить значение у None");
            }
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> function) =>
            _isNone ? new Option<TResult>(default(TResult), true) 
                    : new Option<TResult>(function(_value), false);
    }
}