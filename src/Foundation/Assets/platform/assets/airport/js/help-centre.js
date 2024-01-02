
$('.inside-search').click(function () {
    const { innerWidth: width, innerHeight: height } = window;
    if (width <= 992) {
        const searchBarHtml = document.querySelector("#searchBar").innerHTML;
        document.querySelector("#searchBarOffcanvas .offcanvas-body").innerHTML = searchBarHtml;
        jQuery('#searchBarOffcanvas').offcanvas('show')
    }
});

function closeOffcanvas() {
    const { innerWidth: width, innerHeight: height } = window;
    if (width < 992) {
        try {
            var $myOffcanvas = document.getElementById('offcanvasBottomTest');
            var $myOffBackdrop = document.getElementsByClassName('offcanvas-backdrop')[0];
            $myOffcanvas.classList.remove('show');
            $myOffcanvas.classList.add('hide');
            $myOffBackdrop.classList.remove('show');
            $myOffBackdrop.classList.add('offcanvas-hide');
            document.body.style.removeProperty('overflow');
            document.body.style.removeProperty('padding-right')
        } catch (error) {

        }
    }
}
