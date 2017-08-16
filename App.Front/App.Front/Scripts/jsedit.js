jQuery(function ($) {
    "use strict";

    // Menu fixded
    var header_position = $('.header-topbar').offset();
    var window_height = $(window).height();
    $(window).on('scroll load', function (event) {
        var st = $(this).scrollTop();
        if (st > header_position.top) {
            $('.header-topbar').addClass("header-fixed");
            $('#vnt-header').addClass('headermin');


        }
        else {
            $('.header-topbar').removeClass("header-fixed").addClass('headermin');
            $('#vnt-header').removeClass('headermin');


        }
    });

    // Scroll show/hide menu on mobile
    var lastScroll = 100;
    if ($(window).width() <= 1024) {
        $(window).on('scroll load', function (event) {
            var st = $(this).scrollTop();
            if (st == 0) {
                $('.brand').removeClass('hidden');
            }
            else {
                $('.brand').addClass('hidden');
            }

            if (st > lastScroll) {
                $('.header-topbar').addClass("hidden-menu");
                $('#vnt-header').addClass('headermin');
            }
            else {
                $('.header-topbar').removeClass("hidden-menu");
                $('#vnt-header').removeClass('headermin');
            }
            lastScroll = st;
        });
    }
});