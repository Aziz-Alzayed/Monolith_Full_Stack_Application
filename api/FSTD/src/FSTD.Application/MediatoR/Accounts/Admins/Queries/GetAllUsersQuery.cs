using FSTD.Application.DTOs.Accounts.Admins;
using MediatR;

namespace FSTD.Application.MediatoR.Accounts.Admins.Queries
{
    public class GetAllUsersQuery : IRequest<List<UserFullInfoDto>>
    {
        public GetAllUsersQuery(string requestedBy)
        {
            RequestedBy = requestedBy;
        }

        public string RequestedBy { get; set; }
    }
}
