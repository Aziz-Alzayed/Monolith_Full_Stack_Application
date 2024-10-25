import React from "react";
import ReactDOM from "react-dom/client";
import App from "./App";
import "./index.css";
import "antd/dist/reset.css";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

const NODE_ENV = import.meta.env.NODE_ENV;
console.log("Application mode:" + NODE_ENV + " mode");
if (NODE_ENV === "development") {
  root.render(
    <React.StrictMode>
      <App />
    </React.StrictMode>
  );
} else {
  root.render(<App />);
}
