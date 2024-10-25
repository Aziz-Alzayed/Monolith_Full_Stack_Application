import React, { useEffect, useState } from "react";
import { Form, Input, Button, Card, notification } from "antd";
import { useSearchParams } from "react-router-dom";
import {
  ErrorNotification,
  SuccessNotification,
} from "../notification/notification-components";
import { resetForgetPassword } from "../../stores/slices/user-slice";
import PasswordInput from "../../auth/auth-forms/password-rules/password-input";
import { useDispatch } from "react-redux";
import { AppDispatch } from "../../stores/main-store";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";
import CommonPageTemplate from "../helpers/common-page-template";

interface ResetPasswordFormData {
  password: string;
  confirmPassword: string;
}

const ResetForgottenPasswordPage: React.FC = () => {
  const [searchParams] = useSearchParams();
  const [loading, setLoading] = useState<boolean>(false);
  const navigateWithLanguage = useLanguageAwareNavigate();
  const dispatch = useDispatch<AppDispatch>();

  const email = searchParams.get("email");
  const token = searchParams.get("token");

  // Validate email and token on mount
  useEffect(() => {
    if (!email || !token) {
      handleInvalidTokenOrEmail();
    }
  }, [email, token]);

  const handleInvalidTokenOrEmail = () => {
    ErrorNotification("Email or token is invalid!");
    navigateWithLanguage(RoutePaths.login);
  };

  const handlePasswordMismatch = () => {
    notification.error({
      message: "Error",
      description: "Passwords do not match!",
    });
  };

  const handlePasswordReset = async (values: ResetPasswordFormData) => {
    setLoading(true);
    try {
      const result = await dispatch(
        resetForgetPassword({
          email: email as string,
          newPassword: values.password,
          token: token as string,
        })
      ).unwrap();

      if (result.passed) {
        SuccessNotification("Password has been reset");
        navigateWithLanguage(RoutePaths.login);
      } else {
        ErrorNotification("Error: password has not been reset", result.message);
      }
    } catch (error: unknown) {
      handleResetError(error);
    } finally {
      setLoading(false);
    }
  };

  const handleResetError = (error: unknown) => {
    const errorMessage =
      error instanceof Error ? error.message : "Failed to reset password.";
    notification.error({
      message: "Error",
      description: errorMessage,
    });
  };

  const onFinish = async (values: ResetPasswordFormData) => {
    if (!email || !token) {
      handleInvalidTokenOrEmail();
      return;
    }

    if (values.password !== values.confirmPassword) {
      handlePasswordMismatch();
      return;
    }

    await handlePasswordReset(values);
  };

  return (
    <CommonPageTemplate>
      <div className="flex justify-center items-center min-h-screen">
        <Card
          title="Reset Your Password"
          bordered={false}
          style={{ maxWidth: 400, margin: "auto", boxShadow: "0px 4px 8px rgba(0, 0, 0, 0.1)" }}
        >
          <Form
            name="reset_password_form"
            initialValues={{ remember: true }}
            onFinish={onFinish}
            autoComplete="off"
            layout="vertical"
          >
            <PasswordInput />
            <Form.Item
              name="confirmPassword"
              label="Confirm Password"
              dependencies={["password"]}
              hasFeedback
              rules={[
                { required: true, message: "Please confirm your password!" },
                ({ getFieldValue }) => ({
                  validator(_, value) {
                    if (!value || getFieldValue("password") === value) {
                      return Promise.resolve();
                    }
                    return Promise.reject(
                      new Error("The two passwords do not match!")
                    );
                  },
                }),
              ]}
            >
              <Input.Password placeholder="Confirm Password" />
            </Form.Item>
            <Form.Item>
              <Button type="primary" htmlType="submit" loading={loading} block>
                Reset Password
              </Button>
            </Form.Item>
          </Form>
        </Card>
      </div>
    </CommonPageTemplate>
  );
};

export default ResetForgottenPasswordPage;
