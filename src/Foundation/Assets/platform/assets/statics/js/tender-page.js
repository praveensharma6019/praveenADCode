window.onscroll = function () {
  try {
  let horizontaltabHeader = document.getElementById("navbar-tabs");
  if (window.pageYOffset < horizontaltabHeader.offsetTop) {
        document.getElementsByClassName("tab-nav")[0].classList.add('active');
  }
} catch (error) {       
}
};