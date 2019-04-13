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
                .AddScoped<IUserTempService, UserTempService>()
                .AddScoped<IAdminUserService, AdminUserService>()
                .AddScoped<IPersonService, PersonService>()
                .AddScoped<ICityService, CityService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IGroupService, GroupService>()
                .AddScoped<IRewardService, RewardService>()
                .AddScoped<IVehicleService, VehicleService>()
                .AddScoped<IVehiclePhotoService, VehiclePhotoService>()
                .AddScoped<IVehicleCategoryService, VehicleCategoryService>()
                .AddScoped<IVehicleDriveService, VehicleDriveService>()
                .AddScoped<IVehicleTransmissionService, VehicleTransmissionService>()
                .AddScoped<IVehicleBodyService, VehicleBodyService>()
                .AddScoped<IEmailService, EmailService>()
                .AddScoped<IRatingLevelService, RatingLevelService>()
                .AddScoped<IPostService, PostService>()
                .AddScoped<IPostLikeService, PostLikeService>()
                .AddScoped<IPostCommentService, PostCommentService>()
                .AddScoped<IGroupPostService, GroupPostService>()
                .AddScoped<IUserSubscriptionService, UserSubscriptionService>();
        }
    }
}
