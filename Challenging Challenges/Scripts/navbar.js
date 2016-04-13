$(document).ready(function () {
    var trigger = $('.hamburger'),
        overlay = $('.overlay'),
       isClosed = false;

    function hamburgerCross() {

        if (isClosed == true) {
            overlay.hide();
            trigger.removeClass('is-open');
            trigger.addClass('is-closed');
            isClosed = false;
        } else {
            overlay.show();
            trigger.removeClass('is-closed');
            trigger.addClass('is-open');
            isClosed = true;
        }
    }

    trigger.click(function () {
        hamburgerCross();
    });
    $('[data-toggle="offcanvas"]').click(function () {
        $('#wrapper').toggleClass('toggled');
    });
});