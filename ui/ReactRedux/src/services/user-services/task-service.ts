/* eslint-disable @typescript-eslint/no-explicit-any */
import { AxiosResponse } from "axios";
import { TaskModel } from "../../models/user-models/task-models";
import {
  postAuthRequest,
  putAuthRequest,
  deleteAuthRequest,
  getAuthRequest,
} from "../api-services/api-auth-service";

// Add a new task
export const addTaskApi = async (
  taskData: Omit<TaskModel, "id">
): Promise<AxiosResponse<any, any>> => {
  try {
    console.log(taskData);
    return await postAuthRequest(`/tasks`, taskData);
  } catch (error) {
    console.error("Error in addTaskApi", error);
    throw error;
  }
};

// Get all tasks
export const getAllTasksApi = async (): Promise<AxiosResponse<any, any>> => {
  try {
    return await getAuthRequest(`/tasks/GetAllTasks`);
  } catch (error) {
    console.error("Error in getAllTasksApi", error);
    throw error;
  }
};

// Update an existing task
export const updateTaskApi = async (
  taskData: Partial<TaskModel>
): Promise<AxiosResponse<any, any>> => {
  try {
    return await putAuthRequest(`/tasks`, taskData);
  } catch (error) {
    console.error("Error in updateTaskApi", error);
    throw error;
  }
};

// Delete a task by ID
export const deleteTaskApi = async (
  taskId: string
): Promise<AxiosResponse<any, any>> => {
  try {
    return await deleteAuthRequest(`/tasks/DeleteTask/${taskId}`);
  } catch (error) {
    console.error("Error in deleteTaskApi", error);
    throw error;
  }
};
