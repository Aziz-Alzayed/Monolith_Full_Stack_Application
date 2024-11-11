import { BrowserRouter } from "react-router-dom";
import AuthProvider from "./auth/auth-provider/auth-provider";
import AppLayout from "./components/layout/app-layout";
import ThemeProvider from "./components/theme-configs/config-provider";
import { useLocation } from "react-router-dom";
import ErrorBoundary from "./error-handlers/error-boundary";
import { I18nextProvider } from "react-i18next";
import i18n from "./localization/i18n";
import AppRoutes from "./routing/app-routes";
import { Provider } from "react-redux";
import store from "./stores/main-store";

function App() {
  const { pathname } = useLocation();
  return (
    <I18nextProvider i18n={i18n}>
      <Provider store={store}>
        <BrowserRouter>
          <AuthProvider>
            <ErrorBoundary navigationPath={pathname}>
              <ThemeProvider>
                <AppLayout>
                  <AppRoutes />
                </AppLayout>
              </ThemeProvider>
            </ErrorBoundary>
          </AuthProvider>
        </BrowserRouter>
      </Provider>
    </I18nextProvider>
  );
}
export default App;
