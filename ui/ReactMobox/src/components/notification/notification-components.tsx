import { notification } from "antd";

export const Notification = (
  message: string,
  description: string | undefined = undefined
) => {
  notification.open({
    message,
    description,
  });
};

export const SuccessNotification = (
  message: string,
  description: string | undefined = undefined
) => {
  notification.success({
    message,
    description,
  });
};

export const WarrningNotification = (
  message: string,
  description: string | undefined = undefined
) => {
  notification.warning({
    message,
    description,
  });
};

export const ErrorNotification = (
  message: string,
  description: string | undefined = undefined
) => {
  notification.error({
    message,
    description,
  });
};
