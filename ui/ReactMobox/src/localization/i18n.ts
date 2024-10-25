import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import { EnglishTranslation } from "./translations/en-translation";
import { FinnishTranslation } from "./translations/fi-translation";

// Create instances of the translation classes
const enTranslation = new EnglishTranslation();
const fiTranslation = new FinnishTranslation();

export const languages = [
  { key: "en", label: "English" },
  { key: "fi", label: "Finnish" },
];

const resources = {
  en: {
    translation: enTranslation,
  },
  fi: {
    translation: fiTranslation,
  },
};

i18n.use(initReactI18next).init({
  resources,
  lng: "en", // default language
  fallbackLng: "en",
  interpolation: {
    escapeValue: false, // react already safes from xss
  },
});

export default i18n;
