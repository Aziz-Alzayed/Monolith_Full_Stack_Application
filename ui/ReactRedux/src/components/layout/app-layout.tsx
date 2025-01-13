import { FC, PropsWithChildren } from "react";
import { Layout } from "antd";
import HeaderMenu from "./menus/header-menu/header-menu";
import SideMenu from "./menus/side-menu/side-menu";
import BackTopFloatingButton from "./floating-buttons/back-top-button";
import { AppFooter } from "./menus/footer/footer";
import { useAuth } from "../../auth/auth-provider/auth-provider";
import { AppRoles } from "../../auth/auth-services/role-management";
import styles from "./app-layout.module.css";

const { Content } = Layout;

const AppLayout: FC<PropsWithChildren> = ({ children }) => {
  const { roles } = useAuth();
  const isAdmin =
    roles?.includes(AppRoles.Admin) || roles?.includes(AppRoles.Super);
  return (
    <Layout className={styles.layoutStyle}>
      {isAdmin && <SideMenu />}
      <Layout >
      <HeaderMenu />
        <Layout>
          <Content className={styles.contentStyle}>{children}</Content>
          <BackTopFloatingButton />
          <AppFooter />
        </Layout>
      </Layout>
    </Layout>
  );
};

export default AppLayout;
