import { FC, useEffect } from "react";
import { Route, Routes, Navigate, useLocation } from "react-router-dom";
import UsersLoader from "../components/modules/managements/loaders/users-loader";
import UserManagement from "../components/modules/managements/user-management/user-management";
import UserProfile from "../components/modules/user-profile/user-profile";
import EmailVerificationPage from "../components/pages/email-verification-page";
import LandingPage from "../components/pages/landing-page";
import LoginPage from "../components/pages/login-page";
import ResetForgottenPasswordPage from "../components/pages/reset-forget-password-page";
import NotFoundPage from "../error-handlers/not-found-page";
import TaskListView from "../components/modules/tasks-module/main-view/tasks-list-view";
import TasksLoader from "../components/modules/tasks-module/loader/tasks-loader";
import i18n, { languages } from "../localization/i18n";
import { generateRoutePaths } from "./use-language-aware-navigate";

// Redirect to default language if no language is provided
const RedirectToDefaultLanguage = () => {
  const defaultLanguage = i18n.language || "en";
  return <Navigate to={`/${defaultLanguage}`} />;
};

const AppRoutes: FC = () => {
  const location = useLocation();
  const lang = location.pathname.split("/")[1];
  const paths = generateRoutePaths(lang);
  useEffect(() => {
    // Check if the language in the URL is valid and change it in i18n
    if (
      lang &&
      languages.map((l) => l.key).includes(lang) &&
      i18n.language !== lang
    ) {
      i18n.changeLanguage(lang);
    }
  }, [lang]);

  // Redirect to default language if no valid language is found
  if (!lang || !languages.map((l) => l.key).includes(lang)) {
    const defaultLanguage = i18n.language || "en";
    return <Navigate to={`/${defaultLanguage}${location.pathname}`} />;
  }

  return (
    <Routes>
      <Route path="/" element={<RedirectToDefaultLanguage />} />
      <Route path={paths.home} element={<LandingPage />} />
      <Route path={paths.userManagement} element={<UsersLoader />}>
        <Route path="" element={<UserManagement />} />
      </Route>
      <Route path={paths.userProfile} element={<UserProfile />} />
      <Route path={paths.tasks} element={<TasksLoader />}>
        <Route path="" element={<TaskListView />} />
      </Route>
      <Route path={paths.verifyEmail} element={<EmailVerificationPage />} />
      <Route path={paths.unauthorized} element={<NotFoundPage />} />
      <Route path={paths.login} element={<LoginPage />} />
      <Route
        path={paths.resetPassword}
        element={<ResetForgottenPasswordPage />}
      />
      <Route path={paths.notFound} element={<NotFoundPage />} />
    </Routes>
  );
};

export default AppRoutes;
