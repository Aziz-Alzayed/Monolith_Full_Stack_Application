import {
  createContext,
  useContext,
  useState,
  useEffect,
  FC,
  PropsWithChildren,
  useMemo,
} from "react";
import SecureLS from "secure-ls";
import { IUserInfo } from "../../models/auth-models/auth-models";

const ls = new SecureLS({ encodingType: "aes", isCompression: false });

const emptyUser = (): IUserInfo => ({
  id: "",
  firstName: "",
  lastName: "",
  email: "",
  emailConfirmed: false,
});

interface IUserContext {
  user: IUserInfo;
  saveUser: (userData: IUserInfo) => void;
  clearUser: () => void;
}

const UserContext = createContext<IUserContext>({
  user: emptyUser(),
  saveUser: () => {},
  clearUser: () => {},
});

export const useUser = () => useContext(UserContext);

export const UserProvider: FC<PropsWithChildren> = ({ children }) => {
  const [user, setUser] = useState<IUserInfo>(emptyUser());

  useEffect(() => {
    try {
      const storedUser = ls.get("user") || emptyUser();
      setUser(storedUser);
    } catch (error) {
      console.error("Error retrieving user data:", error);
      setUser(emptyUser());
    }
  }, []);

  const saveUser = (userData: IUserInfo) => {
    ls.set("user", userData);
    setUser(userData);
  };

  const clearUser = () => {
    ls.remove("user");
    setUser(emptyUser());
  };

  const contextValue = useMemo(() => ({ user, saveUser, clearUser }), [user]);

  return (
    <UserContext.Provider value={contextValue}>{children}</UserContext.Provider>
  );
};
