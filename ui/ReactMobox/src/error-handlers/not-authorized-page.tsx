import React from "react";
import { Result, Button } from "antd";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../routing/use-language-aware-navigate";

const NotAuthorizedPage: React.FC = () => {
  const navigateWithLanguage = useLanguageAwareNavigate();
  const goBack = () => {
    navigateWithLanguage(RoutePaths.home); // Navigate to the home page or any other appropriate route
  };

  return (
    <Result
      status="403"
      title="403"
      subTitle="Sorry, you are not authorized to access this page."
      extra={
        <Button type="primary" onClick={goBack}>
          Back Home
        </Button>
      }
    />
  );
};

export default NotAuthorizedPage;
