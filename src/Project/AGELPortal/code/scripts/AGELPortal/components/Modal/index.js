const modal = () => {

  var elems = document.querySelectorAll(".notification-wrapper, .user-trick, .modal");
  
  document.querySelectorAll(".notification-wrapper, .user-trick").forEach((elm) => {
    if (window.screen.width < 600) {
      elm.classList.add("bottom-sheet");
      elm.classList.add("modal");
      elm.classList.remove("dd-menu-dd");
    } else {
      elm.classList.remove("bottom-sheet");
      elm.classList.remove("modal");
    }
  });
  var instances = M.Modal.init(elems, { opacity: 0.7 });
  console.log(instances);
};

export { modal };
