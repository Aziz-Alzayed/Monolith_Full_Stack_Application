import { FC } from "react";
import { useTranslation } from "react-i18next";
import { TranslationKeys } from "../../../../localization/translations/base-translation";

export const HeroSection: FC = () => {
  const { t } = useTranslation();
  const heroContent = {
    welcomeMessage: "FSTD.",
    subtitle: "Full stack To do App",
    iconBoxes: [
      {
        id: 1,
        icon: "bi-fullscreen",
        title: t(TranslationKeys.professionalDesign),
        delay: "100",
      },
      {
        id: 2,
        icon: "bi-headset",
        title:  t(TranslationKeys.quickSupport),
        delay: "200",
      },
      {
        id: 3,
        icon: "bi-person-check",
        title: t(TranslationKeys.satisfactionGuaranteed),
        delay: "500",
      },
    ],
  };

  return (
    <section id="hero" className="hero">
      <div className="container position-relative">
        <div className="row gy-5" data-aos="fade-in">
          <div className="col-lg-12 order-lg-1 d-flex flex-column justify-content-center text-center caption">
            <h2>
              {t(TranslationKeys.welcome)}{" "}
              <span style={{ color: "#FFD700", fontFamily: "parisienne" }}>
                {heroContent.welcomeMessage}
              </span>
            </h2>
            <p>{heroContent.subtitle}</p>
          </div>
        </div>
      </div>
      <div className="icon-boxes position-relative">
        <div className="container position-relative">
          <div className="row gy-4 mt-5">
            {heroContent.iconBoxes.map((box) => (
              <div
                className="col-xl-4 col-md-4"
                data-aos="fade-up"
                data-aos-delay={box.delay}
                key={box.id}
              >
                <div className="icon-box">
                  <div className="icon">
                    <i className={box.icon}></i>
                  </div>
                  <h4 className="title">
                    <a href="" className="stretched-link">
                      {box.title}
                    </a>
                  </h4>
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
};
