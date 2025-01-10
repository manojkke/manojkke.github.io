function findAllColorsInCanvas(canvas, divisiblePercetage, weight) {

   //var canvas = document.getElementsByTagName('canvas')[index]
   // Get the 2D rendering context of the canvas
   const ctx = canvas.getContext('2d');
   if (!ctx) {
      console.error('Canvas context is not available');
      return;
   }

   // Get the image data from the entire canvas
   const imageData = ctx.getImageData(0, 0, canvas.width, canvas.height);
   const pixels = imageData.data;

   // Set to store unique colors
   var colors = [''];

   var coll = new Object();
   // Iterate through the pixels (4 bytes per pixel: R, G, B, A)
   for (let i = 0; i < pixels.length; i += 4) {
      const r = pixels[i]; // Red
      const g = pixels[i + 1]; // Green
      const b = pixels[i + 2]; // Blue
      const a = pixels[i + 3]; // Alpha

      // Add color to the set as a string (e.g., "rgba(255,0,0,1)")
      var color = `rgba(${r},${g},${b},${a / 255})`;

      if (colors.indexOf(color) === -1) {
         colors.push(color);
         coll[color] = 0;
      }

      coll[color] = coll[color] + 1;
   }
   var greenPixel = coll['rgba(76,175,80,1)']
   var redPixel = coll['rgba(242,54,69,1)']

   var greenVal = 0;
   var redVal = 0;

   if (greenPixel) {
      if (divisiblePercetage) {
         greenVal = Math.floor(greenPixel / divisiblePercetage);
      } else {
         greenVal = Math.floor(greenPixel / greenPixel); 
      }
   }
   if (redPixel) { 
      if (divisiblePercetage) {
         redVal = Math.floor(redPixel / divisiblePercetage);
      } else {
         redVal = Math.floor(redPixel / redPixel);
      }
   }
      console.log('Green: ', greenVal, 'Red: ', redVal);
      return [greenVal * weight, redVal * weight]
    

}


function calculatePercentage() {
   var greenVal = 0;
   var redVal = 0;

   var chart_1 = $($('.chart-container')[0]).find('canvas');
   var chart_1_canvas_6 = findAllColorsInCanvas(chart_1[6], 0, 1.0); // single
   greenVal = greenVal + chart_1_canvas_6[0];
   redVal = redVal + chart_1_canvas_6[1];
   var chart_1_canvas_10 = findAllColorsInCanvas(chart_1[10], 0, 1.0); // single
   greenVal = greenVal + chart_1_canvas_10[0];
   redVal = redVal + chart_1_canvas_10[1];

   var chart_2 = $($('.chart-container')[2]).find('canvas');
   var chart_2_canvas_0 = findAllColorsInCanvas(chart_2[0], 600, 1.0); // multiple 3 
   greenVal = greenVal + chart_2_canvas_0[0];
   redVal = redVal + chart_2_canvas_0[1];
   var chart_2_canvas_4 = findAllColorsInCanvas(chart_2[4], 0, 1.0); // single
   greenVal = greenVal + chart_2_canvas_4[0];
   redVal = redVal + chart_2_canvas_4[1];
   var chart_2_canvas_8 = findAllColorsInCanvas(chart_2[8], 0, 1.0); // single
   greenVal = greenVal + chart_2_canvas_8[0];
   redVal = redVal + chart_2_canvas_8[1];
   var chart_2_canvas_12 = findAllColorsInCanvas(chart_2[12], 0, 1.0); // single
   greenVal = greenVal + chart_2_canvas_12[0];
   redVal = redVal + chart_2_canvas_12[1];

   var chart_3 = $($('.chart-container')[3]).find('canvas');
   var chart_3_canvas_0 = findAllColorsInCanvas(chart_3[0], 600, 1.5); // multiple 4 
   greenVal = greenVal + chart_3_canvas_0[0];
   redVal = redVal + chart_3_canvas_0[1];
   var chart_3_canvas_4 = findAllColorsInCanvas(chart_3[4], 0, 1.5); // single
   greenVal = greenVal + chart_3_canvas_4[0];
   redVal = redVal + chart_3_canvas_4[1];
   var chart_3_canvas_8 = findAllColorsInCanvas(chart_3[8], 0, 1.5); // single
   greenVal = greenVal + chart_3_canvas_8[0];
   redVal = redVal + chart_3_canvas_8[1];
   var chart_3_canvas_12 = findAllColorsInCanvas(chart_3[12], 0, 1.5); // single
   greenVal = greenVal + chart_3_canvas_12[0];
   redVal = redVal + chart_3_canvas_12[1];

   var chart_4 = $($('.chart-container')[4]).find('canvas');
   var chart_4_canvas_0 = findAllColorsInCanvas(chart_4[0], 600, 1.0); // multiple 3 
   greenVal = greenVal + chart_4_canvas_0[0];
   redVal = redVal + chart_4_canvas_0[1];
   var chart_4_canvas_4 = findAllColorsInCanvas(chart_4[4], 0, 1.0); // single 
   greenVal = greenVal + chart_4_canvas_4[0];
   redVal = redVal + chart_4_canvas_4[1];
   var chart_4_canvas_8 = findAllColorsInCanvas(chart_4[8], 0, 1.0); // single
   greenVal = greenVal + chart_4_canvas_8[0];
   redVal = redVal + chart_4_canvas_8[1];

   var chart_5 = $($('.chart-container')[5]).find('canvas');
   var chart_5_canvas_0 = findAllColorsInCanvas(chart_5[0], 600, 1.0); // multiple 3 
   greenVal = greenVal + chart_5_canvas_0[0];
   redVal = redVal + chart_5_canvas_0[1];
   var chart_5_canvas_4 = findAllColorsInCanvas(chart_5[4], 0, 1.0); // single 
   greenVal = greenVal + chart_5_canvas_4[0];
   redVal = redVal + chart_5_canvas_4[1];
   var chart_5_canvas_8 = findAllColorsInCanvas(chart_5[8], 0, 1.0); // single
   greenVal = greenVal + chart_5_canvas_8[0];
   redVal = redVal + chart_5_canvas_8[1];
   var chart_5_canvas_12 = findAllColorsInCanvas(chart_5[12], 0, 1.0); // single
   greenVal = greenVal + chart_5_canvas_12[0];
   redVal = redVal + chart_5_canvas_12[1];


   redVal = redVal * 20;
   greenVal = greenVal * 20;

   $('.chart-toolbar').html('<button style="height: 100%;width: ' + redVal + 'px;background: red;"> '+ redVal +'</button><button style="height: 100%;width: ' + greenVal + 'px;background: green;">  ' + greenVal + '</button>')

   if (redVal <= 30 || greenVal <= 30) {
    
    var soundSrc = 'https://www.myinstants.com/media/sounds/stupid-f__king-bird.mp3'
         if(chartSymbol.indexOf('ts') > -1) {
         soundSrc = 'https://raw.githubusercontent.com/manojkke/manojkke.github.io/refs/heads/main/Tesla.mp3'
         }
        if(chartSymbol.indexOf('nv') > -1) {
         soundSrc = 'https://raw.githubusercontent.com/manojkke/manojkke.github.io/refs/heads/main/Nvdia.mp3'
         }

      var audio = new Audio(soundSrc);
      audio.play();
   }
}

var chartSymbol = 'nv';
setInterval(calculatePercentage, 1500);
