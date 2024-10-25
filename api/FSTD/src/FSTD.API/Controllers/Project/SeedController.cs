using FSTD.API.ApiAttributes;
using FSTD.Application.MediatoR.Project.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FSTD.API.Controllers.Project
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SeedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ServiceFilter(typeof(ApiKeyAuthAttribute))]
        public async Task<IActionResult> Post()
        {
            await _mediator.Send(new SeedCommand());
            return Ok();
        }
    }
}
