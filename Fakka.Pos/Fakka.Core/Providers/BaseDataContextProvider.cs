using System;
using System.Collections.Generic;
using Fakka.Core.Interfaces;
using Prism.Ioc;

namespace Fakka.Core.Providers
{
    public class BaseDataContextProvider : IDataContext
    {
        private Dictionary<string, object> _localContext;
        protected readonly IContainerProvider Container;
        public BaseDataContextProvider(IContainerProvider container)
        {
            Container = container;
            _localContext = new Dictionary<string, object>();
        }

        public void Register<TEntity, UEntity>()
            where TEntity : IBaseDataContext
            where UEntity : class , IBaseDataContext
        {
            var instance = Activator.CreateInstance(typeof(UEntity),
                new object[] { Container }) as UEntity;
            _localContext.Add(typeof(TEntity).Name, instance);

        }

        public TEntity Get<TEntity>() where TEntity : class, IBaseDataContext
        {
            _localContext.TryGetValue(typeof(TEntity).Name, out var entityContext);
            return entityContext as TEntity;
        }

    }

}
