import { configureStore } from "@reduxjs/toolkit";
import adminReducer from "./slices/admin-slice";
import tasksReducer from "./slices/tasks-slice";
import userReducer from "./slices/user-slice";

const store = configureStore({
  reducer: {
    admin: adminReducer,
    tasks: tasksReducer,
    user: userReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch; // Export the correct type for dispatch

export default store;
