using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using ProfilesCalculator.WPF.Views;
using System.Windows;

namespace ProfilesCalculator.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<MainModule.MainModuleModule>();
        }

    }
}
