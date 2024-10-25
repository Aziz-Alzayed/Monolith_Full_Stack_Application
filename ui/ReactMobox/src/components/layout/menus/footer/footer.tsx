import { Footer } from "antd/es/layout/layout";
import { FC } from "react";

export const AppFooter: FC = () => {
  return (
    <Footer id="footer" className="footer">
      <div className="container">
        <div className="row gy-4">
          <div className="col-lg-12 col-md-12 footer-info text-center">
            <div className="social-links d-flex mt-4 justify-content-center">
              <a href="#" className="twitter">
                <i className="bi bi-twitter"></i>
              </a>
              <a href="#" className="facebook">
                <i className="bi bi-facebook"></i>
              </a>
              <a href="#" className="instagram">
                <i className="bi bi-instagram"></i>
              </a>
              <a href="#" className="linkedin">
                <i className="bi bi-linkedin"></i>
              </a>
            </div>
          </div>
        </div>
      </div>

      <div className="container mt-4">
        <div className="copyright">
          &copy; Copyright{" "}
          <strong>
            <span>Aziz Alzayed</span>
          </strong>
          . All Rights Reserved
        </div>
      </div>
    </Footer>
  );
};
