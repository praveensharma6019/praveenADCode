(function($) {
    "use strict";

    function initProducts() {
        if ('select2' in $.fn) {
            $('select[name="product_cat"]').select2();
            $('select[name="product_category"]').select2();
        }
    }
    function initProductCategoriesWidget() {
        $('ul.product-categories li.cat-parent a').click(function(event) {
            event.stopPropagation();
        });
        $('ul.product-categories li.cat-parent').click(function() {
            var item = this;
            var children = $(this).find('> ul.children');
            if (children.css('display') == 'none') {
                children.stop(true, true).slideDown();
                children.show();
                $(item).find('> a').addClass('open');
            } else {
                children.stop(true, true).slideUp(400, function() {
                    children.hide();
                    $(item).find('> a').removeClass('open');
                });
            }
        });
    }
    function initQuantity() {
        $('.quantity input[name="quantity"]').each(function() {
            var qty_el = this;
            $(qty_el).parent().find('.qty-increase').off('click.azwoo').on('click.azwoo', function() {
                var qty = qty_el.value;
                if (!isNaN(qty))
                    qty_el.value++;
            });
            $(qty_el).parent().find('.qty-decrease').off('click.azwoo').on('click.azwoo', function() {
                var qty = qty_el.value;
                if (!isNaN(qty) && qty > 1)
                    qty_el.value--;
            });
        });
        return false;
    }
    $(function() {
        initProducts();
        initProductCategoriesWidget();
        initQuantity();        
        $(document).ajaxComplete(function() {
            initQuantity();            
        });
        $(document.body).on('adding_to_cart', function(event, button, data) {
        });
    });
})(jQuery);