import { Button, Result } from "antd";
import React, { ReactNode } from "react";
import styles from "./error-boundary.module.css";

interface ErrorBoundaryProps {
  children: ReactNode;
  navigationPath: string | undefined
}

interface ErrorBoundaryState {
  hasError: boolean;
  error?: Error;
}

class ErrorBoundary extends React.Component<
  ErrorBoundaryProps,
  ErrorBoundaryState
> {
  constructor(props: ErrorBoundaryProps) {
    super(props);
    this.state = { hasError: false };
  }

  static getDerivedStateFromError(error: Error): ErrorBoundaryState {
    return { hasError: true, error };
  }

  componentDidCatch(error: Error, errorInfo: React.ErrorInfo) {
    console.error("ErrorBoundary caught an error", error, errorInfo);
  }

  // eslint-disable-next-line @typescript-eslint/no-unused-vars, @typescript-eslint/no-explicit-any
  componentWillReceiveProps(nextProps: Readonly<ErrorBoundaryProps>, nextContext: any): void {
    if(this.state.hasError && this.props.navigationPath != nextProps.navigationPath){
        this.setState({ hasError: false, error: undefined });
    }
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className={styles.tableContainerStyles}>
          <Result
            status="error"
            title="There are some problems with your operation."
            extra={
              <Button type="primary" key="console">
                Go Console
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
