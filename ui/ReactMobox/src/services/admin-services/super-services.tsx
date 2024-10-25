/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import { putAuthRequest } from "../api-services/api-auth-service";

export const promotSuperUser = async (
  userId: string
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(`/Admin/PromotSuperUser/${userId}`);
  } catch (error) {
    console.error("Error in PromotSuperUser", error);
    throw error;
  }
};

export const demoteSuperUser = async (
  userId: string
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(`/Admin/DemoteSuperUser/${userId}`);
  } catch (error) {
    console.error("Error in demoteSuperUser", error);
    throw error;
  }
};
