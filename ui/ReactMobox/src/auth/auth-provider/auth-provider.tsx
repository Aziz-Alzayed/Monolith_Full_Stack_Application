import { AxiosError } from "axios";
import {
  createContext,
  useContext,
  useState,
  PropsWithChildren,
  FC,
  useEffect,
} from "react";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../components/notification/notification-components";
import {
  ILoginRequest,
  IUserInfo,
  IAuthContextType,
} from "../../models/auth-models/auth-models";
import {
  login,
  logout,
  getRoles,
  isAuthenticated,
} from "../auth-services/auth-service";
import { getUserData, saveUserData } from "../user-utils/user-data-helper";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";

export const AuthContext = createContext<IAuthContextType>({
  user: undefined,
  roles: [],
  handleLogin: async () => {
    /* default empty implementation */
  },
  handleLogout: () => {
    /* default empty implementation */
  },
  updateUser: () => {},
});

export const useAuth = (): IAuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

const AuthProvider: FC<PropsWithChildren> = ({ children }) => {
  const navigateWithLanguage = useLanguageAwareNavigate();
  const [roles, setRoles] = useState<string[]>([]);
  const [user, setUser] = useState<IUserInfo | undefined>(getUserData());

  const updateUser = (newUserData: IUserInfo) => {
    setUser(newUserData);
    saveUserData(newUserData);
  };

  // Function to handle user login
  const handleLogin = async (loginDetails: ILoginRequest) => {
    try {
      const userData = await login(loginDetails);
      setUser(userData);
      const fetchedRoles = await getRoles();
      setRoles(fetchedRoles); // Set roles from the decoded token
      if (userData) SuccessNotification("Login succeed!");
    } catch (error) {
      console.error("Failed to fetch roles after login:", error);
      setUser(undefined); // Invalidate the session if roles cannot be fetched
      setRoles([]); // Clear roles
      ErrorNotification("Login failed! Unable to fetch roles.");
    }
  };

  // Function to handle user logout
  const handleLogout = async () => {
    try {
      await logout(); // Clears the auth tokens
      setUser(undefined); // Reset user state
      setRoles([]); // Reset roles state
      navigateWithLanguage(RoutePaths.home);
    } catch (error) {
      ErrorNotification(
        "Logout failed!",
        (error as unknown as AxiosError).message
      );
    } finally {
      setRoles([]); // Reset roles state
    }
  };

  useEffect(() => {
    const initializeAuth = async () => {
      if (await isAuthenticated()) {
        const userData = getUserData();
        setUser(userData); // Initialize user data
        try {
          const fetchedRoles = await getRoles();

          setRoles(fetchedRoles); // Set roles if fetched successfully
        } catch (error) {
          console.error("Failed to fetch roles:", error);
          setUser(undefined); // Invalidate the session if roles cannot be fetched
          setRoles([]); // Clear roles
          ErrorNotification("Session invalid! Unable to fetch roles.");
          navigateWithLanguage(RoutePaths.login);
        }
      }
    };
    initializeAuth();
  }, []);

  // Provide the authentication context value to child components
  return (
    <AuthContext.Provider
      value={{ user, roles, handleLogin, handleLogout, updateUser }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export default AuthProvider;
