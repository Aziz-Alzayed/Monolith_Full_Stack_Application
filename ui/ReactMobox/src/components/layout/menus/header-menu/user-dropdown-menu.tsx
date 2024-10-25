import { FC, useState } from "react";
import { Dropdown, Button, MenuProps, Avatar } from "antd";
import { LoginOutlined, LogoutOutlined, UserOutlined } from "@ant-design/icons";
import { useAuth } from "../../../../auth/auth-provider/auth-provider";
import LoginForm from "../../../../auth/auth-forms/login-form";
import LogoutForm from "../../../../auth/auth-forms/logout-form";
import { ItemType, MenuItemType } from "antd/es/menu/interface";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../../../routing/use-language-aware-navigate";

interface UserDropdownMenuProps {
  className?: string;
}

const UserDropdownMenu: FC<UserDropdownMenuProps> = ({ className }) => {
  const navigateWithLanguage = useLanguageAwareNavigate();
  const { user } = useAuth();
  const [isLoginModalVisible, setIsLoginModalVisible] = useState(false);
  const [isLogoutModalVisible, setIsLogoutModalVisible] = useState(false);

  const userEmail: string = user?.email || "";

  const userEmailItem: MenuItemType | undefined = userEmail
    ? {
        key: "userEmailItem",
        label: `${userEmail}`,
        icon: (
          <Avatar style={{ backgroundColor: "#f56a00", marginRight: 8 }}>
            {userEmail.charAt(0).toUpperCase()}
          </Avatar>
        ),
        onClick: () => navigateWithLanguage(RoutePaths.userProfile),
      }
    : undefined;

  const logoutItem: MenuItemType | undefined = userEmail
    ? {
        key: "logoutItem",
        label: "Logout",
        icon: <LogoutOutlined style={{ marginRight: 8 }} />,
        onClick: () => setIsLogoutModalVisible(true),
      }
    : undefined;

  const loginItem: MenuItemType | undefined = !userEmail
    ? {
        key: "loginItem",
        label: "Login",
        icon: <LoginOutlined style={{ marginRight: 8 }} />,
        onClick: () => setIsLoginModalVisible(true),
      }
    : undefined;

  // Helper function to add dividers only when necessary
  const addDividerIfNeeded = (
    items: Array<ItemType | undefined>
  ): ItemType[] => {
    return items.reduce((acc: ItemType[], item, index) => {
      if (item) {
        // Add a divider before an item if it's not the first one and previous item exists
        if (acc.length > 0) {
          acc.push({ key: `divider-${index}`, type: "divider" });
        }
        acc.push(item);
      }
      return acc;
    }, []);
  };

  const menuItems = addDividerIfNeeded([
    userEmailItem,
    logoutItem || loginItem,
  ]);

  const userMenuProps: MenuProps = {
    mode: "vertical",
    items: menuItems,
  };

  return (
    <div className={className}>
      <Dropdown menu={userMenuProps} trigger={["click"]}>
        <Button shape="circle" icon={<UserOutlined />} />
      </Dropdown>
      {isLoginModalVisible && (
        <LoginForm
          isOpen={isLoginModalVisible}
          onClose={() => setIsLoginModalVisible(false)}
        />
      )}
      {isLogoutModalVisible && (
        <LogoutForm
          isOpen={isLogoutModalVisible}
          onClose={() => setIsLogoutModalVisible(false)}
        />
      )}
    </div>
  );
};

export default UserDropdownMenu;
