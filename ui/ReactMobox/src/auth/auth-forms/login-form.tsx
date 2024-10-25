import { FC, useState } from "react";
import { useAuth } from "../auth-provider/auth-provider";
import {
  Form,
  Input,
  Button,
  Checkbox,
  Modal,
  Spin,
  Typography,
  Row,
  Col,
} from "antd";
import { ILoginRequest } from "../../models/auth-models/auth-models";
import ForgotPasswordForm from "./forget-password-form";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../localization/translations/base-translation";

import styles from "./login-form.module.css";

interface LoginFormProps {
  isOpen: boolean;
  onClose: () => void;
}

const LoginForm: FC<LoginFormProps> = ({ isOpen, onClose }) => {
  const { handleLogin } = useAuth();
  const { t } = useTranslation();
  const [isLoading, setIsLoading] = useState(false);
  const [loginError, setLoginError] = useState("");
  const [isForgotPasswordModalOpen, setIsForgotPasswordModalOpen] =
    useState<boolean>(false);

  const onFinish = async (values: ILoginRequest) => {
    try {
      setIsLoading(true);
      setLoginError("");
      await handleLogin(values);
      onClose();
    } catch (error) {
      setLoginError(`${t(TranslationKeys.failedLoginMessage)}.`);
      console.error("Error while logging in", error);
    } finally {
      setIsLoading(false);
    }
  };

  const [form] = Form.useForm();

  return (
    <>
      <Modal
        title="Login"
        open={isOpen}
        onCancel={onClose}
        footer={null}
        transitionName="zoom"
      >
        <Spin spinning={isLoading}>
          <Form
            form={form}
            name="login"
            initialValues={{ remember: true }}
            onFinish={onFinish}
            layout="vertical"
          >
            {loginError && (
              <Typography.Paragraph type="danger">
                {loginError}
              </Typography.Paragraph>
            )}

            <Form.Item
              name="email"
              label={t(TranslationKeys.email)}
              rules={[
                {
                  required: true,
                  message: t(TranslationKeys.emailInputMessage),
                  type: "email",
                },
              ]}
            >
              <Input placeholder="Email" />
            </Form.Item>

            <Form.Item
              name="password"
              label={t(TranslationKeys.password)}
              rules={[
                {
                  required: true,
                  message: t(TranslationKeys.passwordInputMessage),
                },
              ]}
            >
              <Input.Password placeholder="Password" />
            </Form.Item>

            <Row justify="space-between">
              <Col>
                <Form.Item name="remember" valuePropName="checked">
                  <Checkbox>{t(TranslationKeys.readMe)}</Checkbox>
                </Form.Item>
              </Col>
              <Col>
                <Form.Item>
                  <a
                    onClick={() => setIsForgotPasswordModalOpen(true)}
                    className={styles.forgetPassword}
                  >
                    {t(TranslationKeys.forgotPassword)}?
                  </a>
                </Form.Item>
              </Col>
            </Row>

            <Form.Item>
              <Button type="primary" htmlType="submit" block>
                {t(TranslationKeys.login)}
              </Button>
            </Form.Item>
          </Form>
        </Spin>
      </Modal>
      {isForgotPasswordModalOpen && (
        <ForgotPasswordForm
          isOpen={isForgotPasswordModalOpen}
          onClose={() => setIsForgotPasswordModalOpen(false)}
        />
      )}
    </>
  );
};

export default LoginForm;
