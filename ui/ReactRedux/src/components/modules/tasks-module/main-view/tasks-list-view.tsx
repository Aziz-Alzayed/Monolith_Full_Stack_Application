import { FC, useEffect, useState, useCallback } from "react";
import { TaskModel } from "../../../../models/user-models/task-models";
import { Button, Space, Table, Popconfirm } from "antd";
import {
  CheckCircleOutlined,
  DeleteOutlined,
  EditOutlined,
  ExclamationCircleOutlined,
} from "@ant-design/icons";
import dayjs from "dayjs";
import { deleteTask, loadAllTasks } from "../../../../stores/slices/tasks-slice"; // Import Redux actions
import CommonPageTemplate from "../../../helpers/common-page-template";
import AuthenticatedComponent from "../../../../auth/auth-wrappers/authenticated-user-components";
import {
  ErrorNotification,
  SuccessNotification,
} from "../../../notification/notification-components";
import TaskModal from "../modal/task-modal";
import { getColumnSearchProps } from "../../../table-helpers/table-filter-helper";
import { AppDispatch, RootState } from "../../../../stores/main-store";
import { useDispatch, useSelector } from "react-redux";

const TaskListView: FC = () => {
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [modalMode, setModalMode] = useState<"add" | "edit">("add");
  const [currentTask, setCurrentTask] = useState<TaskModel | undefined>(
    undefined
  );
  const [, setSearchText] = useState("");
  const [, setSearchedColumn] = useState("");

  const dispatch = useDispatch<AppDispatch>();
  const tasks = useSelector((state: RootState) => Object.values(state.tasks.taskMap)); // Get tasks from Redux
  const tasksLoading = useSelector((state: RootState) => state.tasks.tasksLoading); // Get loading state

  useEffect(() => {
    if (tasks.length === 0) {
      dispatch(loadAllTasks()); // Load tasks on component mount
    }
  }, [dispatch, tasks.length]);

  const handleDelete = useCallback(
    async (id: string) => {
      try {
        const resultAction = await dispatch(deleteTask(id)).unwrap();
        if (resultAction.passed) {
          SuccessNotification("Task has been deleted!");
        } else {
          ErrorNotification("Something went wrong while deleting the task!");
        }
      } catch {
        ErrorNotification("Something went wrong while deleting the task!");
      }
    },
    [dispatch]
  );

  const handleEdit = (record: TaskModel) => {
    setCurrentTask(record); // Set the task to be edited
    setModalMode("edit"); // Set mode to edit
    setIsAddModalVisible(true); // Open modal
  };

  const handleAddNewTask = () => {
    setCurrentTask(undefined); // Clear task for adding new
    setModalMode("add"); // Set mode to add
    setIsAddModalVisible(true); // Open modal
  };

  const columns = [
    {
      title: "Name",
      dataIndex: "name",
      key: "name",
      editable: true,
      inputType: "text",
      sorter: (a: TaskModel, b: TaskModel) => a.name.localeCompare(b.name),
      ...getColumnSearchProps<TaskModel>("name", setSearchText, setSearchedColumn),
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      editable: true,
      inputType: "text",
      sorter: (a: TaskModel, b: TaskModel) => a.description.localeCompare(b.description),
      ...getColumnSearchProps<TaskModel>("description", setSearchText, setSearchedColumn),
    },
    {
      title: "Is Done",
      dataIndex: "isDone",
      key: "isDone",
      editable: true,
      inputType: "checkbox",
      render: (isDone: boolean) =>
        isDone ? <CheckCircleOutlined style={{ color: "green" }} /> : <ExclamationCircleOutlined style={{ color: "red" }} />,
      sorter: (a: TaskModel, b: TaskModel) => Number(a.isDone) - Number(b.isDone),
    },
    {
      title: "Valid Until",
      dataIndex: "validUntil",
      key: "validUntil",
      editable: true,
      inputType: "date",
      render: (date: Date) => dayjs(date).format("DD-MM-YYYY"),
      sorter: (a: TaskModel, b: TaskModel) => dayjs(a.validUntil).unix() - dayjs(b.validUntil).unix(),
    },
    {
      title: "Action",
      key: "action",
      render: (_: unknown, record: TaskModel) => {
        return (
          <Space>
            <EditOutlined onClick={() => handleEdit(record)} />
            <Popconfirm
              title="Are you sure to delete this task?"
              onConfirm={() => handleDelete(record.id)}
            >
              <DeleteOutlined />
            </Popconfirm>
          </Space>
        );
      },
    },
  ];

  const dataSource = tasks.map((task) => ({ ...task, key: task.id }));
  return (
    <CommonPageTemplate>
      <Button type="primary" onClick={handleAddNewTask} style={{ marginBottom: 16 }}>
        Add Task
      </Button>

      <Table columns={columns as never} dataSource={dataSource} rowKey="id" loading={tasksLoading} />

      <TaskModal
        visible={isAddModalVisible}
        onClose={() => setIsAddModalVisible(false)}
        task={currentTask || undefined}
        mode={modalMode}
      />
    </CommonPageTemplate>
  );
};

const TaskListViewComponent = AuthenticatedComponent(TaskListView);
export default TaskListViewComponent;