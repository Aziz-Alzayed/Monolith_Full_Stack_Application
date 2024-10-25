import { useState, useEffect, FC } from "react";
import { Menu, Button, Space } from "antd";
import { MenuFoldOutlined } from "@ant-design/icons"; // For the toggle button icon
import UserDropdownMenu from "./user-dropdown-menu";
import logoImage from "../../../../assets/images/Logo-White.png";
import { Header } from "antd/es/layout/layout";
import { DrawerMenu } from "./drawer-menu";
import { MenuItemType } from "antd/es/menu/interface";
import { useAuth } from "../../../../auth/auth-provider/auth-provider";
import LanguageSwitcher from "./language-switcher";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../../../routing/use-language-aware-navigate";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../../../localization/translations/base-translation";
import styles from "./header-menu.module.css";

const HeaderMenu: FC = () => {
  const [visible, setVisible] = useState(false);
  const [isMobile, setIsMobile] = useState(window.innerWidth < 768);
  const { user } = useAuth();
  const navigateWithLanguage = useLanguageAwareNavigate();
  const { t } = useTranslation();
  // Handle window resize
  useEffect(() => {
    const handleResize = () => setIsMobile(window.innerWidth < 768);
    window.addEventListener("resize", handleResize);
    return () => window.removeEventListener("resize", handleResize);
  }, []);

  // Toggle drawer visibility
  const toggleDrawer = () => setVisible(!visible);

  const menuItems: MenuItemType[] = [
    {
      key: RoutePaths.home,
      label: t(TranslationKeys.homePage),
      onClick: () => navigateWithLanguage(RoutePaths.home),
    },
    {
      key: RoutePaths.tasks,
      label: t(TranslationKeys.tasks),
      onClick: () => navigateWithLanguage(RoutePaths.tasks),
      disabled: user === undefined,
    },
  ];

  const currentSelectedKey =
    (menuItems.find((item) => location.pathname === item.key)?.key as string) ||
    "";

  return (
    <>
      <Header className={styles.headerStyle}>
        <div
          className={styles.logoDivStyle}
          onClick={() => navigateWithLanguage(RoutePaths.home)}
        >
          <img src={logoImage} alt="Logo" className={styles.logoImageStyle} />
        </div>
        {!isMobile ? (
          <>
            <Menu
              mode="horizontal"
              items={menuItems}
              className={styles.menu}
              selectedKeys={[currentSelectedKey]}
            />
            <Space>
              <LanguageSwitcher />
              <UserDropdownMenu />
            </Space>
          </>
        ) : (
          <Button icon={<MenuFoldOutlined />} onClick={toggleDrawer} />
        )}
      </Header>
      {visible && (
        <DrawerMenu
          visible={visible}
          menuItems={menuItems}
          toggleDrawer={toggleDrawer}
        />
      )}
    </>
  );
};

export default HeaderMenu;
