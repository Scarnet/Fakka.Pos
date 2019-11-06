using AutoMapper;
using Fakka.Core.Business.Models;
using Fakka.Pos.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fakka.Pos.MappingProfiles
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<OnlineOrder, TodayOnlineOrder>()
                .ForMember(dest => dest.HijriDurationFrom, opt => opt.MapFrom(src => TrimDateYear(src.HijriDurationFrom)))
                .ForMember(dest => dest.HijriDurationTo, opt => opt.MapFrom(src => TrimDateYear(src.HijriDurationTo)));
            ;
            CreateMap<ChildProfile, TodayOnlineOrder>()
                .ForMember(dest => dest.ChildName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ChildHasImage, opt => opt.MapFrom(src => src.HasImage));

            CreateMap<TransactionItem, OrderTransaction>();
        }

        private string TrimDateYear(string date)
        {
            string[] prts = date.Split('-');

            if (prts.Length < 3)
                return date;

            return $"{prts[1]}-{prts[2]}";

        }
    }
}
