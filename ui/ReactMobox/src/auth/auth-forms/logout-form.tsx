import { FC, useState } from "react";
import { useAuth } from "../auth-provider/auth-provider";
import { Button, Modal, Spin } from "antd";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../localization/translations/base-translation";

interface LogoutFormProps {
  isOpen: boolean;
  onClose: () => void;
}

const LogoutForm: FC<LogoutFormProps> = ({ isOpen, onClose }) => {
  const { handleLogout } = useAuth();
  const { t } = useTranslation();
  const [isLoading, setIsLoading] = useState(false);

  const onConfirmLogout = async () => {
    try {
      setIsLoading(true);
      await handleLogout();
    } catch (error) {
      console.error("Error while logging out", error);
    } finally {
      setIsLoading(false);
      onClose();
    }
  };

  return (
    <Spin spinning={isLoading}>
      <Modal
        title={t(TranslationKeys.cancel)}
        open={isOpen}
        onCancel={onClose}
        footer={[
          <Button key="back" onClick={onClose}>
            {t(TranslationKeys.cancel)}
          </Button>,
          <Button key="submit" type="primary" onClick={onConfirmLogout}>
            {t(TranslationKeys.confirmLogout)}
          </Button>,
        ]}
      >
        <p> {t(TranslationKeys.confirmLogoutQuestion)}?</p>
      </Modal>
    </Spin>
  );
};

export default LogoutForm;
