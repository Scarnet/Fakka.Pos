using Prism.Ioc;
using Prism.Modularity;


namespace Fakka.Core.Modules
{
    public abstract class BaseModule : IModule
    {
        protected BaseModule()
        {
        }
        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
        }
        public abstract void RegisterTypes(IContainerRegistry containerRegistry);

    }
}
