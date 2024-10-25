export interface ILoginRequest {
  email: string;
  password: string;
}

export interface TokenPayload {
  "http://schemas.microsoft.com/ws/2008/06/identity/claims/role":
    | string
    | string[];
  exp: number;
  // Include other properties from your token payload as needed
}

export interface IUser {
  userInfo: IUserInfo;
  AccessToken: string;
  RefreshToken: string;
}

export interface IAuthContextType {
  user: IUserInfo | undefined;
  roles: string[];
  handleLogin: (loginDetails: ILoginRequest) => Promise<void>;
  handleLogout: () => void;
  updateUser: (newUserData: IUserInfo) => void;
}

export interface IUserInfo {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  emailConfirmed: boolean;
}
