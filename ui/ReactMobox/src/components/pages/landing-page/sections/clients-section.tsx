import { FC } from "react";
import { Swiper, SwiperSlide } from "swiper/react";
import { Autoplay, Pagination } from "swiper/modules";

import client1 from "../../landing-page/assets/images/clients/client-1.png";
import client2 from "../../landing-page/assets/images/clients/client-2.png";
import client3 from "../../landing-page/assets/images/clients/client-3.png";
import client4 from "../../landing-page/assets/images/clients/client-4.png";
import client5 from "../../landing-page/assets/images/clients/client-5.png";
import client6 from "../../landing-page/assets/images/clients/client-6.png";
import client7 from "../../landing-page/assets/images/clients/client-7.png";
import client8 from "../../landing-page/assets/images/clients/client-8.png";

const ClientsSection: FC = () => {
  return (
    <section id="clients" className="clients">
      <div className="container" data-aos="zoom-out">
        <Swiper
          modules={[Autoplay, Pagination]} // Specify Swiper modules
          speed={400}
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
          breakpoints={{
            320: {
              slidesPerView: 2,
              spaceBetween: 40,
            },
            480: {
              slidesPerView: 3,
              spaceBetween: 60,
            },
            640: {
              slidesPerView: 3,
              spaceBetween: 80,
            },
            992: {
              slidesPerView: 5,
              spaceBetween: 120,
            },
          }}
        >
          <SwiperSlide>
            <img src={client1} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client2} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client3} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client4} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client5} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client6} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client7} className="img-fluid" alt="" />
          </SwiperSlide>
          <SwiperSlide>
            <img src={client8} className="img-fluid" alt="" />
          </SwiperSlide>
        </Swiper>
      </div>
    </section>
  );
};

export default ClientsSection;
