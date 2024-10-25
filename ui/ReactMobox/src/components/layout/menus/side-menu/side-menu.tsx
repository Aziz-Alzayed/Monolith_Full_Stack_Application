import { Menu } from "antd";
import Sider from "antd/es/layout/Sider";
import { FC, useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import { useAuth } from "../../../../auth/auth-provider/auth-provider";
import { isAdmin, isSuper } from "../../../../auth/auth-services/auth-service";
import { MenuItemType } from "antd/es/menu/interface";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../../../localization/translations/base-translation";
import { MenuFoldOutlined, MenuUnfoldOutlined } from "@ant-design/icons";
import styles from "./side-menu.module.css";
import { RoutePaths, useLanguageAwareNavigate } from "../../../../routing/use-language-aware-navigate";

export const collapsedSideMenuWidth= "5em";
export const SideMenuWidth = "13em";

interface SideMenuProps {
  collapsed: boolean;
  setCollapsed: (collapsed: boolean) => void;
}

const SideMenu: FC<SideMenuProps> = ({ collapsed, setCollapsed }) => {
  const { user } = useAuth();
  const location = useLocation();
  const [userIsAdmin, setUserIsAdmin] = useState<boolean>(false);
  const navigateWithLanguage = useLanguageAwareNavigate();
  const { t } = useTranslation();

  useEffect(() => {
    const checkRoles = async () => {
      const isAdminUser = await isAdmin();
      const isSuperUser = await isSuper();
      setUserIsAdmin((user && (isAdminUser || isSuperUser)) || false);
    };

    if (user) {
      checkRoles();
    } else {
      setUserIsAdmin(false);
    }
  }, [user]);

  const menuItems: MenuItemType[] = [
    {
      key: RoutePaths.userManagement,
      label: t(TranslationKeys.userManagement),
      onClick: () => navigateWithLanguage(RoutePaths.userManagement),
    },
  ];

  const currentSelectedKey =
    (menuItems.find((item) => location.pathname.startsWith(item.key as string))
      ?.key as string) || "";

  return !userIsAdmin ? (
    <></>
  ) : (
    <Sider
      className={styles.sidebarStyle}
      width={SideMenuWidth}
      collapsedWidth={collapsedSideMenuWidth}
      collapsible 
      collapsed={collapsed} 
      onCollapse={setCollapsed}
      trigger={collapsed ? <MenuUnfoldOutlined /> : <MenuFoldOutlined />}  // Collapse button icons
    >
      <Menu
        mode="inline"
        selectedKeys={[currentSelectedKey]}
        items={menuItems}
        inlineCollapsed={collapsed}
        className={styles.menuStyle}
      />
    </Sider>
  );
};

export default SideMenu;
