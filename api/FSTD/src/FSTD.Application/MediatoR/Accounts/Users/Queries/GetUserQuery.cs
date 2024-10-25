using FSTD.Application.DTOs.Accounts.Users;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Users.Queries
{
    public class GetUserQuery : IRequest<UserInfoDto>
    {
        public GetUserQuery(string userEmail)
        {
            UserEmail = userEmail;
        }

        public string UserEmail { get; set; }
    }
}
