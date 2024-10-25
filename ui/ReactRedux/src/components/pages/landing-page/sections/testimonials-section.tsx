import { FC } from "react";
import { Swiper, SwiperSlide } from "swiper/react";
import "swiper/css"; // Import Swiper CSS
import "swiper/css/pagination"; // Import styles for the pagination module
import "swiper/css/navigation"; // Import styles for the navigation module

// Assuming these are imported image assets
import testimonial1 from "../../landing-page/assets/images/testimonials/testimonial-1.jpg";
import testimonial2 from "../../landing-page/assets/images/testimonials/testimonial-2.jpg";
import testimonial3 from "../../landing-page/assets/images/testimonials/testimonial-3.jpg";
import testimonial4 from "../../landing-page/assets/images/testimonials/testimonial-4.jpg";

// Import required modules
import { Pagination, Navigation, Autoplay } from "swiper/modules";

const TestimonialsSection: FC = () => {
  const testimonials = [
    {
      img: testimonial1,
      name: "Jhone Doe",
      position: "CFO",
      text: "Proin iaculis purus consequat sem cure digni ssim donec porttitora entum suscipit rhoncus. Accusantium quam, ultricies eget id, aliquam eget nibh et. Maecen aliquam.",
    },
    {
      img: testimonial2,
      name: "Afa Rose",
      position: "Web Designer",
      text: "Export tempor illum tamen malis malis eram quae irure esse labore quem cillum quid cillum eram malis quorum velit fore eram velit sunt aliqua noster fugiat irure amet.",
    },
    {
      img: testimonial3,
      name: "Keena Lara",
      position: "Store Owner",
      text: "Enim nisi quem export duis labore cillum quae magna enim sint quorum nulla quem veniam duis minim tempor labore quem eram duis noster aute amet eram fore quis minim.",
    },
    {
      img: testimonial4,
      name: "Fizzi Brandon",
      position: "Freelancer",
      text: "Fugiat enim eram quae cillum dolore dolor amet nulla culpa multos export minim fugiat minim velit minim dolor enim duis veniam ipsum anim magna sunt elit dolore.",
    },
  ];

  return (
    <section id="testimonials" className="testimonials">
      <div className="container" data-aos="fade-up">
        <div className="section-header">
          <h2>Testimonials</h2>
          <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit</p>
        </div>
        <Swiper
          modules={[Pagination, Navigation, Autoplay]} // Specify Swiper modules
          speed={600}
          loop={true}
          autoplay={{
            delay: 5000,
            disableOnInteraction: false,
          }}
          slidesPerView={"auto"}
          pagination={{
            clickable: true,
            type: "bullets",
          }}
          navigation={true}
          breakpoints={{
            320: {
              slidesPerView: 1,
              spaceBetween: 40,
            },
            1200: {
              slidesPerView: 2,
            },
          }}
          className="slides-3 swiper"
          data-aos="fade-up"
          data-aos-delay="100"
        >
          {testimonials.map((testimonial, index) => (
            <SwiperSlide key={index}>
              <div className="testimonial-wrap">
                <div className="testimonial-item">
                  <div className="d-flex align-items-center info-box">
                    <img
                      src={testimonial.img}
                      className="testimonial-img flex-shrink-0"
                      alt=""
                    />
                    <div>
                      <h3>{testimonial.name}</h3>
                      <h4>{testimonial.position}</h4>
                      <div className="stars">
                        <i className="bi bi-star-fill"></i>
                        <i className="bi bi-star-fill"></i>
                        <i className="bi bi-star-fill"></i>
                        <i className="bi bi-star-fill"></i>
                        <i className="bi bi-star-fill"></i>
                      </div>
                    </div>
                  </div>
                  <p>
                    <i className="bi bi-quote quote-icon-left"></i>
                    {testimonial.text}
                    <i className="bi bi-quote quote-icon-right"></i>
                  </p>
                </div>
              </div>
            </SwiperSlide>
          ))}
        </Swiper>
      </div>
    </section>
  );
};

export default TestimonialsSection;
