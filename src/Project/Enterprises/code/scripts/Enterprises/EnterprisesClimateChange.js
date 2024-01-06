 $(function () {
        // Owl Carousel
        var owl = $(".owl-carousel");
        owl.owlCarousel({
          items: 3,
          margin: 10,
          // loop: true,
          nav: true,
		  responsive:{
          0: {
          items: 1, // Display only 1 item on screens less than 767px wide (phone view)
        },
          768: {
          items: 3, // Display 3 items on screens 768px wide and above (desktop view)
        },
        }
        });

        $(".owl-next1").click(function () {
          owl.trigger("next.owl.carousel");
        });
        $(".owl-prev1").click(function () {
          owl.trigger("prev.owl.carousel");
        });
      });
	  
	  


// Get all modal buttons and close buttons using common classes
var modalButtons = document.querySelectorAll(".expand-button");
var closeButtons = document.querySelectorAll(".close");
var modals = document.querySelectorAll(".modal1");

// Attach event listeners for opening modals
modalButtons.forEach(function(button, index) {
	for(let i = 0 ; i< modals.length ; i++){
		modals[i].style.display = "none";
	}
    button.addEventListener("click", function() {
        modals[index].style.display = "block";
    });
});

// Attach event listeners for closing modals
closeButtons.forEach(function(closeButton, index) {
    closeButton.addEventListener("click", function() {
        modals[index].style.display = "none";
    });
});

// Close modals when clicking outside
window.addEventListener("click", function(event) {
    modals.forEach(function(modal) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    });
});

