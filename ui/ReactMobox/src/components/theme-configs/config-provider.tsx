import { ConfigProvider } from "antd";
import { FC, PropsWithChildren } from "react";
import { ThemeConfigs } from "./theme-configs";
const ThemeProvider: FC<PropsWithChildren> = ({ children }) => {
  return <ConfigProvider theme={ThemeConfigs}>{children}</ConfigProvider>;
};

export default ThemeProvider;
