//jQuery to collapse the navbar on scroll
$(window).scroll(function () {
    if ($(".navbar").offset().top > 50) {
        $(".navbar-fixed-top.transform").addClass("top-nav-collapse");
    } else {
        $(".navbar-fixed-top.transform").removeClass("top-nav-collapse");
    }
});

$('.navbar-nav').children().click(function () {
    var navbar = $('.navbar-main-collapse');
    navbar.removeClass('in');
    navbar.addClass('collapse');
    navbar.css('height', '1px');
});


$(function () {
    var video = document.getElementById("videoBackground");
    if (video) {
        video.playbackRate = 0.75;
    }
});

//jQuery for page scrolling feature - requires jQuery Easing plugin
$(function () {
    $('.page-scroll a').bind('click', function (event) {
        var $anchor = $(this);
        var targetEl = $($anchor.data('target'));
        if (targetEl.length === 0)
            return;

        $('html, body').stop().animate({
            scrollTop: targetEl.offset().top
        }, 1500, 'easeInOutExpo');
        event.preventDefault();
    });
});