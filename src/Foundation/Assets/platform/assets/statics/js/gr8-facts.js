$(".video-modal, .video_center").click(function () {
    $(".video_canvas").hide();
    var video = $(this).attr("data-video") || $(this).closest(".video-modal").attr("data-video");
    $(".video-iframe").attr("src", video);
    setTimeout(() => {
        $(".video_canvas").show()
    }, 340)
});

$(".dismiss-video").click(function () {
    $(".video_canvas").hide(); $(".video-iframe").attr("src", "");
});