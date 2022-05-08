

// let btn = document.querySelector("#btn");
// let sidebar = document.querySelector(".sidebar");
// let scripthiden = document.querySelectorAll(".scripthiden");
// let content = document.querySelectorAll(".content");
// let list = document.querySelectorAll(".list");

// btn.onclick = function () {
//     sidebar.classList.toggle("active");
//     for (let i = 0; i < scripthiden.length; i++) {
//         scripthiden[i].classList.toggle("active");
//     }
//     for (let i = 0; i < content.length; i++) {
//         content[i].classList.toggle("active");
//     }
//     for (let i = 0; i < list.length; i++) {
//         list[i].classList.toggle("active");
//     }
// }


// $(document).ready(function () {
//     $("#mySearch").on("keyup", function () {
//         var value = $(this).val().toLowerCase();
//         $("#myList li").filter(function () {
//             $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
//         });
//     });
// });


let body = document.querySelector(".body");
let cont = document.getElementsByClassName("cont");
const checkbox = document.getElementById('checkbox');

var toggle = false;
checkbox.addEventListener('change', () => {
    body.classList.toggle('dark');
    sidebar.classList.toggle('dark');

    for (var i = 0; i < cont.length; i++) {
        cont[i].classList.toggle('dark');
    }

    if (toggle === false) {
        document.getElementById('changelogo').src = 'image/logodark.png';

    } else {
        document.getElementById('changelogo').src = '/image/logo.png';

    }
    toggle = !toggle;

})






$.extend(true, $.fn.dataTable.defaults, {
    "searching": true,
    "ordering": true
});
$(document).ready(function () {
    var table = $('#example').DataTable({
        search: {
            return: true,
        },
    });
    // $('#example').DataTable();
    $(".dataTables_info").hide();
    // $("#example_filter").hide();
    // $("#example_length").css({ "position": "absolute", "right": "0", "bottom": "288px" });
    // $("#example_length").html('<label>أظهر<select class="mx-3" name="example_length" aria-controls="example" class=""><option value="10" >10</option><option value="25">25</option><option value="50">50</option><option value="100">100</option></select>الخيارات</label>');
    // $("#example_filter").html('<label>ابحث:<input type="search" class placeholder aria-controls="example"></label>');
});


//js
window.onload = function e() {

    var form = document.getElementById('inserts');
    var all = document.getElementById('all');
    var oplist = form.elements.genre;
    all.addEventListener('change', checkList);
    function checkList() {
        for (var i = 0; i < oplist.length; i++) {
            oplist[i].checked = all.checked;
        }
    }

    for (var i = 0; i < oplist.length; i++) {
        oplist[i].addEventListener('change', check);
    }

    function check(e) {
        if (!e.target.checked) {
            all.checked = false;
        }
        for (var i = 0; i < oplist.length; i++) {

            if (!oplist[i].checked) {
                return;
            }

        }
        all.checked = true;
    }

    var search = document.getElementById('example_length').firstChild;
    search.innerHTML = '<label>أظهر<select class="mx-3" name="example_length" aria-controls="example" class=""><option value="10" >10</option><option value="25">25</option><option value="50">50</option><option value="100">100</option></select>الخيارات</label>';


    var list = document.getElementById('btndel');
    list.addEventListener('click', function (e) {
        DeleteItem(e);
    }, false);
    function DeleteItem(e) {
        var elm = e.target;
        var a, li;
        if (elm.nodeName.toLowerCase() == "button") {

            //a = elm.parentNode;
            //li = a.parentNode;
            //list.removeChild(li);

            li = elm.parentNode.parentNode;
            list.removeChild(li);
        }
        e.preventDefault();
    }
};





// function myFunction() {
//     var x = document.getElementById("myDIV");
//     if(window.innerWidth > 992){
//         x.style.display = "block";
//     }

//         if (x.style.display === "block") {
//             x.style.display = "none";
//         }
//         else {
//             x.style.display = "block";
//             x.style.position = "ABSOLUTE";
//             x.style.zIndex = "100";
//             x.style.width = "250px";
//             x.style.transition = "all 0.5s ease-out";
//         }
  
// }



 