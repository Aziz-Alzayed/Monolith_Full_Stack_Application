import React, { useEffect, useState } from "react";
import { Form, Input, Button, notification, Card } from "antd";
import { useSearchParams } from "react-router-dom";
import {
  ErrorNotification,
  SuccessNotification,
} from "../notification/notification-components";
import userStore from "../../stores/user-stores/user-store";
import PasswordInput from "../../auth/auth-forms/password-rules/password-input";
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
  const email = searchParams.get("email");
  const token = searchParams.get("token");

  useEffect(() => {
    if (!email || !token) {
      ErrorNotification("Email or the token is invalid!");
      navigateWithLanguage(RoutePaths.login); // Redirect them to a safe location
    }
  }, [email, token, navigateWithLanguage]);

  const onFinish = async (values: ResetPasswordFormData) => {
    if (!email && !token) {
      ErrorNotification("Email or the token is invalid!");
      return;
    }

    if (values.password !== values.confirmPassword) {
      notification.error({
        message: "Error",
        description: "Passwords do not match!",
      });
      return;
    }
    setLoading(true);
    try {
      if (email && token) {
        const result = await userStore.ResetForgetPassword({
          email,
          newPassword: values.password,
          token,
        });
        if (result.passed) {
          SuccessNotification("Password has been reset");
          navigateWithLanguage(RoutePaths.login);
        } else {
          ErrorNotification("Error: password has been not rest");
        }
      }
    } catch (error: unknown) {
      let errorMessage = "Failed to reset password.";

      if (error instanceof Error) {
        errorMessage = error.message;
      }

      notification.error({
        message: "Error",
        description: errorMessage,
      });
    } finally {
      setLoading(false);
    }
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
