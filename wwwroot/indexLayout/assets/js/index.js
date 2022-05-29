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












//ÇÍÏË ÇáÚÞÇÑÇÊ
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
        delay: 1000,
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

//cards

window.addEventListener('load', (event) => {
    document.getElementsByClassName('swiper-pagination')[0].children[4].click();


});
