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








//cards

window.addEventListener('load', (event) => {
    document.getElementsByClassName('swiper-pagination')[0].children[4].click();


});















$(function () {
    $('.material-card > .mc-btn-action').click(function () {
        var card = $(this).parent('.material-card');
        var icon = $(this).children('i');
        icon.addClass('fa-spin-fast');

        if (card.hasClass('mc-active')) {
            card.removeClass('mc-active');

            window.setTimeout(function () {
                icon
                    .removeClass('fa-arrow-left')
                    .removeClass('fa-spin-fast')
                    .addClass('fa-bars');

            }, 800);
        } else {
            card.addClass('mc-active');

            window.setTimeout(function () {
                icon
                    .removeClass('fa-bars')
                    .removeClass('fa-spin-fast')
                    .addClass('fa-arrow-left');

            }, 800);
        }
    });
});
function myFunction() {
    var input, filter, ul, article, li, strong, a, i;
    input = document.getElementById("mySearch");
    filter = input.value.toUpperCase();
    li = document.getElementsByClassName("article");

    const collection = document.getElementsByClassName("n");
    for (let i = 0; i < collection.length; i++) {
        if (collection[i].innerHTML.toUpperCase().indexOf(filter) > -1) {
            li[i].style.display = "";
        } else {
            li[i].style.display = "none";

        }
    }
}
