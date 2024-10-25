import { Alert, Button, Form, Input, Modal } from "antd";
import { FC, useState } from "react";
import { useDispatch } from "react-redux";
import AuthenticatedComponent from "../../../../auth/auth-wrappers/authenticated-user-components";
import { UpdateUserPasswordDto } from "../../../../models/user-models/user-models";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../../notification/notification-components";
import { updateUserPassword } from "../../../../stores/slices/user-slice";
import { AppDispatch } from "../../../../stores/main-store";

type ResetPasswordModalProps = {
  isResetPasswordModalOpen: boolean;
  closeResetPasswordModal: () => void;
};

const ResetPasswordModal: FC<ResetPasswordModalProps> = ({
  isResetPasswordModalOpen,
  closeResetPasswordModal,
}) => {
  const dispatch = useDispatch<AppDispatch>();
  const [form] = Form.useForm();
  const [submitError, setSubmitError] = useState<string | undefined>(undefined);

  const onResetPassword = async (values: {
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
  }) => {
    try {
      const requestDto: UpdateUserPasswordDto = {
        oldPassword: values.oldPassword,
        newPassword: values.newPassword,
      };
      const resultAction = await dispatch(updateUserPassword(requestDto)).unwrap();

      if (resultAction.passed) {
        SuccessNotification("Password has been updated successfully");
      } else {
        ErrorNotification(
          "Password has not been updated!"
        );
      }

      // Reset form and state
      form.resetFields();
      setSubmitError(undefined);
      closeResetPasswordModal();
    } catch {
      // Handle errors (e.g., show error message)
      setSubmitError("An error occurred while resetting the password.");
    }
  };

  return (
    <Modal
      title="Reset Password"
      open={isResetPasswordModalOpen}
      onCancel={() => {
        setSubmitError(undefined);
        closeResetPasswordModal();
      }}
      footer={null}
    >
      {submitError && <Alert message={submitError} type="error" showIcon />}
      <Form form={form} onFinish={onResetPassword}>
        <Form.Item
          name="oldPassword"
          rules={[
            { required: true, message: "Please input your old password!" },
          ]}
        >
          <Input.Password placeholder="Old Password" />
        </Form.Item>
        <Form.Item
          name="newPassword"
          rules={[
            { required: true, message: "Please input your new password!" },
            { min: 8, message: "Password must be at least 8 characters long." },
            {
              pattern: new RegExp("[A-Z]"),
              message: "Password must contain an uppercase letter.",
            },
            {
              pattern: new RegExp("[a-z]"),
              message: "Password must contain a lowercase letter.",
            },
            {
              pattern: new RegExp("[0-9]"),
              message: "Password must contain a digit.",
            },
            {
              pattern: new RegExp("[^a-zA-Z0-9]"),
              message: "Password must contain a non-alphanumeric character.",
            },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (value === getFieldValue("oldPassword")) {
                  return Promise.reject(
                    new Error(
                      "The new password cannot be the same as your old password."
                    )
                  );
                }
                return Promise.resolve();
              },
            }),
          ]}
        >
          <Input.Password placeholder="New Password" />
        </Form.Item>
        <Form.Item
          name="confirmPassword"
          dependencies={["newPassword"]}
          rules={[
            { required: true, message: "Please confirm your new password!" },
            ({ getFieldValue }) => ({
              validator(_, value) {
                if (!value || getFieldValue("newPassword") === value) {
                  return Promise.resolve();
                }
                return Promise.reject(
                  new Error("The two passwords that you entered do not match!")
                );
              },
            }),
          ]}
        >
          <Input.Password placeholder="Confirm New Password" />
        </Form.Item>
        <Form.Item>
          <Button type="primary" htmlType="submit">
            Submit
          </Button>
        </Form.Item>
      </Form>
    </Modal>
  );
};

const WrappedComponent = AuthenticatedComponent(ResetPasswordModal);
export default WrappedComponent;
