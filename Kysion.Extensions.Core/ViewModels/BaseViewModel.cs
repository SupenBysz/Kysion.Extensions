using Kysion.Extensions.Core.Contracts;
using Kysion.Extensions.Core.Models.Base;
using Kysion.Extensions.Core.Services;
using Microsoft.Extensions.Logging;

namespace Kysion.Extensions.Core.ViewModels
{
    public class BaseViewModel<T> : BaseModel, IBaseViewModel, IDisposable
    {
        public string Title
        {
            get => data.Title ??= string.Empty;
            set => SetAndNotify(() => data.Title = value);
        }

        private ILogger? _logger;
        public ILogger logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LoggerService.CreateLogger<T>(Title);
                }
                return _logger;
            }
        }

        public BaseViewModel(string title)
        {
            Title = title;
        }

        public virtual void Dispose()
        {

        }
    }
}
