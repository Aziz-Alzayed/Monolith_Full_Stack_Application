import { RouterProvider } from "react-router-dom";
import AuthProvider from "./auth/auth-provider/auth-provider";
import ThemeProvider from "./components/theme-configs/config-provider";
import ErrorBoundary from "./error-handlers/error-boundary";
import { I18nextProvider } from "react-i18next";
import i18n from "./localization/i18n";
import { Provider } from "react-redux";
import store from "./stores/main-store";
import router from "./routing/app-routes";

function App() {
  return (
    <I18nextProvider i18n={i18n}>
      <Provider store={store}>
          <AuthProvider>
            <ErrorBoundary>
              <ThemeProvider>
                <RouterProvider router={router()} />
              </ThemeProvider>
            </ErrorBoundary>
          </AuthProvider>
      </Provider>
    </I18nextProvider>
  );
}
export default App;
