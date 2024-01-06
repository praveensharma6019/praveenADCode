var tabbedContent = function() {
    var tab = document.getElementsByClassName("tab-link");
    var tabContent = document.getElementsByClassName("tab-content");
    for (var i = 0; i < tab.length; i++) {
            tab[i].addEventListener('click', function() {
                    for (var i = 0; i < tab.length; i++) {
                            tab[i].classList.remove('current');
                    };
                    for (var i = 0; i < tabContent.length; i++) {
                            tabContent[i].classList.remove('current');
                    };
                    this.className += ' current';
                    var matchingTab = this.getAttribute('data-tab');
                    document.getElementById(matchingTab).className += ' current';
            }, false);
    }
}
tabbedContent();
setTimeout(function(){
$('.tab-content').first().addClass('current');   
},50);