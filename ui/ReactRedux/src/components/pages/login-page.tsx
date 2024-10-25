import { FC } from "react";
import LoginForm from "../../auth/auth-forms/login-form";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";

const LoginPage: FC = () => {
  const navigateWithLanguage = useLanguageAwareNavigate();

  const handleLoginClose = () => {
    navigateWithLanguage(RoutePaths.home);
  };

  return (
    <div className="login-page-container">
      <LoginForm isOpen={true} onClose={handleLoginClose} />
    </div>
  );
};

export default LoginPage;
