  document.addEventListener("DOMContentLoaded", function () {
    document.getElementById("social-box").style.display = "none";
    document.getElementById("governance-box").style.display = "none";
    const showLinks = document.querySelectorAll(".show-link-highlights");
    showLinks.forEach(function (link) {
        link.addEventListener("click", function (e) {
            e.preventDefault();
            const coolBoxes = document.querySelectorAll(".cool-box");
            const hrHighlights = document.querySelectorAll(".hr-highlight");

            // Hide all cool-box elements
            coolBoxes.forEach(function (box) {
                box.style.display = "none";
            });

            // Hide all hr-highlight elements
            hrHighlights.forEach(function (hr) {
                hr.style.display = "none";
            });

            const target = this.getAttribute("data-target");
            document.getElementById(target + "-box").style.display = "flex";

            // Show the hr-highlight element for the clicked tab
            this.querySelector(".hr-highlight").style.display = "block";

            // Remove the "active" class from all tabs
            showLinks.forEach(function (tab) {
                tab.classList.remove("active");
            });

            // Add the "active" class to the clicked tab
            this.classList.add("active");
        });
    });
});
