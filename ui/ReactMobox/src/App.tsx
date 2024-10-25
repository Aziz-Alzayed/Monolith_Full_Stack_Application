import { BrowserRouter } from "react-router-dom";
import AuthProvider from "./auth/auth-provider/auth-provider";
import AppLayout from "./components/layout/app-layout";
import ThemeProvider from "./components/theme-configs/config-provider";
import ErrorBoundary from "./error-handlers/error-boundary";
import { I18nextProvider } from "react-i18next";
import i18n from "./localization/i18n";
import AppRoutes from "./routing/app-routes";

function App() {
  return (
    <I18nextProvider i18n={i18n}>
      <BrowserRouter>
        <AuthProvider>
          <ErrorBoundary>
            <ThemeProvider>
              <AppLayout>
                <AppRoutes />
              </AppLayout>
            </ThemeProvider>
          </ErrorBoundary>
        </AuthProvider>
      </BrowserRouter>
    </I18nextProvider>
  );
}
export default App;
