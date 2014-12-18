using System.Windows;
using GalaSoft.MvvmLight.Threading;

namespace LastWeekOnChannel9UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
