import React, { useEffect, useState } from "react";
import { Modal, Button, Form, Input, Checkbox, DatePicker } from "antd";
import dayjs from "dayjs";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../../notification/notification-components";
import { addTask, updateTask } from "../../../../stores/slices/tasks-slice";
import { TaskModel } from "../../../../models/user-models/task-models";
import { AppDispatch } from "../../../../stores/main-store";
import { useDispatch } from "react-redux";

interface TaskModalProps {
  visible: boolean;
  onClose: () => void;
  task?: TaskModel; // Optional task for editing mode
  mode: "add" | "edit"; // Mode to switch between adding and editing
}

const TaskModal: React.FC<TaskModalProps> = ({
  visible,
  onClose,
  task,
  mode,
}) => {
  const dispatch = useDispatch<AppDispatch>();
  const [form] = Form.useForm();
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (visible) {
      form.resetFields();
      if (task) {
        form.setFieldsValue({
          ...task,
          validUntil: task.validUntil ? dayjs(task.validUntil) : undefined,
        });
      }
    }
  }, [visible, task, form]);

  const handleSubmit = async () => {
    setIsLoading(true);
    form
      .validateFields()
      .then(async (values) => {
        const newTask: TaskModel = {
          ...values,
          validUntil: values.validUntil
            ? dayjs(values.validUntil).format("YYYY-MM-DD")
            : undefined,
          id: mode === "edit" && task ? task.id : "",
        };

        try {
          const result =
            mode === "add"
              ? await dispatch(addTask(newTask)).unwrap()
              : await dispatch(updateTask({ taskId: newTask.id, taskData: newTask })).unwrap();

          if (result.passed) {
            SuccessNotification(
              `The task has been ${
                mode === "add" ? "added" : "updated"
              } successfully!`
            );
            onClose();
          } else {
            ErrorNotification(
              `Error while ${mode === "add" ? "adding" : "editing"} the task!`,
              result.message
            );
          }
        } catch {
          ErrorNotification(
            `Something went wrong while ${
              mode === "add" ? "adding" : "editing"
            } the task!`
          );
        }
      })
      .catch((info) => {
        console.warn("Validate Failed:", info);
      })
      .finally(() => {
        setIsLoading(false);
      });
  };

  const handleCancel = () => {
    form.resetFields();
    onClose();
  };

  return (
    <Modal
      open={visible}
      title={mode === "add" ? "Add New Task" : "Edit Task"}
      onCancel={handleCancel}
      footer={[
        <Button key="back" onClick={handleCancel}>
          Cancel
        </Button>,
        <Button
          key="submit"
          type="primary"
          loading={isLoading}
          onClick={handleSubmit}
        >
          {mode === "add" ? "Save" : "Update"}
        </Button>,
      ]}
    >
      <Form form={form} layout="vertical" name="add_task_form">
        <Form.Item
          name="name"
          label="Task Name"
          rules={[{ required: true, message: "Please input the task name!" }]}
        >
          <Input placeholder="Enter task name" />
        </Form.Item>
        <Form.Item
          name="description"
          label="Task Description"
          rules={[
            { required: true, message: "Please input the task description!" },
          ]}
        >
          <Input placeholder="Enter task description" />
        </Form.Item>
        <Form.Item name="isDone" valuePropName="checked">
          <Checkbox>Task Completed</Checkbox>
        </Form.Item>
        <Form.Item name="validUntil" label="Valid Until">
          <DatePicker
            format="DD-MM-YYYY"
            disabledDate={(current) =>
              current && current < dayjs().startOf("day")
            }
          />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default TaskModal;
