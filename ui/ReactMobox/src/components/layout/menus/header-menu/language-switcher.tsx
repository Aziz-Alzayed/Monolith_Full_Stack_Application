import React from "react";
import { Button, Dropdown } from "antd";
import { GlobalOutlined } from "@ant-design/icons";
import { useTranslation } from "react-i18next";
import type { MenuProps } from "antd";
import { languages } from "../../../../localization/i18n";
import { useLocation, useNavigate } from "react-router-dom";

const LanguageSwitcher: React.FC = () => {
  const { i18n } = useTranslation();
  const navigate = useNavigate();
  const location = useLocation();

  const changeLanguage = (lng: string) => {
    i18n.changeLanguage(lng);
    // Dynamically create a regular expression based on the languages array
    const languageCodes = languages.map((language) => language.key).join("|");
    const regex = new RegExp(`^\\/(${languageCodes})`);
    // Extract the pathname without the language part
    const newPathname = location.pathname.replace(regex, "");
    // Update the URL with the selected language
    navigate(`/${lng}${newPathname}`);
  };

  const items: MenuProps["items"] = languages.map((language) => ({
    key: language.key,
    label: language.label,
    onClick: () => changeLanguage(language.key),
  }));

  return (
    <Dropdown menu={{ items }} trigger={["click"]}>
      <Button shape="circle" icon={<GlobalOutlined />} />
    </Dropdown>
  );
};

export default LanguageSwitcher;
