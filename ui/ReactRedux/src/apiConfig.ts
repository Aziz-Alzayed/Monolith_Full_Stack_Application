import i18n from "./localization/i18n";

export const apiURL = import.meta.env.VITE_API_URL || "/api";

const currentURI = window.location.origin;
const currentLanguage = i18n.language || "en"; 

export const verificationPath = `${currentURI}/${currentLanguage}/verify-email`;
export const resetPasswordPath = `${currentURI}/${currentLanguage}/reset-password`;
