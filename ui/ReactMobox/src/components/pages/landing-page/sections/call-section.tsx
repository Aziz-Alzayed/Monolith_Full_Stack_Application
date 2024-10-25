import { FC } from "react";

export const CallSection: FC = () => {
  return (
    <section id="call-to-action" className="call-to-action">
      <div className="container text-center" data-aos="zoom-out">
        <h3>Read to start something great?</h3>
        <p> Lorem ipsum dolor sit amet, consectetur adipiscing elit</p>
        <a className="cta-btn" href="mailto:info@example.com">
          Conatct Us
        </a>
      </div>
    </section>
  );
};
