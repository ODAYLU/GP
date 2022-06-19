
//احدث العقارات
var swiper = new Swiper(".mySwiper", {
    pagination: {
        el: ".swiper-pagination",
        clickable: true,

    },
    effect: "coverflow",
    grabCursor: true,
    centeredSlides: true,
    autoplay:
    {
        delay: 2000,
        disableOnInteraction: false,
    },
    slidesPerView: "auto",
    coverflowEffect: {
        rotate: 50,
        stretch: 0,
        depth: 100,
        modifier: 1,
        slideShadows: true,


    },
    slidesPerView: 4,
    spaceBetween: 20,

    centeredSlides: true,
    breakpoints: {
        "@0.00": {
            slidesPerView: 1,
            spaceBetween: 5,
        },
        "@0.75": {
            slidesPerView: 2,
            spaceBetween: 10,
        },
        "@1.00": {
            slidesPerView: 3,
            spaceBetween: 15
        },
        "@1.50": {
            slidesPerView: 4,
            spaceBetween: 20,

        },
    },

});
