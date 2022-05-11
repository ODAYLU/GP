var loadFile = function (event) {
    var image = document.getElementById("output");
  var imgs=image.src = URL.createObjectURL(event.target.files[0]);
  
  };
  
  window.addEventListener('load', (event) => {
    document.getElementById("output1").src = imgs;
  });