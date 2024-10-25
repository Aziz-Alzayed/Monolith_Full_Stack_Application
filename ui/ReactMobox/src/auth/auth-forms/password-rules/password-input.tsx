import React from "react";
import { Form, Input } from "antd";
import { Rule } from "rc-field-form/lib/interface";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../../localization/translations/base-translation";

interface PasswordRulesType {
  RequireDigit: boolean;
  RequireLowercase: boolean;
  RequireNonAlphanumeric: boolean;
  RequireUppercase: boolean;
  RequiredLength: number;
  RequiredUniqueChars: number;
}

// Simulate the PasswordRules coming from your backend or config
const PasswordRules: PasswordRulesType = {
  RequireDigit: true,
  RequireLowercase: true,
  RequireNonAlphanumeric: true,
  RequireUppercase: true,
  RequiredLength: 8,
  RequiredUniqueChars: 2, // Implementing this in UI validation is complex and might not be exact
};

const PasswordInput: React.FC = () => {
  const { t } = useTranslation();

  // An array of validation rules
  const validationRules: Rule[] = [
    { required: true, message: t(TranslationKeys.passwordInputMessage) },
    {
      min: PasswordRules.RequiredLength,
      message: `${t(TranslationKeys.passwordWarningMessage)}:  ${
        PasswordRules.RequiredLength
      }.`,
    },
    {
      validator: (_, value) =>
        PasswordRules.RequireDigit && value && !/[0-9]/.test(value)
          ? Promise.reject(
              new Error(t(TranslationKeys.passwordAtLeastOneDigit))
            )
          : Promise.resolve(),
    },
    {
      validator: (_, value) =>
        PasswordRules.RequireLowercase && value && !/[a-z]/.test(value)
          ? Promise.reject(
              new Error(t(TranslationKeys.passwordAtLeastOneLowCase))
            )
          : Promise.resolve(),
    },
    {
      validator: (_, value) =>
        PasswordRules.RequireUppercase && value && !/[A-Z]/.test(value)
          ? Promise.reject(
              new Error(t(TranslationKeys.passwordAtLeastOneUpperCase))
            )
          : Promise.resolve(),
    },
    {
      validator: (_, value) =>
        PasswordRules.RequireNonAlphanumeric &&
        value &&
        !/[^A-Za-z0-9]/.test(value)
          ? Promise.reject(
              new Error(t(TranslationKeys.passwordAtLeastOneNonAlphaNum))
            )
          : Promise.resolve(),
    },
    // Additional validators can be added here
  ];

  return (
    <Form.Item
      name="password"
      label={t(TranslationKeys.password)}
      rules={validationRules}
      hasFeedback
    >
      <Input.Password placeholder={t(TranslationKeys.password)} />
    </Form.Item>
  );
};

export default PasswordInput;
