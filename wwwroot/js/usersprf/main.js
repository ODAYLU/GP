$(function () {
    $(".material-card > .mc-btn-action").click(function () {
      var card = $(this).parent(".material-card");
      var icon = $(this).children("i");
      icon.addClass("fa-spin-fast");
  
      if (card.hasClass("mc-active")) {
        card.removeClass("mc-active");
  
        window.setTimeout(function () {
          icon
            .removeClass("fa-arrow-left")
            .removeClass("fa-spin-fast")
            .addClass("fa-bars");
        }, 800);
      } else {
        card.addClass("mc-active");
  
        window.setTimeout(function () {
          icon
            .removeClass("fa-bars")
            .removeClass("fa-spin-fast")
            .addClass("fa-arrow-left");
        }, 800);
      }
    });
  });
  
  /////////////////////////////////////////////////////////////////////////////
  $(document).ready(function() {
    $(".form-control").focusin(function() {
      $(".search").addClass("search-focus");
    });
    $(".form-control").focusout(function() {
      $(".search").removeClass("search-focus");
    });
  });
  /////////////////////////////////////////////////////////////////
  function myFunction() {
    // Declare variables
    var input, filter, ul, txt, a, i, txtValue;
    input = document.getElementById('myInput');
    filter = input.value.toUpperCase();
    ul = document.getElementById("myUL");
    txt = ul.getElementsByClassName('txt');
  
    // Loop through all list items, and hide those who don't match the search query
    for (i = 0; i < txt.length; i++) {
      a = txt[i].getElementsByClassName("A")[0];
      txtValue = a.textContent || a.innerText;
      if (txtValue.toUpperCase().indexOf(filter) > -1) {
        txt[i].style.display = "";
      } else {
        txt[i].style.display = "none";
      }
    }
  }