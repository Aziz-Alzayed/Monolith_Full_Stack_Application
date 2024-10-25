﻿using FluentValidation;
using FSTD.Application.Validations.Extentions;

namespace FSTD.Application.DTOs.Accounts.Admins
{
    public class UpdateUserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
    }
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(dto => dto.Id)
               .NotNull().WithMessage(errorMessage: "Id cannot be null.")
               .NotEmpty().WithMessage("Id is required.");

            RuleFor(dto => dto.FirstName)
               .NotEmpty().WithMessage("First name is required.")
               .NotEmpty().WithMessage("First name is required.")
               .Length(2, 50).WithMessage("First name must be between 2 and 50 characters.");

            RuleFor(dto => dto.LastName)
                .NotNull().WithMessage(errorMessage: "Last Name cannot be null.")
                .NotEmpty().WithMessage("Last name is required.")
                .Length(2, 50).WithMessage("Last name must be between 2 and 50 characters.");

            RuleFor(dto => dto.PhoneNumber)
                .NotNull().WithMessage("Phone Number cannot be null.")
                .NotEmpty().WithMessage("Phone number is required.")
                .IsPhoneNumber();


            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email is required.")
                .NotEmpty().WithMessage("Email is required.")
                .IsEmail();

            RuleFor(dto => dto.Roles)
               .Cascade(CascadeMode.Stop)
               .NotNull().WithMessage("Roles cannot be null.")
               .NotEmpty().WithMessage("At least one role is required.")
               .Must(roles => roles.All(role => !string.IsNullOrEmpty(role)))
               .WithMessage("Roles cannot contain an empty value.");

        }
    }
}