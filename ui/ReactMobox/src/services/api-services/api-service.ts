/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import { axiosInstance } from "../../auth/auth-services/auth-api";

export const postRequest = async (
  routingPath: string,
  params?: any
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosInstance.post(routingPath, params);
    return response;
  } catch (error) {
    console.error("Error in postRequest:", error);
    throw error;
  }
};

export const getRequest = async (
  routingPath: string
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosInstance.get(routingPath);
    return response;
  } catch (error) {
    console.error("Error in getRequest:", error);
    throw error;
  }
};

export const putRequest = async (
  routingPath: string,
  params?: any
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosInstance.put(routingPath, params);
    return response;
  } catch (error) {
    console.error("Error in putRequest:", error);
    throw error;
  }
};
