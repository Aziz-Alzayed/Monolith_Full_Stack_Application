import { HttpStatusCode } from "axios";
import { makeAutoObservable, observable, runInAction, set } from "mobx";
import {
  AddNewUserByAdmin,
  IUserFullInfos,
} from "../../models/admin/admin-models";
import {
  addUserRequestApi,
  deleteUserRequestApi,
  getAllUsersApi,
  updateUserRequestApi,
} from "../../services/admin-services/admin-service";
import { StoresResults } from "../stores-utils/stores-results";

class AdminStore {
  userMap: Record<string, IUserFullInfos> = observable.object({});
  usersLoading: boolean = false;
  error: string | undefined = undefined;

  constructor() {
    makeAutoObservable(this);
  }

  get users() {
    return Object.values(this.userMap) || [];
  }

  loadAllUsers = async () => {
    if (this.usersLoading) {
      return;
    }

    runInAction(() => {
      this.usersLoading = true;
      this.error = undefined;
    });

    try {
      const response = await getAllUsersApi();
      const usersData = response.data;

      runInAction(() => {
        this.userMap = usersData.reduce(
          (acc: Record<string, IUserFullInfos>, user: IUserFullInfos) => {
            acc[user.id] = user;
            return acc;
          },
          {}
        );
      });
    } catch (error) {
      console.error("Error loading users", error);
      runInAction(() => {
        this.error = error instanceof Error ? error.message : String(error);
      });
    } finally {
      runInAction(() => {
        this.usersLoading = false;
      });
    }
  };

  addUser = async (userData: AddNewUserByAdmin): Promise<StoresResults> => {
    runInAction(() => {
      this.usersLoading = true;
      this.error = undefined;
    });

    try {
      const result = await addUserRequestApi(userData);
      if (
        result.status === HttpStatusCode.Ok ||
        result.status === HttpStatusCode.Created
      ) {
        const newUser = (await result.data) as IUserFullInfos;

        runInAction(() => {
          if (!this.userMap[newUser.id])
            set(this.userMap, newUser.id, {
              ...this.userMap[newUser.id],
              ...newUser,
            });
        });

        return {
          passed: true,
          message: "User added successfully",
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
      console.error("Error adding user", error);
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
        this.usersLoading = false;
      });
    }
  };

  updateUser = async (
    userId: string,
    userData: Partial<IUserFullInfos>
  ): Promise<StoresResults> => {
    if (this.usersLoading) {
      return {
        passed: false,
        message: "Wait while users are loading.",
        statusCode: HttpStatusCode.Locked,
      };
    }

    runInAction(() => {
      this.usersLoading = true;
      this.error = undefined;
    });

    try {
      const result = await updateUserRequestApi(userData);
      if (
        [HttpStatusCode.Ok, HttpStatusCode.Accepted].includes(result.status)
      ) {
        runInAction(() => {
          Object.assign(this.userMap[userId], userData as IUserFullInfos);
        });

        return {
          passed: true,
          message: "User updated successfully",
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
      console.error("Error updating user", error);
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
        this.usersLoading = false;
      });
    }
  };

  deleteUser = async (userId: string): Promise<StoresResults> => {
    if (this.usersLoading) {
      return {
        passed: false,
        message: "Wait while users loading is completed!",
        statusCode: HttpStatusCode.Locked,
      };
    }

    runInAction(() => {
      this.usersLoading = true;
      this.error = undefined;
    });

    try {
      const result = await deleteUserRequestApi(userId);
      if (result.status === HttpStatusCode.Ok) {
        runInAction(() => {
          delete this.userMap[userId]; // Correctly update userMap by deleting the user
        });
        return {
          passed: true,
          message: "User deleted successfully",
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
      console.error("Error deleting user", error);
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
        this.usersLoading = false;
      });
    }
  };
}

export default new AdminStore();
