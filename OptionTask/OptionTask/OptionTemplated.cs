using System;

namespace OptionTask
{
    public class Option<T>
    {
        private T _value;
        public static Option<T> None = new Option<T>(default(T), true);

        public static Option<T> Some(T value)
        {
            return new Option<T>(value, false);
        } 

        private Option(T value, bool isNone)
        {
            _value = value;
            IsNone = isNone;
        }

        public bool IsNone { get; }
        public bool IsSome => !IsNone;

        public T Value() 
        {
            if (IsSome)
            {
                return _value;
            }
            else
            {
                throw new OptionException("Попытка запросить значение у None");
            }
        }

        public Option<TResult> Map<TResult>(Func<T, TResult> function) =>
            IsNone ? Option<TResult>.None 
                    : new Option<TResult>(function(_value), false);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var other = (Option<T>)obj;
            return IsNone && other.IsNone || _value.Equals(other._value);
        }

        public override int GetHashCode() => _value.GetHashCode() ^ IsNone.GetHashCode();
    }
}