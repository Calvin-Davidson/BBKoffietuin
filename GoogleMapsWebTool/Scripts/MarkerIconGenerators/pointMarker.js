function GeneratePointMarkerIcon(size, text = "?") {
    // Create a canvas element and set its dimensions to 25 by 25 pixels
    const canvas = document.createElement('canvas');
    canvas.width = size;
    canvas.height = size;

    // Get the 2D rendering context for the canvas
    const ctx = canvas.getContext('2d');

    // Draw a blue circle with a white border
    ctx.beginPath();
    ctx.arc(size / 2, size / 2, 10, 0, 2 * Math.PI);
    ctx.fillStyle = 'blue';
    ctx.fill();
    ctx.lineWidth = 2;
    ctx.strokeStyle = 'white';
    ctx.stroke();

    // Set the font and text alignment for the number
    ctx.font = 'bold 8px sans-serif';
    ctx.textAlign = 'center';
    ctx.textBaseline = 'middle';

    // Draw the number in the center of the circle
    ctx.fillStyle = 'red';
    ctx.fillText(text, size / 2, size / 2 + 17.5);

    // Convert the canvas to a PNG image and set it as the src attribute of an <img> element
    const img = document.createElement('img');
    img.src = canvas.toDataURL('image/png');

    return img;
}
