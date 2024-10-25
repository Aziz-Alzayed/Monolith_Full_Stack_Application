import { ThemeConfig } from "antd";

export const ThemeConfigs: ThemeConfig = {
  token: {
    fontFamily: "sans-serif",
  },
  components: {
    Layout: {
      headerBg: "#2A2A2A",
      siderBg: "#2A2A2A",
      triggerBg: "#2A2A2A",
    },
    Menu: {
      colorBgContainer: "#2A2A2A",
      itemColor: "#FFFFFF",
      itemHoverColor: "#E69E7f",
      colorPrimary: "#FFFFFF",
      itemSelectedBg: "#FFFFFF",
      itemSelectedColor: "black",
    },
    Button: {
      colorPrimary: "var(--color-primary)", //creame
      colorPrimaryHover: "#E69E7f", // copper
      colorPrimaryText: "#FFFFFF",
    },
    FloatButton: {
      colorPrimary: "var(--color-primary)",
      colorPrimaryHover: "#E69E7f",
    },
    Drawer: {
      colorBgElevated: "#2A2A2A",
      colorInfo: "#2A2A2A",
      colorPrimaryText: "#2A2A2A",
      colorTextHeading: "#FFFFFF",
    },
    Checkbox: {
      colorPrimary: "var(--color-primary)",
    },
    Input: {
      colorPrimary: "var(--color-primary)",
      hoverBorderColor: "#E69E7f",
    },
  },
};
