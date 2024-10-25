import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { Spin } from "antd";
import { Outlet } from "react-router-dom";
import AuthenticatedComponent from "../../../../auth/auth-wrappers/authenticated-user-components";
import { loadAllTasks } from "../../../../stores/slices/tasks-slice";
import { AppDispatch, RootState } from "../../../../stores/main-store";

const TasksLoader: React.FC = () => {
  const dispatch = useDispatch<AppDispatch>();
  const tasks = useSelector((state: RootState) => state.tasks.taskMap);
  const tasksLoading = useSelector((state: RootState) => state.tasks.tasksLoading);

  useEffect(() => {
    if (tasks && Object.keys(tasks).length === 0) {
      dispatch(loadAllTasks());
    }
  }, [tasks, dispatch]);

  return (
    <Spin tip="Loading Tasks..." spinning={tasksLoading && Object.keys(tasks).length === 0}>
      <Outlet />
    </Spin>
  );
};

const TasksLoaderComponent = AuthenticatedComponent(TasksLoader);
export default TasksLoaderComponent;
