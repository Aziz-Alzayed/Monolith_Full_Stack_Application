import { makeAutoObservable, observable, runInAction, set } from "mobx";
import { HttpStatusCode } from "axios";
import { TaskModel } from "../../models/user-models/task-models";
import { StoresResults } from "../stores-utils/stores-results";
import {
  addTaskApi,
  deleteTaskApi,
  getAllTasksApi,
  updateTaskApi,
} from "../../services/user-services/task-service";

class TaskStore {
  taskMap: Record<string, TaskModel> = observable.object({});
  tasksLoading: boolean = false;
  error: string | undefined = undefined;

  constructor() {
    makeAutoObservable(this);
  }

  // Getter to retrieve all tasks as an array
  get tasks() {
    return Object.values(this.taskMap) || [];
  }

  // Load all tasks
  loadAllTasks = async () => {
    if (this.tasksLoading) {
      return;
    }

    runInAction(() => {
      this.tasksLoading = true;
      this.error = undefined;
    });

    try {
      const response = await getAllTasksApi();
      const tasksData = response.data;
      runInAction(() => {
        this.taskMap = tasksData.reduce(
          (acc: Record<string, TaskModel>, task: TaskModel) => {
            acc[task.id] = task;
            return acc;
          },
          {}
        );
      });
    } catch (error) {
      console.error("Error loading tasks", error);
      runInAction(() => {
        this.error = error instanceof Error ? error.message : String(error);
      });
    } finally {
      runInAction(() => {
        this.tasksLoading = false;
      });
    }
  };

  // Add a new task
  addTask = async (taskData: Omit<TaskModel, "id">): Promise<StoresResults> => {
    runInAction(() => {
      this.tasksLoading = true;
      this.error = undefined;
    });

    try {
      const result = await addTaskApi(taskData);
      if (
        result.status === HttpStatusCode.Created ||
        result.status === HttpStatusCode.Ok
      ) {
        const newTask = result.data as TaskModel;

        runInAction(() => {
          set(this.taskMap, newTask.id, {
            ...this.taskMap[newTask.id],
            ...newTask,
          });
        });

        return {
          passed: true,
          message: "Task added successfully",
          statusCode: result.status,
        };
      } else {
        return {
          passed: false,
          message: result.statusText,
          statusCode: result.status,
        };
      }
    } catch (error) {
      console.error("Error adding task", error);
      const errorMessage =
        error instanceof Error ? error.message : String(error);
      runInAction(() => {
        this.error = errorMessage;
      });
      return {
        passed: false,
        message: errorMessage,
        statusCode: HttpStatusCode.InternalServerError,
      };
    } finally {
      runInAction(() => {
        this.tasksLoading = false;
      });
    }
  };

  // Update an existing task
  updateTask = async (
    taskId: string,
    taskData: Partial<TaskModel>
  ): Promise<StoresResults> => {
    if (this.tasksLoading) {
      return {
        passed: false,
        message: "Wait while tasks are loading.",
        statusCode: HttpStatusCode.Locked,
      };
    }

    runInAction(() => {
      this.tasksLoading = true;
      this.error = undefined;
    });

    try {
      const result = await updateTaskApi(taskData);
      if (
        [HttpStatusCode.Ok, HttpStatusCode.Accepted].includes(result.status)
      ) {
        runInAction(() => {
          Object.assign(this.taskMap[taskId], taskData);
        });

        return {
          passed: true,
          message: "Task updated successfully",
          statusCode: result.status,
        };
      } else {
        return {
          passed: false,
          message: result.statusText,
          statusCode: result.status,
        };
      }
    } catch (error) {
      console.error("Error updating task", error);
      const errorMessage =
        error instanceof Error ? error.message : String(error);
      runInAction(() => {
        this.error = errorMessage;
      });
      return {
        passed: false,
        message: errorMessage,
        statusCode: HttpStatusCode.InternalServerError,
      };
    } finally {
      runInAction(() => {
        this.tasksLoading = false;
      });
    }
  };

  // Delete a task
  deleteTask = async (taskId: string): Promise<StoresResults> => {
    if (this.tasksLoading) {
      return {
        passed: false,
        message: "Wait while tasks are loading.",
        statusCode: HttpStatusCode.Locked,
      };
    }

    runInAction(() => {
      this.tasksLoading = true;
      this.error = undefined;
    });

    try {
      const result = await deleteTaskApi(taskId);
      if (result.status === HttpStatusCode.Ok) {
        runInAction(() => {
          delete this.taskMap[taskId];
        });

        return {
          passed: true,
          message: "Task deleted successfully",
          statusCode: result.status,
        };
      } else {
        return {
          passed: false,
          message: result.statusText,
          statusCode: result.status,
        };
      }
    } catch (error) {
      console.error("Error deleting task", error);
      const errorMessage =
        error instanceof Error ? error.message : String(error);
      runInAction(() => {
        this.error = errorMessage;
      });
      return {
        passed: false,
        message: errorMessage,
        statusCode: HttpStatusCode.InternalServerError,
      };
    } finally {
      runInAction(() => {
        this.tasksLoading = false;
      });
    }
  };
}

export default new TaskStore();
