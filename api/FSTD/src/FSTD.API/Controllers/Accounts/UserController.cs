using FSTD.Application.DTOs.Accounts.Auths;
using FSTD.Application.DTOs.Accounts.Users;
using FSTD.Application.MediatoR.Accounts.Users.Commands;
using FSTD.Application.MediatoR.Accounts.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FSTD.API.Controllers.Accounts
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesDefaultResponseType(typeof(UserInfoDto))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _mediator.Send(new GetUserQuery(User?.Identity?.Name ?? "")));
        }

        [HttpPut]
        [Route("updateUserEmail")]
        public async Task<IActionResult> UpdateEmail([FromBody] UpdateUserEmailDto updateUserEmailDto)
        {
            var validationResult = new UpdateUserEmailDtoValidator().Validate(updateUserEmailDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            await _mediator.Send(new UpdateUserEmailCommand(updateUserEmailDto, updateUserEmailDto.VerificationUrl, User?.Identity?.Name ?? ""));

            return Ok("User Email has been updated successfully, an email has been sent for verifying.");
        }
        [HttpPut("UpdateUserDetails")]
        public async Task<IActionResult> UpdateUserDetails([FromBody] UpdateUserDetailsDto updateUserDetails)
        {
            await _mediator.Send(new UpdateUserCommand(updateUserDetails, User?.Identity?.Name ?? ""));
            return Ok("User has been updated successfully.");
        }

        [HttpPut("UpdateUserPassword")]
        public async Task<IActionResult> UpdateUserPassword([FromBody] UpdateUserPasswordDto updateUserPassword)
        {
            var validationResult = new UpdateUserPasswordDtoValidator().Validate(updateUserPassword);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mediator.Send(new UpdateUserPasswordCommand(updateUserPassword, User?.Identity?.Name ?? ""));
            return Ok("User has been updated successfully.");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _mediator.Send(new DeleteUserCommand(User?.Identity?.Name ?? ""));
            return Ok("User has been deleted successfully.");
        }

        [HttpPost("ResendVerificationEmail")]
        public async Task<IActionResult> ResendVerificationEmail([FromBody] ResendVerificationEmailDto resendVerificationEmailDto)
        {
            var validationResult = new ResendVerificationEmailDtoValidator().Validate(resendVerificationEmailDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mediator.Send(new ResendVerificationEmailCommand(resendVerificationEmailDto.UserEmail, resendVerificationEmailDto.VerificationUrl));

            return Ok("Email has seen send successfully.");
        }

        [HttpPost("RequestResetPassword")]
        public async Task<IActionResult> RequestResetPassword([FromBody] RequestResetPasswordDto request)
        {
            var validationResult = new RequestResetPasswordDtoValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mediator.Send(new RequestResetPasswordCommand(request));

            return Ok("Password reset request processed. If the email exists, a reset link has been sent.");
        }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var validationResult = new ResetPasswordDtoValidator().Validate(resetPasswordDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            new ResetPasswordCommand(resetPasswordDto.Email, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            return Ok("Password reset successfully");
        }

        #region AllowAnonymous APIs

        [AllowAnonymous]
        [HttpPut("VerifyUserEmail")]
        public async Task<IActionResult> VerifyUserEmail([FromBody] VerifyEmailDto request)
        {
            var validationResult = new VerifyEmailDtoValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            await _mediator.Send(new VerifyEmailCommand(request.UserId, request.Token));

            return Ok("Email verified successfully.");
        }

        [AllowAnonymous]
        [HttpPut("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto request)
        {
            var validationResult = new ForgetPasswordDtoValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            await _mediator.Send(new ForgetPasswordCommand(request.Email, request.ResetUrl));

            return Ok("Email sent successfully.");
        }

        [AllowAnonymous]
        [HttpPut("ResetForgetPassword")]
        public async Task<IActionResult> ResetForgetPassword([FromBody] ResetForgetPasswordDto request)
        {
            var validationResult = new ResetForgetPasswordDtoValidator().Validate(request);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            await _mediator.Send(new ResetForgetPasswordCommand(request.Email, request.Token, request.NewPassword));

            return Ok("Password has been reset successfully.");
        }
        #endregion
    }
}
