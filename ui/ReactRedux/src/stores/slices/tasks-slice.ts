import { createSlice, createAsyncThunk } from "@reduxjs/toolkit";
import { HttpStatusCode } from "axios";
import { TaskModel } from "../../models/user-models/task-models";
import { StoresResults } from "../stores-utils/stores-results";
import {
  addTaskApi,
  deleteTaskApi,
  getAllTasksApi,
  updateTaskApi,
} from "../../services/user-services/task-service";

// Define the TaskState interface
interface TaskState {
  taskMap: Record<string, TaskModel>;
  tasksLoading: boolean;
  error?: string;
}

const initialState: TaskState = {
  taskMap: {},
  tasksLoading: false,
  error: undefined,
};

// Load all tasks thunk with StoresResults
export const loadAllTasks = createAsyncThunk<StoresResults<TaskModel[]>, void>(
  "task/loadAllTasks",
  async (_, { rejectWithValue }) => {
    try {
      const response = await getAllTasksApi();
      return {
        passed: true,
        message: "Tasks loaded successfully",
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

// Add task thunk with StoresResults
export const addTask = createAsyncThunk<StoresResults<TaskModel>, Omit<TaskModel, "id">>(
  "task/addTask",
  async (taskData, { rejectWithValue }) => {
    try {
      const result = await addTaskApi(taskData);
      if (result.status === HttpStatusCode.Created || result.status === HttpStatusCode.Ok) {
        return {
          passed: true,
          message: "Task added successfully",
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

// Update task thunk with StoresResults
export const updateTask = createAsyncThunk<StoresResults<{ taskId: string; taskData: Partial<TaskModel> }>, { taskId: string; taskData: Partial<TaskModel> }>(
  "task/updateTask",
  async ({ taskId, taskData }, { rejectWithValue }) => {
    try {
      const result = await updateTaskApi(taskData);
      if (result.status === HttpStatusCode.Ok || result.status === HttpStatusCode.Accepted) {
        return {
          passed: true,
          message: "Task updated successfully",
          statusCode: result.status,
          entity: { taskId, taskData },
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

// Delete task thunk with StoresResults
export const deleteTask = createAsyncThunk<StoresResults<string>, string>(
  "task/deleteTask",
  async (taskId: string, { rejectWithValue }) => {
    try {
      const result = await deleteTaskApi(taskId);
      if (result.status === HttpStatusCode.Ok) {
        return {
          passed: true,
          message: "Task deleted successfully",
          statusCode: result.status,
          entity: taskId,
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

// Task slice with extra reducers for each case
const taskSlice = createSlice({
  name: "task",
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    // Load tasks
    builder.addCase(loadAllTasks.pending, (state) => {
      state.tasksLoading = true;
      state.error = undefined;
    });
    builder.addCase(loadAllTasks.fulfilled, (state, action) => {
      state.tasksLoading = false;
      if (action.payload.passed && action.payload.entity) {
        state.taskMap = action.payload.entity.reduce(
          (acc: Record<string, TaskModel>, task: TaskModel) => {
            acc[task.id] = task;
            return acc;
          },
          {}
        );
      }
    });
    builder.addCase(loadAllTasks.rejected, (state, action) => {
      state.tasksLoading = false;
      state.error = (action.payload as StoresResults)?.message;
    });

    // Add task
    builder.addCase(addTask.pending, (state) => {
      state.tasksLoading = true;
      state.error = undefined;
    });
    builder.addCase(addTask.fulfilled, (state, action) => {
      state.tasksLoading = false;
      if (action.payload.passed && action.payload.entity) {
        state.taskMap[action.payload.entity.id] = action.payload.entity;
      }
    });
    builder.addCase(addTask.rejected, (state, action) => {
      state.tasksLoading = false;
      state.error = (action.payload as StoresResults)?.message;
    });

    // Update task
    builder.addCase(updateTask.pending, (state) => {
      state.tasksLoading = true;
      state.error = undefined;
    });
    builder.addCase(updateTask.fulfilled, (state, action) => {
      state.tasksLoading = false;
      if (action.payload.passed && action.payload.entity) {
        const { taskId, taskData } = action.payload.entity;
        state.taskMap[taskId] = { ...state.taskMap[taskId], ...taskData };
      }
    });
    builder.addCase(updateTask.rejected, (state, action) => {
      state.tasksLoading = false;
      state.error = (action.payload as StoresResults)?.message;
    });

    // Delete task
    builder.addCase(deleteTask.pending, (state) => {
      state.tasksLoading = true;
      state.error = undefined;
    });
    builder.addCase(deleteTask.fulfilled, (state, action) => {
      state.tasksLoading = false;
      if (action.payload.passed && action.payload.entity) {
        delete state.taskMap[action.payload.entity];
      }
    });
    builder.addCase(deleteTask.rejected, (state, action) => {
      state.tasksLoading = false;
      state.error = (action.payload as StoresResults)?.message;
    });
  },
});

export default taskSlice.reducer;
