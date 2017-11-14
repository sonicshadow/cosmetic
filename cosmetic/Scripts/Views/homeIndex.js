$(function () {
    var swiper = new Swiper('.homeIndex-swiper .swiper-container', {
        pagination: '.homeIndex-swiper .swiper-pagination',
        paginationClickable: true,
        //autoplay: 5000,
        //loop: true,
    });

    $(".benefits-list-title").dotdotdot(function () {
        wrap		: 'letter'
    });
})