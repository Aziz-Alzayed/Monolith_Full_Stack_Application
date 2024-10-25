import { FC } from "react";
import blog1 from "../assets/images/blog/blog-1.jpg";
import blog2 from "../assets/images/blog/blog-2.jpg";
import blog3 from "../assets/images/blog/blog-3.jpg";

export const BlogSection: FC = () => {
  return (
    <section id="recent-posts" className="recent-posts sections-bg">
      <div className="container" data-aos="fade-up">
        <div className="section-header">
          <h2>Recent Blog Posts</h2>
          <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit</p>
        </div>

        <div className="row gy-4">
          <div className="col-xl-4 col-md-6">
            <article>
              <div className="post-img">
                <img src={blog1} alt="" className="img-fluid" />
              </div>

              <p className="post-category">Domain & Hosting</p>

              <h2 className="title">
                <a href="blog-details.html">
                  How to host website on any hosting provider?
                </a>
              </h2>

              <div className="d-flex align-items-center">
                <div className="post-meta">
                  <p className="post-author">William Bla</p>
                  <p className="post-date">
                    <time dateTime="2022-01-01">Feb 1, 2022</time>
                  </p>
                </div>
              </div>
            </article>
          </div>

          <div className="col-xl-4 col-md-6">
            <article>
              <div className="post-img">
                <img src={blog2} alt="" className="img-fluid" />
              </div>

              <p className="post-category">Advertisement</p>

              <h2 className="title">
                <a href="blog-details.html">
                  How to create add on google adwords?
                </a>
              </h2>

              <div className="d-flex align-items-center">
                <div className="post-meta">
                  <p className="post-author">Jobi Ret</p>
                  <p className="post-date">
                    <time dateTime="2022-01-01">Oct 5, 2022</time>
                  </p>
                </div>
              </div>
            </article>
          </div>

          <div className="col-xl-4 col-md-6">
            <article>
              <div className="post-img">
                <img src={blog3} alt="" className="img-fluid" />
              </div>

              <p className="post-category">Marketing</p>

              <h2 className="title">
                <a href="blog-details.html">
                  What is digital marketing and why is important?
                </a>
              </h2>

              <div className="d-flex align-items-center">
                <div className="post-meta">
                  <p className="post-author">Main Dow</p>
                  <p className="post-date">
                    <time dateTime="2022-01-01">Dec 22, 2022</time>
                  </p>
                </div>
              </div>
            </article>
          </div>
        </div>
      </div>
    </section>
  );
};
