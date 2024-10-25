import { FC, useEffect, useState } from "react";
import { Modal, Form, Input, Select, Button } from "antd";
import { useDispatch, useSelector } from "react-redux";
import {
  AppRoles,
  getAllAppRoles,
} from "../../../../../auth/auth-services/role-management";
import { IUserFullInfos } from "../../../../../models/admin/admin-models";
import {
  addUser,
  updateUser,
} from "../../../../../stores/slices/admin-slice"; // Import Redux actions
import {
  ErrorNotification,
  SuccessNotification,
} from "../../../../notification/notification-components";
import { resetPasswordPath } from "../../../../../apiConfig";
import { AppDispatch, RootState } from "../../../../../stores/main-store";

const { Option } = Select;

interface AddUserModalProps {
  isOpen: boolean;
  onClose: () => void;
  userData?: IUserFullInfos | undefined;
  modifiedBySuper: boolean;
}

const AddUserModal: FC<AddUserModalProps> = ({
  isOpen,
  onClose,
  userData,
  modifiedBySuper,
}) => {
  const [form] = Form.useForm();
  const isEdit: boolean = !!userData;
  const [listOfAllowedRoles, setListOfAllowedRoles] = useState<string[]>([]);
  
  // Get the loading and error state from the Redux store
  const userLoading = useSelector((state: RootState) => state.admin.usersLoading);
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    const allRoles = getAllAppRoles();
    if (modifiedBySuper) {
      setListOfAllowedRoles(allRoles);
    } else {
      const withoutSuper = allRoles.filter((r) => r !== AppRoles.Super);
      setListOfAllowedRoles(withoutSuper);
    }
  }, [modifiedBySuper]);

  useEffect(() => {
    if (userData) {
      form.setFieldsValue({
        ...userData,
        role: userData.roles[0],
      });
    } else {
      form.resetFields();
    }
  }, [userData, form]);

  const handleSubmit = async () => {
    form
      .validateFields()
      .then(async (values) => {
        if (isEdit && userData) {
          const resultAction = await dispatch(
            updateUser({
              userId: userData.id,
              userData: {
                id: userData.id,
                email: values.email,
                firstName: values.firstName,
                lastName: values.lastName,
                phoneNumber: values.phoneNumber,
                roles: [values.role],
              },
            })
          ).unwrap();

          if (resultAction.passed) {
            SuccessNotification("The user has been updated!");
          } else {
            ErrorNotification("Error while updating user info!");
          }
        } else {
          const resultAction = await dispatch(
            addUser({
              email: values.email,
              firstName: values.firstName,
              lastName: values.lastName,
              phoneNumber: values.phoneNumber,
              roles: [values.role],
              resetUrl: resetPasswordPath,
            })
          ).unwrap();

          if (resultAction.passed) {
            SuccessNotification("The new user has been added!");
          } else {
            ErrorNotification("Error while adding new user!");
          }
        }
        onClose();
      })
      .catch((info) => {
        console.warn("Validation Failed:", info);
      });
  };

  return (
    <Modal
      title={isEdit ? "Edit User" : "Add New User"}
      open={isOpen}
      onCancel={onClose}
      footer={[
        <Button key="back" onClick={onClose}>
          Cancel
        </Button>,
        <Button
          key="submit"
          type="primary"
          onClick={handleSubmit}
          loading={userLoading}  // Add loading state for better UX
        >
          {userData ? "Update User" : "Add User"}
        </Button>,
      ]}
    >
      <Form form={form} layout="vertical" onFinish={handleSubmit}>
        <Form.Item
          name="firstName"
          label="First Name"
          rules={[{ required: true, message: "Please input the first name!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="lastName"
          label="Last Name"
          rules={[{ required: true, message: "Please input the last name!" }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="phoneNumber"
          label="Phone Number"
          rules={[
            {
              pattern: new RegExp(/^\+[1-9]\d{1,14}$/),
              message: "Please enter a valid phone number!",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="email"
          label="Email"
          rules={[
            {
              required: true,
              message: "Please input the email!",
              type: "email",
            },
          ]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          name="role"
          label="Role"
          rules={[
            { required: true, message: "Please select at least one role!" },
          ]}
        >
          <Select
            placeholder="Select role"
            filterOption={(input, option) =>
              option?.children
                ? option.children
                    .toString()
                    .toLowerCase()
                    .indexOf(input.toLowerCase()) >= 0
                : false
            }
          >
            {listOfAllowedRoles.map((role) => (
              <Option key={role} value={role}>
                {role}
              </Option>
            ))}
          </Select>
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default AddUserModal;
