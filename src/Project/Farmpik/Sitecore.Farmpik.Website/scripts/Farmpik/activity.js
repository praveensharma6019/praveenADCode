$(function () {

    //Activities Page View more button functionality Start

    var activitiesToShowOnLoad = 5;
    var activitiesToShow = activitiesToShowOnLoad;
    var nextActivityToShow = 0;
    var totalActivityCount = $('.activity').length;
    if (totalActivityCount <= activitiesToShowOnLoad) {
        $('#ViewMore').hide();
    }
    else {
        $('#ViewMore').show();
        for (i = (activitiesToShow + 1); i <= totalActivityCount; i++) {
            $('#activity-' + i).addClass("hide");
        }
    }

    $('#ViewMore').on('click', function () {
        totalActivityCount = totalActivityCount - activitiesToShowOnLoad;
        activitiesToShow = activitiesToShow + 5;
        nextActivityToShow = nextActivityToShow + activitiesToShowOnLoad;
        for (i = (nextActivityToShow + 1); i <= activitiesToShow; i++) {
            $('#activity-' + i).removeClass("hide");
        }
        if (totalActivityCount <= activitiesToShow) {
            $('#ViewMore').hide();
        }
    });

    //Activities Page View more button functionality End

    //KnowledgeHub Page read more functionality
    var content = $('.content_area');
    var button = $('.read-more');
    var maxChar = 410;
    // Check if content exceeds max height for each pair
    for (var i = 0; i < content.length; i++) {
        if (content[i].scrollHeight > content[i].offsetHeight) {
            button.eq(i).show();
        }
        if (content[i].innerText.length < maxChar) {
            button.eq(i).hide()
        }
    }


    // Read More button click event for each button
    button.on('click', function () {
        var currentIndex = button.index($(this));
        var currentContent = content.eq(currentIndex);

        currentContent.toggleClass('show-content');
        if (currentContent.hasClass('show-content')) {
            $(this).text('Read Less');
        } else {
            $(this).text('Read More');
        }
    });
    //KnowledgeHub Page read more functionality end
});

