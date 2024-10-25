import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import {
  addUserRequestApi,
  deleteUserRequestApi,
  getAllUsersApi,
  updateUserRequestApi,
} from "../../services/admin-services/admin-service";
import { HttpStatusCode } from "axios";
import { IUserFullInfos, AddNewUserByAdmin } from "../../models/admin/admin-models";
import { StoresResults } from "../stores-utils/stores-results";

// Define the AdminState interface
interface AdminState {
  userMap: Record<string, IUserFullInfos>;
  usersLoading: boolean;
  error?: string;
}

const initialState: AdminState = {
  userMap: {},
  usersLoading: false,
  error: undefined,
};

// Load all users thunk with StoresResults
export const loadAllUsers = createAsyncThunk<StoresResults<IUserFullInfos[]>, void>(
  "admin/loadAllUsers",
  async (_, { rejectWithValue }) => {
    try {
      const response = await getAllUsersApi();
      return {
        passed: true,
        message: "Users loaded successfully",
        statusCode: response.status,
        entity: response.data,
      };
    } catch (error) {
      return rejectWithValue({
        passed: false,
        message: error instanceof Error ? error.message : String(error),
        statusCode: HttpStatusCode.InternalServerError,
      });
    }
  }
);

// Add user thunk with StoresResults
export const addUser = createAsyncThunk<StoresResults<IUserFullInfos>, AddNewUserByAdmin>(
  "admin/addUser",
  async (userData, { rejectWithValue }) => {
    try {
      const result = await addUserRequestApi(userData);
      if (result.status === HttpStatusCode.Ok || result.status === HttpStatusCode.Created) {
        return {
          passed: true,
          message: "User added successfully",
          statusCode: result.status,
          entity: result.data,
        };
      }
      return rejectWithValue({
        passed: false,
        message: result.statusText,
        statusCode: result.status,
      });
    } catch (error) {
      return rejectWithValue({
        passed: false,
        message: error instanceof Error ? error.message : String(error),
        statusCode: HttpStatusCode.InternalServerError,
      });
    }
  }
);

// Update user thunk with StoresResults
export const updateUser = createAsyncThunk<StoresResults<{ userId: string; userData: Partial<IUserFullInfos> }>, { userId: string; userData: Partial<IUserFullInfos> }>(
  "admin/updateUser",
  async ({ userId, userData }, { rejectWithValue }) => {
    try {
      const result = await updateUserRequestApi(userData);
      if (result.status === HttpStatusCode.Ok || result.status === HttpStatusCode.Accepted) {
        return {
          passed: true,
          message: "User updated successfully",
          statusCode: result.status,
          entity: { userId, userData },
        };
      }
      return rejectWithValue({
        passed: false,
        message: result.statusText,
        statusCode: result.status,
      });
    } catch (error) {
      return rejectWithValue({
        passed: false,
        message: error instanceof Error ? error.message : String(error),
        statusCode: HttpStatusCode.InternalServerError,
      });
    }
  }
);

// Delete user thunk with StoresResults
export const deleteUser = createAsyncThunk<StoresResults<string>, string>(
  "admin/deleteUser",
  async (userId: string, { rejectWithValue }) => {
    try {
      const result = await deleteUserRequestApi(userId);
      if (result.status === HttpStatusCode.Ok) {
        return {
          passed: true,
          message: "User deleted successfully",
          statusCode: result.status,
          entity: userId,
        };
      }
      return rejectWithValue({
        passed: false,
        message: result.statusText,
        statusCode: result.status,
      });
    } catch (error) {
      return rejectWithValue({
        passed: false,
        message: error instanceof Error ? error.message : "Unknown error",
        statusCode: HttpStatusCode.InternalServerError,
      });
    }
  }
);

// Admin slice with extra reducers for each case
const adminSlice = createSlice({
  name: "admin",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    // Load users
    builder.addCase(loadAllUsers.pending, (state) => {
      state.usersLoading = true;
      state.error = undefined;
    });
    builder.addCase(loadAllUsers.fulfilled, (state, action) => {
      state.usersLoading = false;
      if (action.payload.passed && action.payload.entity) {
        state.userMap = action.payload.entity.reduce(
          (acc: Record<string, IUserFullInfos>, user: IUserFullInfos) => {
            acc[user.id] = user;
            return acc;
          },
          {}
        );
      }
    });
    builder.addCase(loadAllUsers.rejected, (state, action) => {
      state.usersLoading = false;
      state.error = (action.payload as StoresResults)?.message; // Typecast action.payload
    });

    // Add user
    builder.addCase(addUser.pending, (state) => {
      state.usersLoading = true;
      state.error = undefined;
    });
    builder.addCase(addUser.fulfilled, (state, action) => {
      state.usersLoading = false;
      if (action.payload.passed && action.payload.entity) {
        state.userMap[action.payload.entity.id] = action.payload.entity;
      }
    });
    builder.addCase(addUser.rejected, (state, action) => {
      state.usersLoading = false;
      state.error = (action.payload as StoresResults)?.message; // Typecast action.payload
    });

    // Update user
    builder.addCase(updateUser.pending, (state) => {
      state.usersLoading = true;
      state.error = undefined;
    });
    builder.addCase(updateUser.fulfilled, (state, action) => {
      state.usersLoading = false;
      if (action.payload.passed && action.payload.entity) {
        const { userId, userData } = action.payload.entity;
        state.userMap[userId] = { ...state.userMap[userId], ...userData };
      }
    });
    builder.addCase(updateUser.rejected, (state, action) => {
      state.usersLoading = false;
      state.error = (action.payload as StoresResults)?.message; // Typecast action.payload
    });

    // Delete user
    builder.addCase(deleteUser.pending, (state) => {
      state.usersLoading = true;
      state.error = undefined;
    });
    builder.addCase(deleteUser.fulfilled, (state, action) => {
      state.usersLoading = false;
      if (action.payload.passed && action.payload.entity) {
        delete state.userMap[action.payload.entity];
      }
    });
    builder.addCase(deleteUser.rejected, (state, action) => {
      state.usersLoading = false;
      state.error = (action.payload as StoresResults)?.message; // Typecast action.payload
    });
  },
});

export default adminSlice.reducer;
