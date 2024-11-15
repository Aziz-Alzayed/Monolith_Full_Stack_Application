import { Button, Result } from "antd";
import React, { ReactNode } from "react";
import styles from "./error-boundary.module.css";

interface ErrorBoundaryProps {
  children: ReactNode;
}

interface ErrorBoundaryState {
  hasError: boolean;
  error?: Error;
  currentPath: string;
}

class ErrorBoundary extends React.Component<ErrorBoundaryProps, ErrorBoundaryState> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false, currentPath: window.location.pathname };
  }

  static getDerivedStateFromError(error: Error): ErrorBoundaryState {
    return { hasError: true, error, currentPath: window.location.pathname };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error("ErrorBoundary caught an error", error, errorInfo);
  }

  componentDidMount() {
    // Listen for navigation changes
    window.addEventListener("popstate", this.handlePathChange);
  }

  componentWillUnmount() {
    // Clean up the event listener
    window.removeEventListener("popstate", this.handlePathChange);
  }

  handlePathChange = () => {
    const newPath = window.location.pathname;
    if (this.state.hasError && this.state.currentPath !== newPath) {
      this.setState({ hasError: false, error: undefined, currentPath: newPath });
    }
  };

  render() {
    if (this.state.hasError) {
      return (
        <div className={styles.tableContainerStyles}>
          <Result
            status="error"
            title="There are some problems with your operation."
            extra={
              <Button type="primary" key="console" onClick={() => window.location.reload()}>
                Reload Page
              </Button>
            }
          />
        </div>
      );
    }

    return this.props.children;
  }
}

export default ErrorBoundary;
