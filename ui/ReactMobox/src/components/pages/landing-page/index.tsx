import { FC, useEffect } from "react";
import AOS from "aos";
import TestimonialsSection from "./sections/testimonials-section";
import ClientsSection from "./sections/clients-section";
import { TeamsSection } from "./sections/teams-section";
import { BlogSection } from "./sections/blog-section";
import { QuestionsSection } from "./sections/questions-section";
import { CallSection } from "./sections/call-section";
import { AboutSection } from "./sections/about-section";
import { HeroSection } from "./sections/hero-section";
import "bootstrap/dist/js/bootstrap.bundle.min";

//#region "Css"
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap-icons/font/bootstrap-icons.css";
import "aos/dist/aos.css";
import "swiper/css";
import "../landing-page/assets/stylesheets/styles.css";
import "swiper/css/pagination";
//#endregion "Css"

function aos_init() {
  AOS.init({
    duration: 1000,
    easing: "ease-in-out",
    once: true,
    mirror: false,
  });
}

const LandingPage: FC = () => {
  useEffect(() => {
    aos_init();
  }, []);

  return (
    <>
      <HeroSection />
      <main id="main">
        <AboutSection />
        <ClientsSection />
        <CallSection />
        <TestimonialsSection />
        <TeamsSection />
        <QuestionsSection />
        <BlogSection />
      </main>
    </>
  );
};

export default LandingPage;
