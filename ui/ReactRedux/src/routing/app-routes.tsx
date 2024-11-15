import {
  Route,
  Navigate,
  createBrowserRouter,
  createRoutesFromElements,
} from "react-router-dom";
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
import i18n from "../localization/i18n";
import {
  generateRoutePaths,
  getCurrentLanguage,
} from "./use-language-aware-navigate";
import AppLayout from "../components/layout/app-layout";

// Redirect to default language if no language is provided
const RedirectToDefaultLanguage = () => {
  const defaultLanguage = i18n.language || "en";
  return <Navigate to={`/${defaultLanguage}`} />;
};

const router = () => {
  const lang = getCurrentLanguage();
  const paths = generateRoutePaths(lang);

  return createBrowserRouter(
    createRoutesFromElements(
      <Route path="/" element={<AppLayout />}>
        <Route index element={<RedirectToDefaultLanguage />} />
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
      </Route>
    )
  );
};
export default router;
