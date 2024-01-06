			/*Scroll To top JS*/
// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function() {scrollFunction()};

function scrollFunction() {
    if (document.body.scrollTop > 150 || document.documentElement.scrollTop > 150) {
        document.getElementById("scrolltop").style.display = "block";
		$('#mainMenu').addClass('stickyheader');
		$('.mainMenu').css("display","none");
    } else {
        document.getElementById("scrolltop").style.display = "none";
		$('#mainMenu').removeClass('stickyheader');
		$('.mainMenu').css("display","block");
    }
}

// When the user clicks on the button, scroll to the top of the document
function topFunction() {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
}