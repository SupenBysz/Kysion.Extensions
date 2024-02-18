namespace Kysion.Extensions.Core.Contracts
{
    public interface INotifyPropertyChangedDispatcher
    {
        Action<Action> PropertyChangedDispatcher { get; set; }
    }
}
