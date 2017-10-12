namespace OptionTask
{
    public static class Option
    {
        public static Option<T> Some<T>(T value) => new Option<T>(value, false);
        public static Option<T> None<T>() => new Option<T>(default(T), true);

        public static Option<T> Flatten<T>(Option<Option<T>> option) =>
            option.IsNone() ? new Option<T>(default(T), true) : option.Value();
    }
}
