using AutoMapper;
using FSTD.Application.DTOs.Accounts.Users;
using FSTD.Application.MediatoR.Accounts.Users.Queries;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Users.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserInfoDto>
    {
        IUserQueriesRepo _usersRepository;
        IMapper _mapper;

        public GetUserQueryHandler(IUserQueriesRepo usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<UserInfoDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _usersRepository.GetUserByEmailAsync(request.UserEmail);

                if (user == null)
                {
                    throw new NotFoundException("User with the following email has not been found!");
                }
                return _mapper.Map<UserInfoDto>(user);
            }
            catch
            {

                throw;
            }
        }
    }
}
