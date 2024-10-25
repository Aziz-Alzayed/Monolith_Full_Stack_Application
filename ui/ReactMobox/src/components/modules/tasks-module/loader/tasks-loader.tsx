import { observer } from "mobx-react-lite";
import tasksStore from "../../../../stores/user-stores/tasks-store";
import { useEffect } from "react";
import AuthenticatedComponent from "../../../../auth/auth-wrappers/authenticated-user-components";
import { Spin } from "antd";
import { Outlet } from "react-router-dom";

const TasksLoader: React.FC = observer(() => {
  const { tasks, loadAllTasks, tasksLoading } = tasksStore;

  useEffect(() => {
    if (tasks && !tasks.length) {
      loadAllTasks();
    }
  }, []);

  return (
    <Spin tip="Load Tasks..." spinning={!tasks.length && tasksLoading}>
      <Outlet />
    </Spin>
  );
});

const TasksLoaderComponent = AuthenticatedComponent(TasksLoader);
export default TasksLoaderComponent;
