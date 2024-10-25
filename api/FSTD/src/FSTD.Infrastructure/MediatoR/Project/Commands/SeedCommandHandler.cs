using FSTD.Application.MediatoR.Project.Commands;
using FSTD.Application.MediatoR.Project.Repos;
using FSTD.Infrastructure.CommonServices.SeedServices;
using MediatR;


namespace FSTD.Infrastructure.MediatoR.Project.Commands
{
    public class SeedCommandHandler : IRequestHandler<SeedCommand>
    {
        private readonly ISeedCommandsRepo _seedCommandsRepo;
        private readonly IIdentitySeedService _identitySeedService;

        public SeedCommandHandler(ISeedCommandsRepo seedCommandsRepo, IIdentitySeedService identitySeedService)
        {
            _seedCommandsRepo = seedCommandsRepo;
            _identitySeedService = identitySeedService;
        }

        public async Task Handle(SeedCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _seedCommandsRepo.DeleteAllUsersAync();
                await Task.WhenAll
                    (
                    _identitySeedService.SeedRoles(),
                    _identitySeedService.SeedUser(),
                    _identitySeedService.SeedAdminUser(),
                    _identitySeedService.SeedSuperUser()
                      );
            }
            catch
            {
                throw;
            }
        }
    }
}
