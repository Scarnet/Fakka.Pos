using System;
using System.Collections.Generic;
using System.Text;
using Fakka.Core.Business.Interfaces;
using Prism.Ioc;
using Fakka.Core.Business.Contexts;

namespace Fakka.Pos.Modules
{
    public class ContextsModule : BaseModule
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            base.RegisterTypes(containerRegistry);
            containerRegistry.Register<IAuthenticationContext, AuthenticationContext>();
            containerRegistry.Register<IOdataContext, OdataContext>();
            containerRegistry.Register<ISalesTransactionContext, SalesTransactionContext>();
            containerRegistry.Register<ISchoolContext, SchoolContext>();
            containerRegistry.Register<IPosUserContext, PosUserContext>();
        }
    }
}
