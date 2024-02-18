namespace Kysion.Extensions.Core.Contracts
{
    public interface IDispatcher
    {
        bool IsCurrent { get; }

        void Post(Action action);

        void Send(Action action);
    }
}
