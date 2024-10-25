using AutoMapper;
using FSTD.Application.DTOs.Accounts.Admins;
using FSTD.Application.MediatoR.Accounts.Admins.Commands;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.Application.MediatoR.Accounts.Users.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Infrastructure.MediatoR.Accounts.Auth.Services;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Admins.Commands
{
    public class AddNewUserCommandHandler : IRequestHandler<AddNewUserCommand, UserFullInfoDto>
    {
        private readonly IAdminCommandsRepo _adminCommandsRepo;
        private readonly IAdminQueriesRepo _adminQueriesRepo;
        private readonly IAccountService _accountService;
        private readonly IRolesService _rolesService;
        private readonly IMapper _mapper;
        private readonly IUserCommandsRepo _userCommandsRepo;
        private readonly IAccountEmailSender _accountEmailSender;

        public AddNewUserCommandHandler(
            IAdminCommandsRepo adminCommandsRepo,
            IAdminQueriesRepo adminQueriesRepo,
            IAccountService accountService,
            IRolesService rolesService,
            IMapper mapper,
            IUserCommandsRepo userCommandsRepo,
            IAccountEmailSender accountEmailSender)
        {
            _adminCommandsRepo = adminCommandsRepo;
            _adminQueriesRepo = adminQueriesRepo;
            _accountService = accountService;
            _rolesService = rolesService;
            _mapper = mapper;
            _userCommandsRepo = userCommandsRepo;
            _accountEmailSender = accountEmailSender;
        }

        public async Task<UserFullInfoDto> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedDto = request.AddNewUserDto;

                // Validate the roles
                var validatedRoles = _rolesService.ValidateRoles(requestedDto.Roles.ToList());
                if (!validatedRoles.Any())
                {
                    throw new UnauthorizedAccessException("One or more of the requested roles are not allowed.");
                }


                bool canAssign = await _accountService.CanAssignRolesAsync(request.AddedBy, validatedRoles.ToArray());
                if (!canAssign)
                {
                    throw new UnauthorizedAccessException("You do not have permission to assign one or more of the requested roles.");
                }

                var newApplicationUser = new ApplicationUser()
                {
                    FirstName = requestedDto.FirstName,
                    LastName = requestedDto.LastName,
                    UserName = requestedDto.Email,
                    PhoneNumber = requestedDto.PhoneNumber,
                    Email = requestedDto.Email,
                    CreatedOn = DateTime.UtcNow
                };

                await _adminCommandsRepo.AddNewUserAsync(newApplicationUser);
                await _adminCommandsRepo.AddRolesAsync(newApplicationUser, validatedRoles);
                var token = await _userCommandsRepo.GeneratePasswordResetTokenAsync(newApplicationUser);
                await _accountEmailSender.SendPasswordResetEmailAsync(requestedDto.ResetURL, token, newApplicationUser);

                var userWithRoles = await _adminQueriesRepo.GetlApplicationUserWithRolesAsync(newApplicationUser);
                return _mapper.Map<UserFullInfoDto>(userWithRoles);
            }
            catch
            {

                throw;
            }
        }
    }
}
