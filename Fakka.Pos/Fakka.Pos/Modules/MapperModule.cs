using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Prism.Ioc;

namespace Fakka.Pos.Modules
{
    public class MapperModule : BaseModule
    {
        public override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            try
            {
            base.RegisterTypes(containerRegistry);

            var config = new MapperConfiguration(cfg => {
                
                cfg.AddMaps(this.GetType().Assembly);
            });

            var mapper = config.CreateMapper();

            containerRegistry.RegisterInstance<IMapper>(mapper);

            }
            catch(Exception ex)
            {

            }

        }
    }
}
