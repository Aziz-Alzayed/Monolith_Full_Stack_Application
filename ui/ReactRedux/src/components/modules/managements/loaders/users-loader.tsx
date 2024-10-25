import { useEffect } from "react";
import { Outlet } from "react-router-dom";
import { Spin } from "antd";
import { useDispatch, useSelector } from "react-redux";
import AdminRoleComponent from "../../../../auth/auth-wrappers/admin-role-components";
import { useAuth } from "../../../../auth/auth-provider/auth-provider";
import { loadAllUsers } from "../../../../stores/slices/admin-slice";
import { AppDispatch, RootState } from "../../../../stores/main-store";


const UsersLoader: React.FC = () => {
  const { user } = useAuth();
  const dispatch = useDispatch<AppDispatch>();
  const users = useSelector((state: RootState) => Object.values(state.admin.userMap));
  const usersLoading = useSelector((state: RootState) => state.admin.usersLoading);

  useEffect(() => {
    if (user && users.length === 0) {
      dispatch(loadAllUsers());
    }
  }, [user, users.length, dispatch]);

  return (
    <Spin tip="Load Users..." spinning={!users.length && usersLoading}>
      <Outlet />
    </Spin>
  );
};

const AdminUsersLoader = AdminRoleComponent(UsersLoader);
export default AdminUsersLoader;
