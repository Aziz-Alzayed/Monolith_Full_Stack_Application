import React from "react";
import { Spin, Typography } from "antd";
import styles from "./centered-spin.module.css";

const { Text } = Typography;

interface CenteredSpinProps {
  message?: string;
  size?: "small" | "default" | "large";
}

const CenteredSpin: React.FC<CenteredSpinProps> = ({
  message = "Loading...",
  size = "large",
}) => {
  return (
    <div className={styles.spinDiv}>
      <Spin size={size} />
      {message && <Text style={{ marginTop: 16 }}>{message}</Text>}
    </div>
  );
};

export default CenteredSpin;
