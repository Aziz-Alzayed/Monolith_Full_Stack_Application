import { FC } from "react";
import aboutImage from "../assets/images/about.jpg";

export const AboutSection: FC = () => {
  const aboutContent = {
    title: "About Us",
    description:
      "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Xonsectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
    bulletPoints: [
      "Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit.",
      "Sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt.",
      "Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet.",
      "Consectetur, adipisci velit, sed quia non numquam eius.",
      "Rempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem.",
      "Sed do eiusmod tempor incididunt ut labore et dolore",
      "Enim ad minima veniam, quis nostrum exercitationem ullam corporis.",
    ],
  };

  return (
    <section id="about" className="about">
      <div className="container" data-aos="fade-up">
        <div className="section-header">
          <h2>{aboutContent.title}</h2>
          <p>{aboutContent.description}</p>
        </div>
        <div className="row gy-4">
          <div className="col-lg-4">
            <img src={aboutImage} className="img-fluid rounded-4 mb-4" alt="" />
          </div>
          <div className="col-lg-8">
            <div className="content ps-0 ps-lg-5">
              <p>{aboutContent.description}</p>
              <ul>
                {aboutContent.bulletPoints.map((point, index) => (
                  <li key={index}>
                    <i className={`bi bi-${index + 1}-square`}></i> {point}
                  </li>
                ))}
              </ul>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
};
