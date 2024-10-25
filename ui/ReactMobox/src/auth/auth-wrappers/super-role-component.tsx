import { Button } from "antd";
import { ComponentType, FC, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../auth-provider/auth-provider";
import { isSuper } from "../auth-services/auth-service";
import {
  RoutePaths,
  useLanguageAwareNavigate,
} from "../../routing/use-language-aware-navigate";
import CenteredSpin from "../../components/helpers/centered-spin";

const SuperRoleComponent = <P extends object>(
  WrappedComponent: ComponentType<P>
): FC<P> => {
  const WithSuperRoleCheck: FC<P> = (props) => {
    const navigate = useNavigate();
    const { user } = useAuth();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | undefined>(undefined);
    const navigateWithLanguage = useLanguageAwareNavigate();

    useEffect(() => {
      const checkRoles = async () => {
        try {
          if (!user) {
            navigateWithLanguage(RoutePaths.unauthorized); // Redirect if no user
            return;
          }

          const isAllowed = await isSuper();
          if (!isAllowed) {
            navigateWithLanguage(RoutePaths.unauthorized); // Redirect if not super user
          }
        } catch (err) {
          console.error(err);
          setError("An error occurred while checking user roles.");
        } finally {
          setLoading(false); // Loading complete
        }
      };

      checkRoles();
    }, [navigateWithLanguage, user]);

    if (loading) {
      return (
        <CenteredSpin size="small" message="Checking user role..."/>
      );
    }

    if (error) {
      return (
        <div>
          <p>{error}</p>
          <Button onClick={() => navigate(-1)}>Go Back</Button>
        </div>
      );
    }

    return <WrappedComponent {...props} />;
  };

  return WithSuperRoleCheck;
};

export default SuperRoleComponent;
