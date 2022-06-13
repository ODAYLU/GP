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






/************************************/

// article -a -h2 -p -strong document.getElementsByClassName();


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


