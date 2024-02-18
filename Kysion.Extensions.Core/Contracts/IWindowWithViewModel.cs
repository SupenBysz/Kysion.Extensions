namespace Kysion.Extensions.Core.Contracts
{
    public interface IWindowWithViewModel<T> : IWindow
    {
        T ViewModel { get; set; }
    }
}
