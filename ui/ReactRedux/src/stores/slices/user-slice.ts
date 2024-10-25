import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { AxiosResponse } from "axios";
import { HttpStatusCode } from "axios";
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

// Define the UserState interface
interface UserState {
  userLoading: boolean;
  error?: string;
}

const initialState: UserState = {
  userLoading: false,
  error: undefined,
};

// Utility function for making async updates with Redux Thunk
const updateUserInfo = async <T>(
  data: T,
  updateFunction: (data: T) => Promise<AxiosResponse<unknown>>
): Promise<StoresResults> => {
  try {
    const response = await updateFunction(data);

    if (response.status === HttpStatusCode.Ok || response.status === HttpStatusCode.Created) {
      return { passed: true, statusCode: response.status };
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
  }
};

// Thunks for each action
export const updateUserPassword = createAsyncThunk<StoresResults, UpdateUserPasswordDto>(
  "user/updateUserPassword",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, updateUserPasswordApi).catch(rejectWithValue);
  }
);

export const updateUserEmail = createAsyncThunk<StoresResults, UpdateUserEmailDto>(
  "user/updateUserEmail",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, updateUserEmailApi).catch(rejectWithValue);
  }
);

export const updateUserDetails = createAsyncThunk<StoresResults, UpdateUserDetailsDto>(
  "user/updateUserDetails",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, updateUserDetailsApi).catch(rejectWithValue);
  }
);

export const sendVerificationEmail = createAsyncThunk<StoresResults, { userEmail: string; verificationUrl: string }>(
  "user/sendVerificationEmail",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, sendVerificationEmailApi).catch(rejectWithValue);
  }
);

export const verifyUserEmail = createAsyncThunk<StoresResults, { userId: string; token: string }>(
  "user/verifyUserEmail",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, verifyUserEmailApi).catch(rejectWithValue);
  }
);

export const forgetPassword = createAsyncThunk<StoresResults, ForgetPasswordDto>(
  "user/forgetPassword",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, forgetPasswordApi).catch(rejectWithValue);
  }
);

export const resetForgetPassword = createAsyncThunk<StoresResults, ResetForgetPasswordDto>(
  "user/resetForgetPassword",
  async (data, { rejectWithValue }) => {
    return updateUserInfo(data, resetForgetPasswordApi).catch(rejectWithValue);
  }
);

// User slice to handle the state
const userSlice = createSlice({
  name: "user",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      // Update user password
      .addCase(updateUserPassword.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(updateUserPassword.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(updateUserPassword.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      })
      
      // Update user email
      .addCase(updateUserEmail.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(updateUserEmail.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(updateUserEmail.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      })

      // Update user details
      .addCase(updateUserDetails.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(updateUserDetails.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(updateUserDetails.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      })

      // Send verification email
      .addCase(sendVerificationEmail.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(sendVerificationEmail.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(sendVerificationEmail.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      })

      // Verify user email
      .addCase(verifyUserEmail.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(verifyUserEmail.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(verifyUserEmail.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      })

      // Forget password
      .addCase(forgetPassword.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(forgetPassword.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(forgetPassword.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      })

      // Reset forgotten password
      .addCase(resetForgetPassword.pending, (state) => {
        state.userLoading = true;
        state.error = undefined;
      })
      .addCase(resetForgetPassword.fulfilled, (state, action) => {
        state.userLoading = false;
        if (!action.payload.passed) {
          state.error = action.payload.message;
        }
      })
      .addCase(resetForgetPassword.rejected, (state, action) => {
        state.userLoading = false;
        state.error = action.error.message;
      });
  },
});

export default userSlice.reducer;
