/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import {
  ForgetPasswordDto,
  ResendVerificationEmailDto,
  ResetForgetPasswordDto,
  UpdateUserDetailsDto,
  UpdateUserEmailDto,
  UpdateUserPasswordDto,
  VerifyEmailDto,
} from "../../models/user-models/user-models";
import {
  postAuthRequest,
  putAuthRequest,
} from "../api-services/api-auth-service";
import { putRequest } from "../api-services/api-service";

export const updateUserDetailsApi = async (
  updateUserDetailsDto: UpdateUserDetailsDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(
      `/User/updateUserDetails`,
      updateUserDetailsDto
    );
  } catch (error) {
    console.error("Error in updateUserDetails", error);
    throw error;
  }
};
export const updateUserEmailApi = async (
  updateUserEmailsDto: UpdateUserEmailDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(
      `/User/updateUserEmail`,
      updateUserEmailsDto
    );
  } catch (error) {
    console.error("Error in updateUserEmail", error);
    throw error;
  }
};

export const updateUserPasswordApi = async (
  updateUserPasswordDto: UpdateUserPasswordDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(
      `/User/UpdateUserPassword`,
      updateUserPasswordDto
    );
  } catch (error) {
    console.error("Error in updateUserPasswordApi", error);
    throw error;
  }
};

export const sendVerificationEmailApi = async (
  resendVerificationEmailDto: ResendVerificationEmailDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await postAuthRequest(
      `/User/ResendVerificationEmail`,
      resendVerificationEmailDto
    );
  } catch (error) {
    console.error("Error in sendVerificationEmailApi", error);
    throw error;
  }
};

export const verifyUserEmailApi = async (
  verifyEmailDto: VerifyEmailDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putRequest(`/User/VerifyUserEmail`, verifyEmailDto);
  } catch (error) {
    console.error("Error in verifyUserEmailApi", error);
    throw error;
  }
};

export const forgetPasswordApi = async (
  forgetPasswordDto: ForgetPasswordDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putRequest(`/User/ForgetPassword`, forgetPasswordDto);
  } catch (error) {
    console.error("Error in forgetPasswordApi", error);
    throw error;
  }
};

export const resetForgetPasswordApi = async (
  resetForgetPasswordDto: ResetForgetPasswordDto
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putRequest(
      `/User/ResetForgetPassword`,
      resetForgetPasswordDto
    );
  } catch (error) {
    console.error("Error in resetForgetPassword", error);
    throw error;
  }
};
