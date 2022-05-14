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







$(window).scroll(function () {
  if ($(this).scrollTop() > 1) {
    $('#navbar').addClass('black');
  }
  else {
    $('#navbar').removeClass('black');
  }
});















$(document).ready(function(){
  var zindex = 10;
  
  $("div.card").click(function(e){
    e.preventDefault();

    var isShowing = false;

    if ($(this).hasClass("show")) {
      isShowing = true
    }

    if ($("div.cards").hasClass("showing")) {
      // a card is already in view
      $("div.card.show")
        .removeClass("show");

      if (isShowing) {
        // this card was showing - reset the grid
        $("div.cards")
          .removeClass("showing");
      } else {
        // this card isn't showing - get in with it
        $(this)
          .css({zIndex: zindex})
          .addClass("show");

      }

      zindex++;

    } else {
      // no cards in view
      $("div.cards")
        .addClass("showing");
      $(this)
        .css({zIndex:zindex})
        .addClass("show");

      zindex++;
    }
    
  });
});