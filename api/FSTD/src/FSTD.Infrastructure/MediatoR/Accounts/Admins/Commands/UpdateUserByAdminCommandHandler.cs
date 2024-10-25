using FSTD.Application.MediatoR.Accounts.Admins.Commands;
using FSTD.Application.MediatoR.Accounts.Admins.Repos;
using FSTD.DataCore.Models.Users;
using FSTD.Exeptions.Models.HttpResponseExceptions;
using FSTD.Infrastructure.EmailServices;
using FSTD.Infrastructure.MediatoR.Accounts.Auth.Services;
using FSTD.Infrastructure.MediatoR.Accounts.Services;
using MediatR;

namespace FSTD.Infrastructure.MediatoR.Accounts.Admins.Commands
{
    public class UpdateUserByAdminCommandHandler : IRequestHandler<UpdateUserByAdminCommand>
    {
        IAdminCommandsRepo _adminCommandsRepo;
        IAccountService _accountService;
        IRolesService _rolesService;
        IEmailService _emailService;

        public UpdateUserByAdminCommandHandler(
            IAdminCommandsRepo adminCommandsRepo,
            IAccountService accountService,
            IRolesService rolesService,
            IEmailService emailService)
        {
            _adminCommandsRepo = adminCommandsRepo;
            _accountService = accountService;
            _rolesService = rolesService;
            _emailService = emailService;
        }

        public async Task Handle(UpdateUserByAdminCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var requestedDto = request.EditUserDto;
                var user = await _accountService.GetUserAsync(requestedDto.Id);
                if (user == null)
                    throw new NotFoundException("User has been not been found to update!");


                // Validate the roles
                var validatedRoles = _rolesService.ValidateRoles(requestedDto.Roles.ToList());
                if (!validatedRoles.Any())
                {
                    throw new UnauthorizedAccessException("One or more of the requested roles are not allowed.");
                }


                bool canAssign = await _accountService.CanAssignRolesAsync(request.UpdatedBy, validatedRoles.ToArray());
                if (!canAssign)
                {
                    throw new UnauthorizedAccessException("You do not have permission to assign one or more of the requested roles.");
                }


                user.FirstName = requestedDto.FirstName;
                user.LastName = requestedDto.LastName;
                user.PhoneNumber = requestedDto.PhoneNumber;
                user.Email = requestedDto.Email;
                user.ModifiedOn = DateTime.UtcNow;

                await _adminCommandsRepo.UpdateUserAsync(user);
                await _adminCommandsRepo.UpdateRolesAsync(user, validatedRoles);
                await SendNotificationEmail(user);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task SendNotificationEmail(ApplicationUser user)
        {
            // Prepare and send the notification email
            var emailSubject = "Your Account Has Been Updated";
            var emailBody = "Dear " + user.FirstName + ",\n\nYour account information has been successfully updated.";
            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);
        }
    }
}
