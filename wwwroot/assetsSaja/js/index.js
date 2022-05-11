function setTheme(themeName) {
  localStorage.setItem('theme', themeName);
  document.documentElement.className = themeName;
}
(function () {
  if (localStorage.getItem('theme')) {
    setTheme(localStorage.getItem('theme'))
  } else {
    setTheme('theme-dark-blue')
  }
})();



var swiper = new Swiper(".mySwiper", {
  slidesPerView: 1,
  spaceBetween: 10,
  pagination: {
    el: ".swiper-pagination",
    clickable: true
  },

navigation: {
  nextEl: '.swiper-button-next',
  prevEl: '.swiper-button-prev',
},
  breakpoints: {

    768: {
      slidesPerView: 2,
      spaceBetween: 20
    },
    1024: {
      slidesPerView: 4,
      spaceBetween: 50
    }
  }
});