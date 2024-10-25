interface BaseTranslationInterface {
  homePage: string;
  userManagement: string;
  tasks: string;
  password: string;
  submit: string;
  email: string;
  passwordInputMessage: string;
  passwordWarningMessage: string;
  passwordAtLeastOneDigit: string;
  passwordAtLeastOneLowCase: string;
  passwordAtLeastOneUpperCase: string;
  passwordAtLeastOneNonAlphaNum: string;
  submittingForgotPassword: string;
  failedSubmitForgotPassword: string;
  emailInputMessage: string;
  checkYourEmail: string;
  forgotPassword: string;
  failedLoginMessage: string;
  readMe: string;
  login: string;
  cancel: string;
  confirmLogout: string;
  confirmLogoutQuestion: string;
  welcome: string;
  professionalDesign: string;
  quickSupport: string;
  satisfactionGuaranteed: string;
}

export abstract class BaseTranslation implements BaseTranslationInterface {
  abstract homePage: string;
  abstract userManagement: string;
  abstract tasks: string;
  abstract password: string;
  abstract submit: string;
  abstract email: string;
  abstract passwordInputMessage: string;
  abstract passwordWarningMessage: string;
  abstract passwordAtLeastOneDigit: string;
  abstract passwordAtLeastOneLowCase: string;
  abstract passwordAtLeastOneUpperCase: string;
  abstract passwordAtLeastOneNonAlphaNum: string;
  abstract submittingForgotPassword: string;
  abstract failedSubmitForgotPassword: string;
  abstract emailInputMessage: string;
  abstract checkYourEmail: string;
  abstract forgotPassword: string;
  abstract failedLoginMessage: string;
  abstract readMe: string;
  abstract login: string;
  abstract cancel: string;
  abstract confirmLogout: string;
  abstract confirmLogoutQuestion: string;
  abstract welcome: string;
  abstract professionalDesign: string;
  abstract quickSupport: string;
  abstract satisfactionGuaranteed: string;
}

export const TranslationKeys: BaseTranslationInterface = {
  homePage: "homePage",
  userManagement: "userManagement",
  tasks: "tasks",
  password: "password",
  submit: "submit",
  email: "email",
  passwordInputMessage: "passwordInputMessage",
  passwordWarningMessage: "passwordWarningMessage",
  passwordAtLeastOneDigit: "passwordAtLeastOneDigit",
  passwordAtLeastOneLowCase: "passwordAtLeastOneLowCase",
  passwordAtLeastOneUpperCase: "passwordAtLeastOneUpperCase",
  passwordAtLeastOneNonAlphaNum: "passwordAtLeastOneNonAlphaNum",
  submittingForgotPassword: "submittingForgotPassword",
  failedSubmitForgotPassword: "failedSubmitForgotPassword",
  emailInputMessage: "emailInputMessage",
  checkYourEmail: "checkYourEmail",
  forgotPassword: "forgotPassword",
  failedLoginMessage: "failedLoginMessage",
  readMe: "readMe",
  login: "login",
  cancel: "cancel",
  confirmLogout: "confirmLogout",
  confirmLogoutQuestion: "confirmLogoutQuestion",
  welcome: "welcome",
  professionalDesign: "professionalDesign",
  quickSupport: "quickSupport",
  satisfactionGuaranteed: "satisfactionGuaranteed",
};
