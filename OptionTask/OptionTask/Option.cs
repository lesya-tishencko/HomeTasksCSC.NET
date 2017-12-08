namespace OptionTask
{
    public static class Option
    {
        public static Option<T> Some<T>(T value) => Option<T>.Some(value);
        public static Option<T> None<T>() => Option<T>.None;

        public static Option<T> Flatten<T>(this Option<Option<T>> option) =>
            option.IsNone ? Option<T>.None : option.Value();
    }
}
