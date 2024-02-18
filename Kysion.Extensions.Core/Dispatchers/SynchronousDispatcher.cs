using Kysion.Extensions.Core.Contracts;

namespace Kysion.Extensions.Core.Dispatchers
{
    public class SynchronousDispatcher : IDispatcher
    {
        public static SynchronousDispatcher Instance { get; } = new SynchronousDispatcher();
        private SynchronousDispatcher() { }

        public void Post(Action action)
        {
            action();
        }

        public void Send(Action action)
        {
            action();
        }

        public bool IsCurrent
        {
            get { return true; }
        }
    }
}
