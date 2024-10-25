import { Content } from "antd/es/layout/layout";
import { FC, PropsWithChildren } from "react";
import style from "./common-page-template.module.css";

const CommonPageTemplate: FC<PropsWithChildren> = ({ children }) => {
  return <Content className={style.commonPageTemplate}>{children}</Content>;
};

export default CommonPageTemplate;
