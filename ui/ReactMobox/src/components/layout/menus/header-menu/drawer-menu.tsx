import { Drawer, Menu } from "antd";
import { FC } from "react";
import UserDropdownMenu from "./user-dropdown-menu";
import type { DrawerStyles } from "antd/es/drawer/DrawerPanel";
import { MenuItemType } from "antd/es/menu/interface";
import LanguageSwitcher from "./language-switcher";
import styles from "./drawer-menu.module.css"
interface DrawerMenuProps {
  menuItems: MenuItemType[];
  visible: boolean;
  toggleDrawer: () => void;
}

const style = `
.ant-menu-title-content{
    border-bottom:solid
}
`;

export const DrawerMenu: FC<DrawerMenuProps> = ({
  menuItems,
  toggleDrawer,
  visible,
}) => {
  const drawerStyles: DrawerStyles = {
    mask: {},
    content: {},
    header: {},
    body: {},
    footer: {},
  };

  return (
    <>
      <style>{style}</style>
      <Drawer
        placement="right"
        onClose={toggleDrawer}
        open={visible}
        styles={drawerStyles}
      >
        <Menu mode="inline" items={menuItems} />
        <LanguageSwitcher />
        <UserDropdownMenu className={styles.userDropdownMenu} />
      </Drawer>
    </>
  );
};
