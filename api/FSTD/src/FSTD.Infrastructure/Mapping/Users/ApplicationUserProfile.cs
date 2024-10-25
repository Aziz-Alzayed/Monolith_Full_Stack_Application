using AutoMapper;
using FSTD.Application.DTOs.Accounts.Users;
using FSTD.DataCore.Models.Users;

namespace FSTD.Infrastructure.Mapping.Users
{
    public class ApplicationUserProfile : Profile
    {
        public ApplicationUserProfile()
        {
            CreateMap<ApplicationUser, UserInfoDto>().ReverseMap();
        }
    }
}
