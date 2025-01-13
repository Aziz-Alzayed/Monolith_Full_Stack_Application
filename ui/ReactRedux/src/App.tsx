import { BrowserRouter } from "react-router-dom";
import AuthProvider from "./auth/auth-provider/auth-provider";
import AppLayout from "./components/layout/app-layout";
import ThemeProvider from "./components/theme-configs/config-provider";
import ErrorBoundary from "./error-handlers/error-boundary";
import { I18nextProvider } from "react-i18next";
import i18n from "./localization/i18n";
import AppRoutes from "./routing/app-routes";
import { Provider } from "react-redux";
import store from "./stores/main-store";

function App() {
  return (
    <BrowserRouter>
      <I18nextProvider i18n={i18n}>
        <Provider store={store}>
          <AuthProvider>
            <ErrorBoundary>
              <ThemeProvider>
                <AppLayout>
                  <AppRoutes />
                </AppLayout>
              </ThemeProvider>
            </ErrorBoundary>
          </AuthProvider>
        </Provider>
      </I18nextProvider>
    </BrowserRouter>
  );
}
export default App;
