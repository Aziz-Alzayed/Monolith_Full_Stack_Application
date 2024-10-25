export interface UpdateUserDetailsDto {
  newFirstName?: string;
  newLastName?: string;
}

export interface UpdateUserEmailDto {
  newEmail: string;
  verificationUrl: string;
}

export interface UpdateUserPasswordDto {
  oldPassword: string;
  newPassword: string;
}

export interface ResendVerificationEmailDto {
  userEmail: string;
  verificationUrl: string;
}

export interface VerifyEmailDto {
  userId: string;
  token: string;
}

export interface RegistrationFormValues {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

export interface ForgetPasswordDto {
  email: string;
  resetUrl: string;
}

export interface ResetForgetPasswordDto {
  email: string;
  token: string;
  newPassword: string;
}
