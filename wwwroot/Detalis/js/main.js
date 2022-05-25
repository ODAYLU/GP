var main = function() {
   $('.btn1').click(function() {
     var post = $('.status-box').val();
    
     
     var txt2 = "";
     txt2 = $("").text(post);
     var txt3 = "";
     txt3 += `<li class = text-end>
     
     
     <div class = spase row> 
     <div class = col-1> <img class= rounded-circle mt-6 me-2"  src= assets/img/man.png  width= 70 height= 70 /> </div>
     <div class = col-8 ><h5> ${post} </h5> </div>
     <div class = col-3>
     
     <div class = > 
     <svg xmlns="http://www.w3.org/2000/svg" width="30" height="30"   class="bi bi-reply-all-fill btn3 " viewBox="0 0 16 16">
  <path d="M8.021 11.9 3.453 8.62a.719.719 0 0 1 0-1.238L8.021 4.1a.716.716 0 0 1 1.079.619V6c1.5 0 6 0 7 8-2.5-4.5-7-4-7-4v1.281c0 .56-.606.898-1.079.62z"/>
  <path d="M5.232 4.293a.5.5 0 0 1-.106.7L1.114 7.945a.5.5 0 0 1-.042.028.147.147 0 0 0 0 .252.503.503 0 0 1 .042.028l4.012 2.954a.5.5 0 1 1-.593.805L.539 9.073a1.147 1.147 0 0 1 0-1.946l3.994-2.94a.5.5 0 0 1 .699.106z"/>
</svg>

       </div> 
     
  

      
   
  
   
   
     
     </li>`;
    //  $(txt1).prependTo('.posts');
     $('.ss').replaceWith( $(txt3).prependTo('.posts'));
    
     $('.status-box').val('');
     $('.counter').text('250');


     $('.btn1').addClass('disabled');
   });
   $('.status-box').keyup(function() {
     var postLength = $(this).val().length;
     var charactersLeft = 250 - postLength;
     $('.counter').text(charactersLeft);
     if (charactersLeft < 0) {
       $('.btn1').addClass('disabled');
     } else if (charactersLeft === 250) {
       $('.btn1').addClass('disabled');
     } else {
       $('.btn1').removeClass('disabled');
     }
   });
 }
 $('.btn1').addClass('disabled');
 $(document).ready(main)
 ////card
 
 //////////////////
 function copy(that) {
  var inp = document.createElement('input');
  document.body.appendChild(inp)
  inp.value =that.textContent
  inp.select();
  document.execCommand('copy', false);
  inp.remove();
}
$(function() {
  $('[data-toggle="tooltip"]').tooltip()
})
function initMap() {
      var mapOptions = {
          center: new google.maps.LatLng(37.77051595394892, -122.3863952410404),
          mapTypeId: google.maps.MapTypeId.ROADMAP,
          /* ()أنواع 4)نوع الخريطة*/
          zoom: 16 // The largest 16 (zooming)
      };
      var elm = document.getElementById('map');
      var mymap = new google.maps.Map(elm, mapOptions); // create an object (new)

      var pinlocation = new google.maps.LatLng(37.77051595394892, -122.3863952410404);
      var startposition = new google.maps.Marker({
          position: pinlocation,
          map: mymap,
          icon: "images/Untitled-1.png"

      });
  }
  window.onload = function() {
      var code = document.createElement('script');
      code.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyAXuizXxXdy3kjKyHQrjt7femrQR2QtZaE&callback=initMap";
      document.body.appendChild(code);
  }

  /////////////////////////
  $(document).ready(function(){
    $('.your-class').slick();
  });
  
  $('.modal').on('shown.bs.modal', function (e) {
    $('.your-class').slick('setPosition');
    $('.wrap-modal-slider').addClass('open');
  })
  
  