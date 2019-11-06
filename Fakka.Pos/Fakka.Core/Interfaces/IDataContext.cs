using System;

namespace Fakka.Core.Interfaces
{
    public interface IDataContext
    {
        T Get<T>() where T : class, IBaseDataContext;

        //void Register<TEntity, UEntity>()
        //    where TEntity : IBaseDataContext
        //    where UEntity : class;
    }
    public interface IBaseDataContext // Marker Class
    {
    }
}