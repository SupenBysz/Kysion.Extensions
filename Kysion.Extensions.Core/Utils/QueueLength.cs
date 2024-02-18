using System.Collections.Concurrent;

namespace Kysion.Extensions.Core.Utils
{
    public class QueueLength<T> : ConcurrentQueue<T>
    {
        int length = 100;

        public event Action<T>? OnMessage;

        /// <summary>
        /// 初始化新实力
        /// </summary>
        /// <param name="length"></param>
        public QueueLength(int length)
        {
            this.length = length;
        }

        /// <summary>
        /// 将对象添加到结尾处
        /// </summary>
        /// <param name="item"></param>
        public new void Enqueue(T item)
        {
            if(base.Count == length)
                base.TryDequeue(out _);

            base.Enqueue(item);

            OnMessage?.Invoke(item);
        }

        /// <summary>
        /// 设置队列长度
        /// </summary>
        /// <param name="length"></param>
        public void SetLength(int length)
        {
            this.length = length;
        }
    }
}
