import { FC } from "react";

import team1 from "../assets/images/team/team-1.jpg";
import team2 from "../assets/images/team/team-2.jpg";
import team3 from "../assets/images/team/team-3.jpg";
import team4 from "../assets/images/team/team-4.jpg";

export const TeamsSection: FC = () => {
  const teamMembers = [
    {
      name: "Jhone Bi",
      position: "Application Manager",
      image: team1,
      socials: {
        twitter: "#",
        facebook: "#",
        linkedin: "#",
        instagram: "#",
      },
    },
    {
      name: "Sani Awesome",
      position: "Social Media",
      image: team2,
      socials: {
        twitter: "#",
        facebook: "#",
        linkedin: "#",
        instagram: "#",
      },
    },
    {
      name: "Andrio Willi",
      position: "Content Writer",
      image: team3,
      socials: {
        twitter: "#",
        facebook: "#",
        linkedin: "#",
        instagram: "#",
      },
    },
    {
      name: "Afa Jonson",
      position: "Business Manager",
      image: team4,
      socials: {
        twitter: "#",
        facebook: "#",
        linkedin: "#",
        instagram: "#",
      },
    },
  ];

  return (
    <section id="team" className="team sections-bg">
      <div className="container" data-aos="fade-up">
        <div className="section-header">
          <h2>Our Team</h2>
          <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit</p>
        </div>
        <div className="row gy-4">
          {teamMembers.map((member, index) => (
            <div
              className={`col-xl-3 col-md-6 d-flex`}
              data-aos="fade-up"
              data-aos-delay={`${100 * (index + 1)}`}
            >
              <div className="member">
                <div className="social">
                  <a href={member.socials.twitter}>
                    <i className="bi bi-twitter"></i>
                  </a>
                  <a href={member.socials.facebook}>
                    <i className="bi bi-facebook"></i>
                  </a>
                  <a href={member.socials.linkedin}>
                    <i className="bi bi-linkedin"></i>
                  </a>
                  <a href={member.socials.instagram}>
                    <i className="bi bi-instagram"></i>
                  </a>
                </div>
                <img src={member.image} className="img-fluid" alt="" />
                <h4>{member.name}</h4>
                <span>{member.position}</span>
              </div>
            </div>
          ))}
        </div>
      </div>
    </section>
  );
};
