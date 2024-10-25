using AutoMapper;
using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.MediatoR.Accounts.Admins.Queries;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Admins.Queries
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserFullInfoDto>>
    {
        private readonly IAccountService _accountService;
        private readonly IAdminQueriesRepo _adminQueriesRepo;
        private readonly IMapper _mapper;
        public GetAllUsersQueryHandler(IAccountService accountService, IAdminQueriesRepo adminQueriesRepo, IMapper mapper)
        {
            _accountService = accountService;
            _adminQueriesRepo = adminQueriesRepo;
            _mapper = mapper;
        }

        public async Task<List<UserFullInfoDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var hasPermision = await _accountService.CheckPermissionsIsAdminOrSuperAsync(request.RequestedBy);
                if (!hasPermision)
                    throw new UnauthorizedAccessException();

                else
                {
                    var listOfUserWithRoles = await _adminQueriesRepo.GetAllApplicationUsersWithRolesAsync();
                    return _mapper.Map<List<UserFullInfoDto>>(listOfUserWithRoles);
                }

                throw new NotImplementedException();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
