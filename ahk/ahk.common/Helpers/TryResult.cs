namespace ahk.common
{
    public class TryResult<T>
    {
        public bool Success { get; }
        public T Value { get; }

        private TryResult(bool success, T value)
        {
            this.Success = success;
            this.Value = value;
        }

        public static TryResult<T> Failed() => new TryResult<T>(false, default(T));
        public static TryResult<T> Ok(T value) => new TryResult<T>(true, value);
    }
}
