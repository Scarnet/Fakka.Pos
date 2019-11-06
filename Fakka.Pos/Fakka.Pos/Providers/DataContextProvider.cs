using Fakka.Core.Business.Contexts;
using Fakka.Core.Business.Interfaces;
using Fakka.Core.Providers;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.Providers
{
    public class DataContextProvider : BaseDataContextProvider
    {
        public DataContextProvider(IContainerProvider container) : base(container)
        {
            Register<IAuthenticationContext, AuthenticationContext>();
            Register<IOdataContext, OdataContext>();
            Register<IGradeContext, GradeContext>();
            Register<ISalesTransactionContext, SalesTransactionContext>();
            Register<ISchoolContext, SchoolContext>();
            Register<IPosUserContext, PosUserContext>();
        }
    }
}
