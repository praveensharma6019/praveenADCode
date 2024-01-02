$(document).ready(function () {
    $('.selectBox__value').click(function () {
        $(this).next('.custom-dd-menu').toggle();
        $select2.hide();
    });
    $('.selectBox-item__value').click(function () {
        $(this).next('.custom-dd-menu').toggle();
        $select1.hide();
    });
    var $select1 = $('#terminals'),
        $select1_a = $('#terminals a'),
        $select2 = $('#categories'),
        $select2_a = $('#categories a'),
        $options = $select2.find('a');
    var waitTerminal = setInterval(function () {
        if ($select1_a.length > 0) {
            clearInterval(waitTerminal);
            $select1_a.eq(0).click();
        }
    });
    $('#terminals a:first-child').click(); /* on page load */
    function shwoCategory() {
        $select2_a.on('click', function () {
            $(this).addClass('active').siblings().removeClass('active');
            $('.selectBox-item__value span').text($(this).text());
            var $datavalue = $(this).attr('data-value');
            $(".sddir-row").hide();
            $(".sddir-list").removeClass("data-inView");
            $(".cat-" + $datavalue).show().parent(".sddir-list").addClass("data-inView");
            if ($datavalue == "t2ia" || $datavalue == "t2da" || $datavalue == "t2id" || $datavalue == "t2dd" || $datavalue == "t1da") {
                $(".cat-" + $datavalue + ".not-all").hide();
            }
            $select2.hide();
        });
    }
    $select1_a.on('click', function () {
        $(this).addClass('active').siblings().removeClass('active');
        $('.selectBox__value span').text($(this).text());
        $select2.html($options.filter('[value="' + $(this).attr('value') + '"]'));

        let arr = []
        $("#categories a[value='" + $(this).attr('value') + "']").each(function () {
            let dpHtml = $(this).html();
            if (dpHtml.toLowerCase() !== 'all') {
                arr.push(dpHtml);
            }
        })
        setTimeout(function () {
            let index = arr.indexOf($('.selectBox-item span').html());
            if (index >= 0) {
                $select2.find('a').eq(index + 1).click();
            } else {
                $select2.find('a').eq(0).click(); // all select boxes
            }

        }, 200);
        shwoCategory();
        var $datavalue = $(this).attr('value');
        $(".sddir-list").removeClass("data-inView");
        $(".cat-" + $datavalue).show().parent(".sddir-list").addClass("data-inView");
        if ($datavalue == "t2ia" || $datavalue == "t2da" || $datavalue == "t2id" || $datavalue == "t2dd" || $datavalue == "t1da") {
            $(".cat-" + $datavalue + ".not-all").hide();
        }
        $select1.hide();
    });
});