/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import { makeAutoObservable, runInAction } from "mobx";
import {
  ForgetPasswordDto,
  ResetForgetPasswordDto,
  UpdateUserDetailsDto,
  UpdateUserEmailDto,
  UpdateUserPasswordDto,
} from "../../models/user-models/user-models";
import {
  updateUserDetailsApi,
  updateUserEmailApi,
  updateUserPasswordApi,
  sendVerificationEmailApi,
  verifyUserEmailApi,
  forgetPasswordApi,
  resetForgetPasswordApi,
} from "../../services/user-services/user-profile-service";
import { StoresResults } from "../stores-utils/stores-results";

class UserStore {
  userLoading: boolean = false;
  error: string | undefined = undefined;

  constructor() {
    makeAutoObservable(this);
  }

  private async updateUserInfo<T>(
    data: T,
    updateFunction: (data: T) => Promise<AxiosResponse<any>>
  ): Promise<StoresResults> {
    if (this.userLoading) return { passed: false };

    runInAction(() => {
      this.userLoading = true;
      this.error = undefined;
    });

    try {
      const response = await updateFunction(data);

      if (response.status === 200) {
        return { passed: true };
      } else {
        return {
          passed: false,
          statusCode: response.status,
          message: response.statusText,
        };
      }
    } catch (error) {
      console.error("Error", error);
      return { passed: false, message: String(error) };
    } finally {
      runInAction(() => {
        this.userLoading = false;
      });
    }
  }

  UpdateUserPassword = async (
    data: UpdateUserPasswordDto
  ): Promise<StoresResults> => {
    return this.updateUserInfo(data, updateUserPasswordApi);
  };

  UpdateUserEmail = async (
    data: UpdateUserEmailDto
  ): Promise<StoresResults> => {
    return this.updateUserInfo(data, updateUserEmailApi);
  };

  UpdateUserDetails = async (
    data: UpdateUserDetailsDto
  ): Promise<StoresResults> => {
    return this.updateUserInfo(data, updateUserDetailsApi);
  };

  SendVerificationEmail = async (
    userEmail: string,
    verificationUrl: string
  ): Promise<StoresResults> => {
    return this.updateUserInfo(
      { userEmail, verificationUrl },
      sendVerificationEmailApi
    );
  };

  VerifyUserEmail = async (
    userId: string,
    token: string
  ): Promise<StoresResults> => {
    return this.updateUserInfo({ userId, token }, verifyUserEmailApi);
  };

  ForgetPassword = async (
    forgetPasswordDto: ForgetPasswordDto
  ): Promise<StoresResults> => {
    return this.updateUserInfo(forgetPasswordDto, forgetPasswordApi);
  };

  ResetForgetPassword = async (
    resetForgetPasswordDto: ResetForgetPasswordDto
  ): Promise<StoresResults> => {
    return this.updateUserInfo(resetForgetPasswordDto, resetForgetPasswordApi);
  };
}

export default new UserStore();
