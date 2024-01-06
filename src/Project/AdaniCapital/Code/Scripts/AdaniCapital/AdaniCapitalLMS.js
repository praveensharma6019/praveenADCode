(function ($) {
    $.fn.calculateMortgage = function (options) {
        var defaults = {
            currency: '&euro;',
            params: {}
        };
        options = $.extend(defaults, options);

        var calculate = function (params) {
            params = $.extend({
                balance: 0,
                rate: 0,
                term: 0,
                period: 0
            }, params);

            var N = params.term * params.period;
            var I = (params.rate / 100) / params.period;
            var v = Math.pow((1 + I), N);
            var t = (I * v) / (v - 1);
            var result = params.balance * t;

            return result;

        };

        return this.each(function () {
            var $element = $(this);
            var $result = calculate(options.params);
            var output = '<p>' + options.currency + ' ' + $result.toFixed(2) + '</p>';
            $(output).appendTo($element);


        });

    };


})(jQuery);

$(function () {
    $('#test').on('submit', function (e) {
        e.preventDefault();
        var $params = {
            balance: $('#balance').val(),
            rate: $('#rate').val(),
            term: $('#term').val(),
            period: $('option:selected', '#period').val()
        };

        $(this).calculateMortgage({
            params: $params
        })

    });


});
