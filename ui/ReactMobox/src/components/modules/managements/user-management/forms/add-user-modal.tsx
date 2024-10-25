import { FC, useEffect, useState } from "react";
import { Modal, Form, Input, Select, Button } from "antd";
import {
  AppRoles,
  getAllAppRoles,
} from "../../../../../auth/auth-services/role-management";
import { IUserFullInfos } from "../../../../../models/admin/admin-models";
import adminStore from "../../../../../stores/admin-stores/admin-store";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../../../notification/notification-components";
import { resetPasswordPath } from "../../../../../apiConfig";

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
  const isEdit: boolean = userData ? true : false;
  const [listOfAllowedRoles, setListOfAllowedRoles] = useState<string[]>([]);

  useEffect(() => {
    const allRoles = getAllAppRoles();
    if (modifiedBySuper) {
      setListOfAllowedRoles(allRoles);
    } else {
      const withoutSuper = allRoles.filter((r) => r != AppRoles.Super);
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
        if (isEdit) {
          const result = await adminStore.updateUser(userData?.id as string, {
            id: userData?.id as string,
            email: values.email,
            firstName: values.firstName,
            lastName: values.lastName,
            phoneNumber: values.phoneNumber,
            roles: [values.role],
          });
          if (result.passed) SuccessNotification("The user has been updated!");
          else
            ErrorNotification("Error while update user infos!", result.message);
        } else {
          const result = await adminStore.addUser({
            email: values.email,
            firstName: values.firstName,
            lastName: values.lastName,
            phoneNumber: values.phoneNumber,
            roles: [values.role],
            resetUrl: resetPasswordPath,
          });
          if (result.passed) SuccessNotification("The newUser has been added!");
          else
            ErrorNotification("Error while adding new user!", result.message);
        }
        onClose();
      })
      .catch((info) => {
        console.warn("Validate Failed:", info);
      });
  };

  return (
    <Modal
      title="Add New User"
      open={isOpen}
      onCancel={onClose}
      footer={[
        <Button key="back" onClick={onClose}>
          Cancel
        </Button>,
        <Button key="submit" type="primary" onClick={handleSubmit}>
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
