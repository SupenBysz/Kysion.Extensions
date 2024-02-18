namespace Kysion.Extensions.Core.Utils
{
    /// <summary>
    /// 去抖动实现
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Debounce<T>
    {
        private readonly int _debounceTimeout;
        private readonly Action<T>? _action;
        private CancellationTokenSource? _cancel;


        public Debounce(Action<T> func, int debounceTimeout = 500)
        {
            _action = func;
            _debounceTimeout = debounceTimeout;
        }

        public async void UpdateValue(T value, Action<T>? func = null)
        {
            _cancel?.Cancel();

            _cancel = new CancellationTokenSource();
            await Task.Delay(_debounceTimeout, _cancel.Token).ContinueWith(t =>
            {
                if (t.IsCanceled) return;

                _action?.Invoke(value);
                func?.Invoke(value);
            });
        }
    }
}
