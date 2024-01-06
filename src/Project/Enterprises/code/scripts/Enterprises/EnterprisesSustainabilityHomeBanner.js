document.addEventListener("DOMContentLoaded", function () {
        var deskVideos = document.querySelectorAll(".desk-video");
        var mobVideos = document.querySelectorAll(".mob-video");
        var activeSlide = document.querySelector(".carousel-item.active");
        var activeDeskVideos = activeSlide.querySelectorAll(".desk-video");
        var activeMobVideos = activeSlide.querySelectorAll(".mob-video");

        function playVideos(videos) {
            videos.forEach(function (video) {
                video.play();
            });
        }

        function pauseVideos(videos) {
            videos.forEach(function (video) {
                video.pause();
            });
        }

        playVideos(activeDeskVideos);
        playVideos(activeMobVideos);

        document.querySelector(".carousel").addEventListener("slid.bs.carousel", function () {
            activeSlide = document.querySelector(".carousel-item.active");
            activeDeskVideos = activeSlide.querySelectorAll(".desk-video");
            activeMobVideos = activeSlide.querySelectorAll(".mob-video");

            pauseVideos(deskVideos);
            pauseVideos(mobVideos);

            playVideos(activeDeskVideos);
            playVideos(activeMobVideos);
        });
    });