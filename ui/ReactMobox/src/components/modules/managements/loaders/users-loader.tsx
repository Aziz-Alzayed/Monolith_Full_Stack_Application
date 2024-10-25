import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import { Outlet } from "react-router-dom";
import { Spin } from "antd";
import usersStore from "../../../../stores/admin-stores/admin-store";
import AdminRoleComponent from "../../../../auth/auth-wrappers/admin-role-components";
import { useAuth } from "../../../../auth/auth-provider/auth-provider";

const UsersLoader: React.FC = observer(() => {
  const { user } = useAuth();
  const { users, loadAllUsers, usersLoading } = usersStore;

  useEffect(() => {
    if (user && !users.length) {
      loadAllUsers();
    }
  }, []);

  return (
    <Spin tip="Load Users..." spinning={!users.length && usersLoading}>
      <Outlet />
    </Spin>
  );
});

const AdminUsersLoader = AdminRoleComponent(UsersLoader);
export default AdminUsersLoader;

