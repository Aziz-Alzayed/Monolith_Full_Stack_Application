import { Button } from "antd";
import { ComponentType, FC, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth-provider/auth-provider";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";
import CenteredSpin from "../../components/helpers/centered-spin";

const AuthenticatedComponent = <P extends object>(
  WrappedComponent: ComponentType<P>
): FC<P> => {
  const WithAuthenticationCheck: FC<P> = (props) => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | undefined>(undefined);
    const navigateWithLanguage = useLanguageAwareNavigate();

    useEffect(() => {
      const checkAuthentication = async () => {
        try {
          if (!user) {
            navigateWithLanguage(RoutePaths.unauthorized);
          }
        } catch (err) {
          console.error(err);
          setError("An error occurred while checking authentication.");
        } finally {
          setLoading(false); // Loading finished
        }
      };

      checkAuthentication();
    }, [navigateWithLanguage, user]);

    if (loading) {
      return (
        <CenteredSpin size="small" message="Checking authentication..."/>
      );
    }

    if (error) {
      return (
        <div>
          <p>{error}</p>
          {/* Button to go back */}
          <Button onClick={() => navigate(-1)}>Go Back</Button>
        </div>
      );
    }

    return <WrappedComponent {...props} />;
  };

  return WithAuthenticationCheck;
};

export default AuthenticatedComponent;
