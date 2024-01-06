
// Add event listeners to the tabs
var tabs = document.getElementsByClassName('tab');
for (var i = 0; i < tabs.length; i++) {
    tabs[i].addEventListener('click', function () {
        // Remove active class from all tabs
        for (var j = 0; j < tabs.length; j++) {
            tabs[j].classList.remove('active');
            tabs[j].parentElement.classList.remove('line-bs');
            tabs[j].parentElement.classList.remove('line-E');
            tabs[j].parentElement.classList.remove('line-S');
            tabs[j].parentElement.classList.remove('line-G');
        }
        // Add active class to the clicked tab
        this.classList.add('active');
        if (this.classList.contains('ESG-head-E-goals')) {
            this.parentElement.classList.add('line-E');
        }
        else if (this.classList.contains('ESG-head-S-goals')) {

            this.parentElement.classList.add('line-S');

        }
        else {
            this.parentElement.classList.add('line-G');
        }


        // Get the target tab pane and remove active class from all tab panes
        var target = this.getAttribute('data-tab');
        var tabPanes = document.getElementsByClassName('tab-pane');
        for (var k = 0; k < tabPanes.length; k++) {
            tabPanes[k].classList.remove('active');
            tabPanes[k].parentElement.classList.remove('line-bs');
        }
        // Add active class to the target tab pane
        document.getElementById(target).classList.add('active');
    });
}