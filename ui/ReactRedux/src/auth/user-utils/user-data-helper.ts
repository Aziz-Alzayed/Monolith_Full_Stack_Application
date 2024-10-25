import SecureLS from "secure-ls";
import { IUserInfo } from "../../models/auth-models/auth-models";

const ls = new SecureLS({ encodingType: "aes", isCompression: false });

const USER_KEY = "UserApp";

export const saveUserData = (userData: IUserInfo) => {
  ls.set(USER_KEY, userData);
};

export const getUserData = (): IUserInfo | undefined => {
  return ls.get(USER_KEY) || undefined;
};

export const clearUserData = () => {
  ls.remove(USER_KEY);
};
