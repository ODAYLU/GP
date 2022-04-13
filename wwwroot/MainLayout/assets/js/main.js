//swiper
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
  var all = document.getElementsByClassName('top-be');
  var sal = document.getElementsByClassName('sale');
  var rent = document.getElementsByClassName('rent');
  var salandrent = document.getElementsByClassName('salerent');
 
    function a(){
      
    for(var i = 0 ; i < all.length; i++ ){
        sal[i].classList.remove('dn');
        rent[i].classList.remove('dn');
        salandrent[i].classList.remove('dn');
    
    }

    }
    function b(){
      
      for(var i = 0 ; i < all.length; i++ ){
        
        rent[i].classList.add('dn');
        salandrent[i].classList.add('dn');
        sal[i].classList.remove('dn');

      }

      }
      function c(){
      
      for(var i = 0 ; i < all.length; i++ ){
        sal[i].classList.add('dn');
        salandrent[i].classList.add('dn');
        rent[i].classList.remove('dn');

      }

      }
  
      function d(){
      
      for(var i = 0 ; i < all.length; i++ ){
        rent[i].classList.add('dn');
        sal[i].classList.add('dn');
        salandrent[i].classList.remove('dn');

      }

      }
      window.addEventListener('load', (event) => {
        document.getElementsByClassName('swiper-pagination')[0].children[4].click();
       
       
});
//scroll
myID = document.getElementById("customID");

var myScrollFunc = function() {
  var y = window.scrollY;
  if (y >= 800) {
    myID.className = "cta show"
  } else {
    myID.className = "cta hide"
  }
};

window.addEventListener("scroll", myScrollFunc);