/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import { axiosAuthInstance } from "../../auth/auth-services/auth-api";

export const postAuthRequest = async (
  routingPath: string,
  params?: any
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosAuthInstance.post(routingPath, params);
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const putAuthRequest = async (
  routingPath: string,
  params?: any
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosAuthInstance.put(routingPath, params);
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const deleteAuthRequest = async (
  routingPath: string,
  params?: any
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosAuthInstance.delete(routingPath, params);
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};

export const getAuthRequest = async (
  routingPath: string
): Promise<AxiosResponse<any, any>> => {
  try {
    const response = await axiosAuthInstance.get(routingPath);
    return response;
  } catch (error) {
    console.error(error);
    throw error;
  }
};
