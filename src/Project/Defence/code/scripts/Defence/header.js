let header_html = `
<div class="header-wrapper">
<header class="header">
      <div class="container">
        <div class="row">
          <div class="col-md-3 logo-section">
            <div class="mobile-trigger">
              <span></span><span></span><span></span>
            </div>
            <a href="/">
              <img src="images/adani-logo-defence.png" alt="Defence logo" class="logo green-logo" />
              <img src="images/adani-logo-defence.png" alt="Defence logo" class="logo white-logo" style="display:none;" />
            </a>
          </div>
          <div class="col-md-9 header-nav">
            <div class="menu-section">
              <ul>
                <li class="active-link"><a href="#">About Us</a></li>
                <li><a href="#">Green X Talks</a></li>
                <li><a href="#">SDGs</a></li>
                <li><a href="#">Watch</a></li>
                <li><a href="#">Explore</a></li>
                <li><a href="#">Become A Part</a>
                  <ul class="submenu-btm">
                    <li><a href="#">Attend</a></li>
                    <li><a href="#">Speak</a></li>
                    <li><a href="#">Contribute</a></li>
                  </ul>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
      <div class="sidebar-navigation">
        <div class="offcanvas-body">
          <div class="hamburger_section logo-sec">
            <a href="/">
              <img src="images/adani-logo-defence.png" alt="Adani Defence" />
            </a>
          </div>
          <div class="hamburger_section">
            <h5>Adani Defence</h5>
            <ul>
              <li><a href="#">Home</a></li>
              <li><a href="#">About us</a></li>
              <li><a href="#">Events</a></li>
              <li><a href="#">Our Team</a></li>
              <li><a href="#">Privacy Policy</a></li>
              <li><a href="#">Check Eligibility</a></li>
              <li><a href="#">Participate</a></li>
            </ul>
          </div>
          <div class="hamburger_section single_item">
            <ul>
              <li>
                <a class="sub-menu-btn">
                  <span class="icon_box"><i class="i-businesses"></i></span>
                  <label>Category</label>
                  <i class="i-arrow-d icon"></i>
                </a>
                <div class="collapse">
                  <ul class="submenu">
                    <li><a href="#"><img src="images/icons/social-causes-category.svg" alt="All Social Causes">All Social Causes</a></li>
                    <li><a href="#"><img src="images/icons/health-medical-category.svg" alt="Social Causes">Health & Medical</a></li>
                    <li><a href="#"><img src="images/icons/environment-category.png" alt="Environment">Environment</a></li>
                    <li><a href="#"><img src="images/icons/technology-category.svg" alt="Technology">Technology</a></li>
                    <li><a href="#"><img src="images/icons/infrastructure-category.svg" alt="Infrastructure">Infrastructure</a></li>
                    <li><a href="#"><img src="images/icons/community-category.png" alt="Community">Community</a></li>
                  </ul>
                </div>
              </li>
            </ul>
          </div>
          <div class="hamburger_section">
            <h5>Information</h5>
            <ul>
              <li><a href="#">Contact us</a></li>
              <li><a href="#">Help Center</a></li>
            </ul>
          </div>
        </div>
      </div>
      <div class="overLay"></div>
    </header>
    </div>
`;

document.body.insertAdjacentHTML("afterbegin", header_html);
