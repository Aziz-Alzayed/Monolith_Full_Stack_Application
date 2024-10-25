import {
  TokenRefreshRequest,
  applyAuthTokenInterceptor,
  getBrowserLocalStorage,
  IAuthTokens,
} from "axios-jwt";
import axios from "axios";
import { apiURL } from "../../apiConfig";

export const axiosAuthInstance = axios.create({ baseURL: apiURL });
export const axiosInstance = axios.create({ baseURL: apiURL });

const requestRefresh: TokenRefreshRequest = async (
  refreshToken: string
): Promise<IAuthTokens | string> => {
  try {
    const response = await axios.post(`${apiURL}/auth/refresh_token`, {
      token: refreshToken,
    });
    return {
      accessToken: response.data.access_token,
      refreshToken: response.data.refresh_token,
    };
  } catch (e) {
    console.error(e);
    return {
      accessToken: "",
      refreshToken: "",
    };
  }
};

// Here we provide a function that returns the storage service
const getStorage = () => getBrowserLocalStorage();

applyAuthTokenInterceptor(axiosAuthInstance, {
  requestRefresh,
  header: "Authorization",
  headerPrefix: "Bearer ",
  getStorage,
});
