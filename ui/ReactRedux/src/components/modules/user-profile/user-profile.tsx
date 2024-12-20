import { CheckOutlined, CloseOutlined, EditOutlined } from "@ant-design/icons";
import {
  Button,
  Checkbox,
  Col,
  Input,
  List,
  Popconfirm,
  Row,
  Typography,
} from "antd";
import { FC, useEffect, useState } from "react";
import { useAuth } from "../../../auth/auth-provider/auth-provider";
import AuthenticatedComponent from "../../../auth/auth-wrappers/authenticated-user-components";
import { IUserInfo } from "../../../models/auth-models/auth-models";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../notification/notification-components";
import ResetPasswordModal from "./user-forms/reset-password-modal";
import CommonPageTemplate from "../../helpers/common-page-template";
import { useDispatch } from "react-redux";
import { updateUserDetails, updateUserEmail, sendVerificationEmail } from "../../../stores/slices/user-slice";
import { AppDispatch } from "../../../stores/main-store";
import { verificationPath } from "../../../apiConfig";

type EditValuesType = {
  firstName?: string;
  lastName?: string;
  email?: string;
  emailConfirmed?: string;
  [key: string]: string | undefined;
};

type UpdateKey = "firstName" | "lastName" | "email" | "roles" | "emailConfirmed";

type UpdateFunctionType = {
  [key in UpdateKey]: () => Promise<void>;
};

type ListItemType = {
  key: UpdateKey;
  label: string;
  component: React.ReactNode;
};

const UserProfile: FC = () => {
  const { user, roles, updateUser } = useAuth();
  const dispatch = useDispatch<AppDispatch>();
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [editingKey, setEditingKey] = useState<string | undefined>(undefined);
  const [isResetPasswordModalOpen, setIsResetPasswordModalOpen] =
    useState<boolean>(false);
  const [editValues, setEditValues] = useState<EditValuesType>({
    firstName: "",
    lastName: "",
    email: "",
    emailConfirmed: "false",
  });

  useEffect(() => {
    setEditValues({
      firstName: user?.firstName,
      lastName: user?.lastName,
      email: user?.email,
      emailConfirmed: user?.emailConfirmed === false ? "false" : "true",
    });
  }, [user, isLoading]);

  const isEditing = (key: string) => editingKey === key;

  const onEdit = (key: string) => {
    setEditingKey(key);
  };

  const passwordModal = {
    open: () => {
      setIsResetPasswordModalOpen(true);
    },
    close: () => {
      setIsResetPasswordModalOpen(false);
    },
  };

  const onSave = async (key: UpdateKey) => {
    try {
      if (!key) {
        ErrorNotification(`${key} is Empty!`);
        return;
      }
      const newValue = editValues[key];
      if (!newValue || !key) {
        ErrorNotification(`${key} is Empty!`);
        return;
      }
      setIsLoading(true);

      const updateFunctions: UpdateFunctionType = {
        firstName: async () => {
          const result = await dispatch(updateUserDetails({ newFirstName: newValue })).unwrap();
          if (result.passed) {
            return;
          }
          throw new Error("Update failed");
        },
        lastName: async () => {
          const result = await dispatch(updateUserDetails({ newLastName: newValue })).unwrap();
          if (result.passed) {
            return;
          }
          throw new Error("Update failed");
        },
        email: async () => {
          const result = await dispatch(
            updateUserEmail({
              newEmail: newValue,
              verificationUrl: verificationPath,
            })
          ).unwrap();;
          if (result.passed) {
            return;
          }
          throw new Error("Update failed");
        },
        roles: async () => {
          return;
        },
        emailConfirmed: async () => {
          return;
        },
      };
      

      await updateFunctions[key]();
      if (user) {
        const newUserData: IUserInfo = { ...user, [key]: newValue };
        updateUser(newUserData); // Update user state
        SuccessNotification(`${key} has been updated successfully!`);
      }
    } catch {
      ErrorNotification(
        `${key} has not been updated successfully, please check logs!`
      );
    } finally {
      setEditingKey(undefined);
      setIsLoading(false);
    }
  };

  const onCancel = () => {
    setEditingKey(undefined);
  };

  const onChange = (key: string, value: string) => {
    setEditValues((prevValues) => ({ ...prevValues, [key]: value }));
  };

  const sendVerification = async () => {
    if (user?.email) {
      const resultAction = await dispatch(
        sendVerificationEmail({
          userEmail: user?.email,
          verificationUrl: verificationPath,
        })
      ).unwrap();

      if (resultAction.passed) {
        SuccessNotification("Verification email has been sent");
      } else {
        ErrorNotification(
          "Error with sending a verification email"
        );
      }
    }
  };

  const listItems: ListItemType[] = [
    {
      key: "firstName",
      label: "First Name",
      component: <span>{user?.firstName}</span>,
    },
    {
      key: "lastName",
      label: "Last Name",
      component: <span>{user?.lastName}</span>,
    },
    { key: "email", label: "Email", component: <span>{user?.email}</span> },
    {
      key: "emailConfirmed",
      label: "Email Confirmed",
      component:
        editValues.emailConfirmed?.toLowerCase() === "true" ? (
          <Checkbox checked disabled />
        ) : (
          <Button onClick={sendVerification}>Send Verification Email</Button>
        ),
    },
    {
      key: "roles",
      label: "Roles",
      component: <span>{roles.join(", ")}</span>,
    },
  ];

  return (
    <CommonPageTemplate>
      <List
        loading={isLoading}
        header={<div>User Profile</div>}
        bordered
        dataSource={listItems}
        renderItem={(item) => (
          <List.Item
            actions={[
              item.key !== "roles" &&
                item.key !== "emailConfirmed" &&
                (isEditing(item.key) ? (
                  <>
                    <Button
                      key="cancel"
                      icon={<CloseOutlined />}
                      onClick={() => onCancel()}
                      style={{ marginRight: "5px" }}
                    />
                    <Popconfirm
                      placement="topLeft"
                      title="Are you sure you want to update?"
                      okText="Yes"
                      cancelText="No"
                      onConfirm={() => onSave(item.key)}
                    >
                      <Button key="save" icon={<CheckOutlined />} />
                    </Popconfirm>
                  </>
                ) : (
                  <a key="edit" onClick={() => onEdit(item.key)}>
                    <span>
                      Edit <EditOutlined style={{ marginLeft: "4px" }} />
                    </span>
                  </a>
                )),
            ].filter(Boolean)}
          >
            <Row style={{ width: "100%" }}>
              <Col span={4} style={{ textAlign: "left", paddingRight: 16 }}>
                <Typography.Text strong>{item.label}:</Typography.Text>
              </Col>
              <Col span={16} style={{ textAlign: "left" }}>
                {isEditing(item.key) ? (
                  <Input
                    value={editValues[item.key]}
                    onChange={(e) => onChange(item.key, e.target.value)}
                    style={{ width: "100%" }}
                  />
                ) : (
                  <span style={{ flexGrow: 1, textAlign: "left" }}>
                    {item.component}
                  </span>
                )}
              </Col>
            </Row>
          </List.Item>
        )}
      />
      <Button
        onClick={passwordModal.open}
        style={{ marginTop: "10px", display: "block" }}
        type="primary"
      >
        Reset Password
      </Button>
      {isResetPasswordModalOpen && (
        <ResetPasswordModal
          key="ResetPasswordModal"
          closeResetPasswordModal={passwordModal.close}
          isResetPasswordModalOpen={isResetPasswordModalOpen}
        />
      )}
    </CommonPageTemplate>
  );
};

const UserProfileComponent = AuthenticatedComponent(UserProfile);
export default UserProfileComponent;
