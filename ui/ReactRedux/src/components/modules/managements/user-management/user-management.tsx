import { useEffect, useState } from "react";
import { Table, Button, Tag, Popconfirm } from "antd";
import {
  CheckCircleOutlined,
  ExclamationCircleOutlined,
} from "@ant-design/icons";
import AddUserModal from "./forms/add-user-modal";
import { useDispatch, useSelector } from "react-redux";
import { deleteUser, loadAllUsers } from "../../../../stores/slices/admin-slice";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../../notification/notification-components";
import { AppRoles } from "../../../../auth/auth-services/role-management";
import { isSuper } from "../../../../auth/auth-services/auth-service";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../../auth/auth-provider/auth-provider";
import CommonPageTemplate from "../../../helpers/common-page-template";
import { getColumnSearchProps } from "../../../table-helpers/table-filter-helper";
import { AppDispatch, RootState } from "../../../../stores/main-store";
import { IUserFullInfos } from "../../../../models/admin/admin-models";


const UserManagement: React.FC = () => {
  const navigate = useNavigate();
  const { user } = useAuth();
  const dispatch = useDispatch<AppDispatch>();
  const users = useSelector((state: RootState) => state.admin.userMap);
  const [isAddUserModalOpen, setIsAddUserModalOpen] = useState<boolean>(false);
  const [userToEdit, setUserToEdit] = useState<IUserFullInfos | undefined>(
    undefined
  );
  const [isUserSuper, setIsUserSuper] = useState<boolean>(false);
  const [, setSearchText] = useState("");
  const [, setSearchedColumn] = useState("");

  useEffect(() => {
    const checkRoles = async () => {
      try {
        const superUser = await isSuper();
        if (superUser) {
          setIsUserSuper(true);
        }
      } catch (err) {
        console.error(err);
      }
    };

    if (user) {
      checkRoles();
      dispatch(loadAllUsers());
    }
  }, [navigate, user, dispatch]);

  const addNewUserModal = {
    open: () => {
      setIsAddUserModalOpen(true);
    },
    close: () => {
      setUserToEdit(undefined);
      setIsAddUserModalOpen(false);
    },
  };

  const userActions = {
    edit: (user: IUserFullInfos) => {
      setUserToEdit(user);
      setIsAddUserModalOpen(true);
    },
    delete: async (userId: string) => {
      try {
        const result = await dispatch(deleteUser(userId)).unwrap();
        if (result.passed) SuccessNotification("User has been deleted!");
        else {
          ErrorNotification(
            "Something went wrong while deleting the user!",
            result.message
          );
        }
      } catch {
        ErrorNotification("Something went wrong while deleting the user!");
      }
    },
  };

  const canEditOrDelete = (record: IUserFullInfos) => {
    // If the current user is a superuser, they can edit/delete anyone.
    if (isUserSuper) return true;

    // If the current user is not a superuser and the record belongs to a superuser, restrict editing/deleting.
    if (record.roles.includes(AppRoles.Super)) return false;

    // For all other cases (assuming the current user is an admin or lower trying to edit non-super users), allow editing/deleting.
    return true;
  };

  const userColumns = [
    {
      title: "First Name",
      dataIndex: "firstName",
      key: "firstName",
      inputType: "text",
      sorter: (a: IUserFullInfos, b: IUserFullInfos) =>
        a.firstName.localeCompare(b.firstName),
      ...getColumnSearchProps<IUserFullInfos>(
        "firstName",
        setSearchText,
        setSearchedColumn
      ),
    },
    {
      title: "Last Name",
      dataIndex: "lastName",
      key: "lastName",
      inputType: "text",
      sorter: (a: IUserFullInfos, b: IUserFullInfos) =>
        a.lastName.localeCompare(b.lastName),
      ...getColumnSearchProps<IUserFullInfos>(
        "lastName",
        setSearchText,
        setSearchedColumn
      ),
    },
    {
      title: "Email",
      dataIndex: "email",
      key: "email",
      inputType: "text",
      sorter: (a: IUserFullInfos, b: IUserFullInfos) =>
        a.email.localeCompare(b.email),
      ...getColumnSearchProps<IUserFullInfos>(
        "email",
        setSearchText,
        setSearchedColumn
      ),
    },
    {
      title: "Phone Number",
      dataIndex: "phoneNumber",
      key: "phoneNumber",
      inputType: "text",
      sorter: (a: IUserFullInfos, b: IUserFullInfos) =>
        a.phoneNumber.localeCompare(b.phoneNumber),
      ...getColumnSearchProps<IUserFullInfos>(
        "phoneNumber",
        setSearchText,
        setSearchedColumn
      ),
    },
    {
      title: "Email Confirmed",
      dataIndex: "isEmailConfirmed",
      key: "isEmailConfirmed",
      inputType: "checkbox",
      render: (isConfirmed: boolean) =>
        isConfirmed ? (
          <CheckCircleOutlined style={{ color: "green" }} />
        ) : (
          <ExclamationCircleOutlined style={{ color: "red" }} />
        ),
    },
    {
      title: "Roles",
      dataIndex: "roles",
      key: "roles",
      render: (roles: string[]) => (
        <>
          {roles.map((role) => {
            let color = "";
            if (role === AppRoles.Super) {
              color = "red";
            } else if (role === AppRoles.Admin) {
              color = "orange";
            } else {
              color = "blue";
            }
            return (
              <Tag color={color} key={role}>
                {role}
              </Tag>
            );
          })}
        </>
      ),
    },
    {
      title: "Actions",
      key: "actions",
      render: (_: unknown, record: IUserFullInfos) => {
        const isEditableOrDeletable = canEditOrDelete(record);
        return (
          <>
            <Button
              type="link"
              onClick={() => userActions.edit(record)}
              disabled={!isEditableOrDeletable}
            >
              Edit
            </Button>
            <Popconfirm
              placement="topLeft"
              title="Are you sure you want to Delete?"
              okText="Yes"
              cancelText="No"
              onConfirm={() => userActions.delete(record.id)}
              disabled={!isEditableOrDeletable}
            >
              <Button type="link" danger disabled={!isEditableOrDeletable}>
                Delete
              </Button>
            </Popconfirm>
          </>
        );
      },
    },
  ];

  const dataSource = Object.values(users).map((user) => ({
    ...user,
    key: user.id,
  }));

  return (
    <CommonPageTemplate>
      <Button
        type="primary"
        style={{ marginBottom: 16 }}
        onClick={addNewUserModal.open}
      >
        Add User
      </Button>
      <Table columns={userColumns as never} dataSource={dataSource} />
      {isAddUserModalOpen && (
        <AddUserModal
          isOpen={isAddUserModalOpen}
          onClose={addNewUserModal.close}
          userData={userToEdit}
          modifiedBySuper={isUserSuper}
        />
      )}
    </CommonPageTemplate>
  );
};

export default UserManagement;
