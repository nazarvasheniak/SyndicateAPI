﻿using Microsoft.Extensions.DependencyInjection;
using SyndicateAPI.BusinessLogic.Interfaces;
using SyndicateAPI.BusinessLogic.Services;

namespace SyndicateAPI.BusinessLogic
{
    public static class Installer
    {
        public static void AddBuisnessServices(this IServiceCollection container)
        {
            container
                .AddScoped<IUserService, UserService>()
                .AddScoped<IPersonService, PersonService>()
                .AddScoped<ICityService, CityService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IGroupService, GroupService>()
                .AddScoped<IRewardService, RewardService>()
                .AddScoped<IVehicleClassService, VehicleClassService>()
                .AddScoped<IVehicleCategoryService, VehicleCategoryService>()
                .AddScoped<IVehicleService, VehicleService>();
        }
    }
}