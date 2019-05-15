using Microsoft.Extensions.DependencyInjection;
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
                .AddScoped<IPointsRewardService, PointsRewardService>()
                .AddScoped<IPartnerService, PartnerService>()
                .AddScoped<IMissionService, MissionService>()
                .AddScoped<IDriftPlaygroundService, DriftPlaygroundService>()
                .AddScoped<IGatheringService, GatheringService>()
                .AddScoped<IOffroadTrackService, OffroadTrackService>();
        }
    }
}
