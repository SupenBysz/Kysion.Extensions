using Kysion.Extensions.Core.Contracts;
using System.Windows;
using System.Windows.Threading;

namespace Kysion.Extensions.Core.Dispatchers
{
    public class ApplicationDispatcher : IDispatcher
    {
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationDispatcher"/> class with the given <see cref="Dispatcher"/>
        /// </summary>
        /// <param name="dispatcher"><see cref="Dispatcher"/> to use, normally Application.Current.Dispatcher</param>
        public ApplicationDispatcher(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationDispatcher"/> class with the given <see cref="Application"/>
        /// </summary>
        /// <param name="application"><see cref="Application"/> to use, normally Application</param>
        public ApplicationDispatcher(Application application)
            : this(application?.Dispatcher ?? throw new ArgumentNullException(nameof(application)))
        {
        }

        public void Post(Action action)
        {
            this.dispatcher.BeginInvoke(action);
        }

        public void Send(Action action)
        {
            this.dispatcher.Invoke(action);
        }

        public bool IsCurrent
        {
            get { return this.dispatcher.CheckAccess(); }
        }
    }
}
