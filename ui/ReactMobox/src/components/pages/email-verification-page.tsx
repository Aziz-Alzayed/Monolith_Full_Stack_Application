import { Button, notification, Result } from "antd";
import { FC, useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useAuth } from "../../auth/auth-provider/auth-provider";
import userStore from "../../stores/user-stores/user-store";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";
import CenteredSpin from "../helpers/centered-spin";

const EmailVerificationPage: FC = () => {
  const [searchParams] = useSearchParams();
  const { user, updateUser } = useAuth();
  const navigate = useNavigate();
  const navigateWithLanguage = useLanguageAwareNavigate();
  const userId = searchParams.get("userId");
  const token = searchParams.get("token");
  const [verificationStatus, setVerificationStatus] = useState<
    "loading" | "success" | "error"
  >("loading");

  useEffect(() => {
    const verifyEmail = async () => {
      if (userId && token) {
        try {
          const response = await userStore.VerifyUserEmail(userId, token);
          if (response.passed) {
            notification.success({
              message: "Email Verified",
              description: "Your email has been successfully verified.",
              duration: 2,
            });
            setVerificationStatus("success");
            if (user) {
              updateUser({ ...user, emailConfirmed: true });
            }
          } else {
            setVerificationStatus("error");
          }
        } catch {
          setVerificationStatus("error");
        } finally {
          navigate(window.location.pathname, { replace: true });
        }
      } else {
        setVerificationStatus("error");
      }
    };

    verifyEmail();
  }, [navigate]);

  const renderResult = (): JSX.Element => {
    if (verificationStatus === "loading") {
      return <CenteredSpin size="large" message="loading..." />;
    } else if (verificationStatus === "error") {
      return (
        <Result
          status="error"
          title="Email Verification Failed"
          subTitle="There was a problem verifying your email. Please try again or contact support."
          extra={
            <Button
              type="primary"
              onClick={() => navigateWithLanguage(RoutePaths.home)}
            >
              Go to Home
            </Button>
          }
        />
      );
    }
    return (
      <Result
        status="success"
        title="Email Verification Succeed"
        subTitle="All good."
        extra={
          <Button
            type="primary"
            onClick={() => navigateWithLanguage(RoutePaths.userProfile)}
          >
            Go to Profile
          </Button>
        }
      />
    );
  };
  return renderResult();
};

export default EmailVerificationPage;
