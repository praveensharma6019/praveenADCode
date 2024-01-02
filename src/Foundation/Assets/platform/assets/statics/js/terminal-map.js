/*const container = document.getElementById("myPanzoom");
const options = { click: "toggleCover" };
new Panzoom(container, options);

const container2 = document.getElementById("myPanzoom2");
const options2 = { click: "toggleCover" };
new Panzoom(container2, options2); */


const container = document.querySelectorAll("#myPanzoom, #myPanzoom2");
container?.forEach((ele)=> {
  const options = { click: "toggleCover" };
  try{
    new Panzoom(ele, options);
  }catch(e){
  }
});

