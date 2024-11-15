import { useNavigate } from "react-router-dom";

/// Route paths object defining the keys for different routes in the application.
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

/**
 * Generates language-aware route paths based on the provided language.
 *
 * @param lang - The language code (e.g., "en", "fi") to include in the routes.
 * @returns An object with the language-specific paths for each route key.
 */
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

/**
 * Hook-based navigation function to enable language-aware routing.
 *
 * This hook automatically extracts the language from the current URL, generates
 * language-aware paths, and provides a helper function to navigate to specific
 * routes using route keys.
 *
 * @returns A function `navigateWithLanguage` that accepts a route key and navigates to the corresponding path.
 */
export const useLanguageAwareNavigate = () => {
  const navigate = useNavigate();

  // Extract the language from the current URL
  const lang = getCurrentLanguage();

  // Generate language-aware routes
  const paths = generateRoutePaths(lang);

   /**
   * Navigate to a specific route key, preserving the current language in the path.
   *
   * @param routeKey - The key representing the target route (e.g., RoutePaths.home).
   * @param replace - Whether to replace the current history entry (default: false).
   */
  const navigateWithLanguage = (
    routeKey: keyof ReturnType<typeof generateRoutePaths>,
    replace = false
  ) => {
    const targetPath = paths[routeKey]; // Get the correct path for the route key
    navigate(targetPath, { replace });
  };

  return navigateWithLanguage;
};


/**
 * Function-based navigation to enable language-aware routing without using hooks.
 *
 * This function extracts the language from the current URL, generates
 * language-aware paths, and navigates to a specific route key.
 *
 * @param routeKey - The key representing the target route (e.g., RoutePaths.home).
 */
export const languageAwareNavigate = (routeKey: keyof ReturnType<typeof generateRoutePaths>) => {
  const lang = getCurrentLanguage();
  const paths = generateRoutePaths(lang); // Generate language-aware paths
  const targetPath = paths[routeKey]; // Get the target path

  window.location.href = targetPath; // Navigate to the new URL
};

export const getCurrentLanguage = (): string => {
  return window.location.pathname.split("/")[1] || "en";
};
