using System.Windows;

namespace Kysion.Extensions.Core.Contracts
{
    public interface IWindow
    {
        event RoutedEventHandler Loaded;

        double Height { get; set; }
        double Width { get; set; }

        void Show();

        void Close();
    }

}
