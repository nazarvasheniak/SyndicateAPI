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
                .AddScoped<IUserSessionService, UserSessionService>()
                .AddScoped<IUserTempService, UserTempService>()
                .AddScoped<IAdminUserService, AdminUserService>()
                .AddScoped<IPersonService, PersonService>()
                .AddScoped<ICityService, CityService>()
                .AddScoped<IFileService, FileService>()
                .AddScoped<IGroupService, GroupService>()
                .AddScoped<IGroupSubscriptionService, GroupSubscriptionService>()
                .AddScoped<IGroupMemberService, GroupMemberService>()
                .AddScoped<IGroupModeratorService, GroupModeratorService>()
                .AddScoped<IGroupCreatorService, GroupCreatorService>()
                .AddScoped<IGroupJoinRequestService, GroupJoinRequestService>()
                .AddScoped<IRewardService, RewardService>()
                .AddScoped<IAwardService, AwardService>()
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
                .AddScoped<IPostCommentLikeService, PostCommentLikeService>()
                .AddScoped<IGroupPostService, GroupPostService>()
                .AddScoped<IUserSubscriptionService, UserSubscriptionService>()
                .AddScoped<IPartnerService, PartnerService>()
                .AddScoped<IPartnerProductService, PartnerProductService>()
                .AddScoped<IMapPointService, MapPointService>()
                .AddScoped<IDialogService, DialogService>()
                .AddScoped<IDialogMessageService, DialogMessageService>()
                .AddScoped<IPointsCodeService, PointsCodeService>()
                .AddScoped<IStartRewardService, StartRewardService>();
        }
    }
}
