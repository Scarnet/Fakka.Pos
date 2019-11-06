using AutoMapper;
using Fakka.Core.Business.Models;
using Fakka.Pos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.MappingProfiles
{
    public class StockItemsProfile : Profile
    {
        public StockItemsProfile()
        {

            CreateMap<StockItem, LineItem>()
                .ForMember(dest => dest.Quantity, opt => opt.Ignore())
                .ForMember(dest => dest.MaxQuantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ItemType, opt => opt.MapFrom(src => src.Type));

            CreateMap<OnlineOrderLineItem, TransactionItem>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => new Guid(src.Id)))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<TransactionItem, OnlineOrderLineItem>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ItemId.ToString()));

            CreateMap<LineItem, TransactionItem>()
                .ForMember(dest => dest.ItemId, opt => opt.MapFrom(src => new Guid(src.Id)))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .Include<OnlineOrderLineItem, TransactionItem>();

        }
    }
}
