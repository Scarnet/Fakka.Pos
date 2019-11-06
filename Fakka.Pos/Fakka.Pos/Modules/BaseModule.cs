using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.Modules
{
    public class BaseModule : IModule
    {
        public virtual void OnInitialized(IContainerProvider containerProvider)
        {
            
        }

        public virtual void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
