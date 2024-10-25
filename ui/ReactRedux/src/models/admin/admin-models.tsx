import { AppRoles } from "../../auth/auth-services/role-management";

export interface IUserFullInfos {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  isEmailConfirmed: string;
  roles: AppRoles[];
}

export interface AddNewUserByAdmin {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  roles: AppRoles[];
  resetUrl: string;
}
