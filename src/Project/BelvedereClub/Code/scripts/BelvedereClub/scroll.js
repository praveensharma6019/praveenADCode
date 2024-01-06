$.easing.jswing = $.easing.swing;

$.extend($.easing,
{
    def: 'easeOutCubic',
    easeOutCubic: function (x, t, b, c, d) {
        return c*((t=t/d-1)*t*t + 1) + b;
    }
});

var clicked = false, 
    clickX,
    startX = 0;

var d = new Date();
var m, n;

// Use with modernizr to detect no touch
// $('.no-touch .js-scrolly').on({
$('.js-scrolly').on({
    'mousemove': function(e) {
        clicked && updateScrollPos(e, this);
    },
    'mousedown': function(e) {
        e.preventDefault();
        prevDist = parseInt( $(this).data('prevdist') );
        clicked = true;
        clickX = e.pageX;
        
        d = new Date();
        m = d.getTime();
    },
    'mouseup': function(e) {
        if( clicked != false ){
            clicked = false;
            $('html').css('cursor', 'auto');
            
            d = new Date();
            n = d.getTime();
            time = (n - m);
            distance = (clickX-e.pageX);
            velocity = distance/(time/120);
            console.log('time ' + time );
            console.log(distance);
            console.log('velocity '+ velocity );
            console.log('--------');
            
            $(this).data('prevdist', $(this).scrollLeft()+(velocity) );
            
            x = Math.abs(velocity*10)/2;
            console.log(x);
            
            $(this).animate({
              scrollLeft: ($(this).scrollLeft()+(velocity))
            }, 300, 'easeOutCubic');
        }
    },
    'mouseleave': function(e) {
        if( clicked != false ){
            clicked = false;
            $('html').css('cursor', 'auto');
            
            d = new Date();
            n = d.getTime();
            time = (n - m);
            distance = (clickX-e.pageX);
            velocity = distance/(time/120);
            
            $(this).data('prevdist', $(this).scrollLeft()+(velocity) );
            
            x = Math.abs(velocity*10);
            
            $(this).animate({
              scrollLeft: ($(this).scrollLeft()+(velocity))
            }, 300, 'easeOutCubic');
        }
    }
});

var updateScrollPos = function(e, me) {
    distance = clickX-e.pageX;
    $(me).scrollLeft( (prevDist+(distance)) );
}

var clickedAnchor = false,
    movedAnchor = false;

// Use with modernizr to detect no touch
// $('.no-touch .js-scrolly a').on({
$('.js-scrolly a').on({
    'mousedown': function(e) {
        clickedAnchor = true;
        movedAnchor = false;
    },
    'mousemove': function(e) {
        movedAnchor = true;
    },
    'click': function(e) {
        if( clickedAnchor == true && movedAnchor == false ){
            clickedAnchor = false;
            movedAnchor = false;
        } else {
            e.preventDefault();
            clickedAnchor = false;
            movedAnchor = false;
            return false;
        }
    }
});