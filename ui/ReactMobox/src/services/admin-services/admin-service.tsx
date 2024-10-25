/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import {
  AddNewUserByAdmin,
  IUserFullInfos,
} from "../../models/admin/admin-models";
import {
  deleteAuthRequest,
  getAuthRequest,
  postAuthRequest,
  putAuthRequest,
} from "../api-services/api-auth-service";

export const addUserRequestApi = async (
  newUser: AddNewUserByAdmin
): Promise<AxiosResponse<any, any>> => {
  try {
    return await postAuthRequest(`/Admin/AddNewUser`, newUser);
  } catch (error) {
    console.error("Error in addUserRequestApi", error);
    throw error;
  }
};

export const updateUserRequestApi = async (
  user: Partial<IUserFullInfos>
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(`/Admin/UpdateUser`, user);
  } catch (error) {
    console.error("Error in editUserRequestApi", error);
    throw error;
  }
};

export const deleteUserRequestApi = async (
  userId: string
): Promise<AxiosResponse<any, any>> => {
  try {
    return await deleteAuthRequest(`/Admin/DeleteUser/${userId}`);
  } catch (error) {
    console.error("Error in deleteUserRequest", error);
    throw error;
  }
};

export const promotAdminUserApi = async (
  userId: string
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(`/Admin/PromotAdminUser/${userId}`);
  } catch (error) {
    console.error("Error in promotAdminUser", error);
    throw error;
  }
};

export const demoteAdminUserApi = async (
  userId: string
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(`/Admin/DemoteAdminUser/${userId}`);
  } catch (error) {
    console.error("Error in demoteAdminUser", error);
    throw error;
  }
};

export const getAllUsersApi = async (): Promise<
  AxiosResponse<IUserFullInfos[]>
> => {
  try {
    return (await getAuthRequest(`/Admin/GetAllUser`)) || [];
  } catch (error) {
    console.error("Error in getAllUser", error);
    throw error;
  }
};
