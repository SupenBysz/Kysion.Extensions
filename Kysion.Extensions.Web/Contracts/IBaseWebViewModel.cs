using System.Windows;

namespace Kysion.Extensions.Web.Contracts
{
    public interface IBaseWebViewModel
    {
        void OnBack(object sender, RoutedEventArgs e);
        void OnHome(object sender, RoutedEventArgs e);
        void OnRefresh(object sender, RoutedEventArgs e);
        string GetDataFolder();
    }
}
