using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ProfilesCalculator.Shared;

namespace ProfilesCalculator.MainModule
{
    public class MainModuleModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public MainModuleModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            _regionManager.RegisterViewWithRegion(RegionNames.ContentRegion, typeof(Views.Main));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}