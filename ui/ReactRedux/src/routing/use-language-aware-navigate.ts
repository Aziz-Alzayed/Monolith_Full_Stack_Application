import { useNavigate, useLocation } from "react-router-dom";

export const RoutePaths = {
  home: "home",
  userManagement: "userManagement",
  userProfile: "userProfile",
  tasks: "tasks",
  verifyEmail: "verifyEmail",
  unauthorized: "unauthorized",
  login: "login",
  resetPassword: "resetPassword",
  notFound: "*",
};

export const generateRoutePaths = (lang: string) => ({
  [RoutePaths.home]: `/${lang}`,
  [RoutePaths.userManagement]: `/${lang}/user-management`,
  [RoutePaths.userProfile]: `/${lang}/user-profile`,
  [RoutePaths.tasks]: `/${lang}/tasks`,
  [RoutePaths.verifyEmail]: `/${lang}/verify-email`,
  [RoutePaths.unauthorized]: `/${lang}/unauthorized`,
  [RoutePaths.login]: `/${lang}/login`,
  [RoutePaths.resetPassword]: `/${lang}/reset-password`,
  [RoutePaths.notFound]: "*",
});

export const useLanguageAwareNavigate = () => {
  const navigate = useNavigate();
  const location = useLocation();

  // Extract the language from the current URL
  const lang = location.pathname.split("/")[1];

  // Generate language-aware routes
  const paths = generateRoutePaths(lang);

  // Helper to navigate using a route key
  const navigateWithLanguage = (
    routeKey: keyof ReturnType<typeof generateRoutePaths>,
    replace = false
  ) => {
    const targetPath = paths[routeKey]; // Get the correct path for the route key
    navigate(targetPath, { replace });
  };

  return navigateWithLanguage;
};
