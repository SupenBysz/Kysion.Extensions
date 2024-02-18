using Kysion.Extensions.Core.Singleton;
using Kysion.Extensions.Core.Models;
using Kysion.Extensions.Core.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Kysion.Extensions.Core.Services
{
    public class LoggerService
    {
        #region 单例
        private static class SingletonInstance
        {
            public static LoggerService INSTANCE = new();
        }

        public static LoggerService Instance
        {
            get => SingletonInstance.INSTANCE;
        }
        private LoggerService()
        {
            LogItems = new QueueLength<LogInfo>(length: KysionConfig.Instance.LogLength);
        }
        #endregion

        public QueueLength<LogInfo> LogItems { get; private set; }

        public event Action<LogInfo>? LogItemsAddHandle;

        public event Action<LogInfo>? LogItemsRemoveHandle;

        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

        public static ILogger<T> CreateLogger<T>(string categoryName)
        {
            return new Logger<T>(loggerFactory, categoryName);
        }
        /// <summary>
        /// Delegates to a new <see cref="ILogger"/> instance using the full name of the given type, created by the
        /// provided <see cref="ILoggerFactory"/>.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        protected class Logger<T> : ILogger<T>
        {
            private readonly ILogger _logger;
            private readonly string _categoryName;
            private readonly string _basePath;


            public Logger(ILoggerFactory factory, string categoryName)
            {
                _logger = factory.CreateLogger(categoryName);
                _categoryName = categoryName;
                _basePath = Directory.GetCurrentDirectory().Replace("\\", "/") + "/Logs/";

                if (!Directory.Exists(_basePath))
                    Directory.CreateDirectory(_basePath);
            }

            /// <summary>
            /// Writes a log entry.
            /// </summary>
            /// <param name="logLevel">Entry will be written on this level.</param>
            /// <param name="eventId">Id of the event.</param>
            /// <param name="state">The entry to be written. Can be also an object.</param>
            /// <param name="exception">The exception related to this entry.</param>
            /// <param name="formatter">Function to create a <see cref="string"/> message of the <paramref name="state"/> and <paramref name="exception"/>.</param>
            /// <typeparam name="TState">The type of the object to be written.</typeparam>
            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                _logger.Log(logLevel, eventId, state, exception, formatter);

                if (IsEnabled(logLevel))
                {
                    if (state != null && state.ToString() != null)
                    {
                        var logContent = state.ToString();

                        if (logContent != null)
                        {
                            if (exception != null)
                            {
                                var logMsg = new
                                {
                                    message = logContent,
                                    error = new
                                    {
                                        exception?.Source,
                                        exception?.Message,
                                        exception?.StackTrace
                                    }
                                };

                                logContent = JsonConvert.SerializeObject(logMsg);
                            }

                            lock(this)
                            {
                                try
                                {
                                    var log = new LogInfo
                                    {
                                        CreateAt = DateTime.Now.ToString("G"),
                                        Category = _categoryName,
                                        Level = logLevel,
                                        Content = logContent
                                    };

                                    var logStr = JsonConvert.SerializeObject(log);
#if DEBUG
                                    Debug.WriteLine(logStr);
#endif

                                    Instance.LogItemsAddHandle?.Invoke(log);
                                    var logPath = _basePath + DateTime.Now.ToString("yyyyMMddHH") + ".log";
                                    if (Instance.LogItems.Count + 1 > KysionConfig.Instance.LogLength)
                                        Instance.LogItemsRemoveHandle?.Invoke(log);
                                    Instance.LogItems.Enqueue(log);

                                    File.AppendAllText(logPath, logStr + Environment.NewLine, Encoding.UTF8);
                                }
                                catch (Exception)
                                {
                                    //
                                }
                                
                            }
                        }
                    }
                }
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return _logger.IsEnabled(logLevel);
            }

            IDisposable? ILogger.BeginScope<TState>(TState state)
            {
                return _logger?.BeginScope(state);
            }
        }
    }
}
