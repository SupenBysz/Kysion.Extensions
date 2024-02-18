using Microsoft.Extensions.Logging;

namespace Kysion.Extensions.Core.Contracts
{
    public interface IBaseViewModel
    {
        abstract string Title { get; }
        abstract ILogger logger { get; }
    }
}
