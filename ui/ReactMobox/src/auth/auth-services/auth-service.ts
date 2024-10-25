import {
  setAuthTokens,
  clearAuthTokens,
  getAccessToken,
  getRefreshToken,
} from "axios-jwt";
import { jwtDecode } from "jwt-decode";
import { postAuthRequest } from "../../services/api-services/api-auth-service";
import { postRequest } from "../../services/api-services/api-service";
import {
  ILoginRequest,
  IUserInfo,
  TokenPayload,
} from "../../models/auth-models/auth-models";
import { clearUserData, saveUserData } from "../user-utils/user-data-helper";
import { AppRoles } from "./role-management";

export const login = async (params: ILoginRequest): Promise<IUserInfo> => {
  try {
    const response = await postRequest("/auth/login", params);

    setAuthTokens({
      accessToken: response.data.accessToken,
      refreshToken: response.data.refreshToken,
    });
    saveUserData(response.data.userInfo);
    // Return user data along with tokens
    return response.data.userInfo;
  } catch (error) {
    console.error("Error login:", error);
    throw error;
  }
};

export const logout = async (): Promise<void> => {
  try {
    clearUserData();
    await postAuthRequest("/auth/logout");
    clearAuthTokens();
  } catch (error) {
    console.error("Error out:", error);
    throw error;
  }
};

export const getRoles = async (): Promise<string[]> => {
  try {
    const accessToken = await getAccessTokenAsync();
    if (!accessToken) {
      console.error("Access token is missing or invalid");
      return [];
    }

    const decoded: TokenPayload = jwtDecode<TokenPayload>(accessToken);
    // Use correct key based on JWT structure
    const roles =
      decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

    // Ensure roles is in an array format and return it
    if (roles) {
      return Array.isArray(roles) ? roles : [roles];
    }

    console.warn("Roles not found in token");
    return [];
  } catch (error) {
    console.error("Error decoding token or fetching roles:", error);
    return [];
  }
};

export const isAuthenticated = async (): Promise<boolean> => {
  // Retrieve access token asynchronously
  const accessToken = await getAccessToken();

  // If access token exists, return true
  if (accessToken) {
    return true;
  } else {
    // If no access token, clear tokens and return false
    clearAuthTokens();
    clearUserData();
    return false;
  }
};

export const isAdmin = async (): Promise<boolean> => {
  const roles = await getRoles();
  return roles.includes(AppRoles.Admin);
};

export const isSuper = async (): Promise<boolean> => {
  const roles = await getRoles();
  return roles.includes(AppRoles.Super);
};

export const getAccessTokenAsync = async (): Promise<string | undefined> => {
  return await getAccessToken();
};

export const getRefreshTokenAsync = async (): Promise<string | undefined> => {
  return await getRefreshToken();
};
