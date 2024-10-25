import React from "react";
import { Result, Button } from "antd";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../routing/use-language-aware-navigate";

const NotFoundPage: React.FC = () => {
  const navigateWithLanguage = useLanguageAwareNavigate();

  const goHome = () => {
    navigateWithLanguage(RoutePaths.home); // navigate to the home page or any other appropriate route
  };

  return (
    <Result
      status="404"
      title="404"
      subTitle="Sorry, the page you visited does not exist."
      extra={
        <Button type="primary" onClick={goHome}>
          Back Home
        </Button>
      }
    />
  );
};

export default NotFoundPage;
