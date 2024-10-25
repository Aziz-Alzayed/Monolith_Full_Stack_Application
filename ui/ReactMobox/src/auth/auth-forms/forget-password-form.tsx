import React, { useState } from "react";
import { Modal, Form, Input, Button, Typography } from "antd";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../components/notification/notification-components";
import userStore from "../../stores/user-stores/user-store";
import { ForgetPasswordDto } from "../../models/user-models/user-models";
import { resetPasswordPath } from "../../apiConfig";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../localization/translations/base-translation";

interface ForgotPasswordFormProps {
  isOpen: boolean;
  onClose: () => void;
}

const ForgotPasswordForm: React.FC<ForgotPasswordFormProps> = ({
  isOpen,
  onClose,
}) => {
  const { t } = useTranslation();
  const [form] = Form.useForm();
  const navigateWithLanguage = useLanguageAwareNavigate();
  const [loading, setLoading] = useState<boolean>(false);
  const [submitted, setSubmitted] = useState<boolean>(false);

  const handleSubmit = async (values: { email: string }) => {
    try {
      setLoading(true);
      const forgetPasswordDto: ForgetPasswordDto = {
        email: values.email,
        resetUrl: resetPasswordPath,
      };
      const result = await userStore.ForgetPassword(forgetPasswordDto);
      if (result.passed) {
        SuccessNotification(
          `${t(TranslationKeys.submittingForgotPassword)}: ${values.email}`,
          `${t(TranslationKeys.checkYourEmail)}.`
        );
        navigateWithLanguage(RoutePaths.login);
      } else
        ErrorNotification(
          `${t(TranslationKeys.failedSubmitForgotPassword)}.`,
          result.message
        );
    } catch (error) {
      console.error("Failed to submit forgot password request", error);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title={t(TranslationKeys.forgotPassword)}
      open={isOpen}
      onCancel={() => {
        onClose();
        setSubmitted(false);
      }}
      footer={null}
    >
      {!submitted ? (
        <Form form={form} layout="vertical" onFinish={handleSubmit}>
          <Typography.Text>
            Enter your email address and we'll send you a link to reset your
            password.
          </Typography.Text>
          <Form.Item
            name="email"
            rules={[
              {
                required: true,
                message: t(TranslationKeys.emailInputMessage),
                type: "email",
              },
            ]}
          >
            <Input placeholder={t(TranslationKeys.email)} />
          </Form.Item>
          <Form.Item>
            <Button type="primary" htmlType="submit" block loading={loading}>
              {t(TranslationKeys.submit)}
            </Button>
          </Form.Item>
        </Form>
      ) : (
        <Typography.Text>Check your email for the reset link.</Typography.Text>
      )}
    </Modal>
  );
};

export default ForgotPasswordForm;
