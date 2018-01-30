using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FarmworkersWebAPI.Entities;
using FarmworkersWebAPI.ViewModels;

namespace FarmworkersWebAPI
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                // mapping for FarmDto
                cfg.CreateMap<Farm, FarmInfoDTO>();
                cfg.CreateMap<Farm, FarmDTO>()
                .ForMember(
                    d => d.NumberOfFarmWorkers,
                    o => o.MapFrom(s =>
                        s.UserFarms.Where(u =>
                            u.IsLatest == 1 && u.User.UserType == "Farm Worker" && u.User.IsActive == "1"
                        ).Count()
                    )
                ).ForMember(
                    d => d.FarmOwners,
                    o => o.MapFrom(s =>
                        s.UserFarms.Where(u =>
                            u.IsLatest == 1 && u.User.UserType == "Farm Owner" && u.User.IsActive == "1"
                        ).Select(u => u.User)
                    )
                );
                //mapping for LoginCredentialDTO
                cfg.CreateMap<LoginCredential, LoginCredentialDTO>();
                //mapping for UserDTO
                cfg.CreateMap<User, UserDTO>();
                cfg.CreateMap<UserFarm, UserFarmDTO>();
                cfg.CreateMap<User, UserWithFarmDTO>()
                .ForMember(
                    d => d.CurrentFarm,
                    o => o.MapFrom(s =>
                        s.UserFarms
                        .Where(x => x.IsLatest == 1)
                        .Select(x => x.Farm )
                        .FirstOrDefault()
                    )
                 );
                cfg.CreateMap<UserCommunicationPreference, UserCommunicationPreferenceDTO>();
            });
        }
    }
}