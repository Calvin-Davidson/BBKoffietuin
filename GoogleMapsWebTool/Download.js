    let wantsDownload = false;
    function Draw() {
        // Create a custom overlay to draw the map on a canvas
        class CanvasOverlay extends google.maps.OverlayView {
            constructor() {
                super();
                this.canvas = document.createElement('canvas');
            }
            onAdd() {
                const canvasProjection = this.getProjection();
                const bounds = this.getMap().getBounds();
                const ne = canvasProjection.fromLatLngToDivPixel(bounds.getNorthEast());
                const sw = canvasProjection.fromLatLngToDivPixel(bounds.getSouthWest());
                const width = ne.x - sw.x;
                const height = sw.y - ne.y;
                this.canvas.width = width;
                this.canvas.height = height;
                this.canvas.style.position = 'absolute';
                this.canvas.style.top = 0;
                this.canvas.style.left = 0;
                this.getPanes().overlayLayer.appendChild(this.canvas);
                this.draw();
            }

            draw() {
                let returnVal = {};
                const canvasProjection = this.getProjection();
                const bounds = this.getMap().getBounds();
                const ne = canvasProjection.fromLatLngToDivPixel(bounds.getNorthEast());
                const sw = canvasProjection.fromLatLngToDivPixel(bounds.getSouthWest());
                const width = ne.x - sw.x;
                const height = sw.y - ne.y;
                const ctx = this.canvas.getContext('2d');
                ctx.clearRect(0, 0, width, height);
                const mapImg = new Image();
                mapImg.crossOrigin = "Anonymous"
                mapImg.onload = () => { 
                    ctx.drawImage(mapImg, 0, 0, width, height);
                    const dataURL = this.canvas.toDataURL('image/png');
                    returnVal.base64Url = dataURL;
                    returnVal.width = width;
                    returnVal.height = height;
                    returnVal.bounds = bounds;
                    returnVal.zoom = this.getMap().getZoom();
                    if(wantsDownload){
                        wantsDownload = false; 
                        console.log("downloading");
                        downloadObjectAsJson(returnVal, "map");
                    }

                    this.getPanes().overlayLayer.removeChild(this.canvas);
                };
                mapImg.src = `https://maps.googleapis.com/maps/api/staticmap?center=${bounds.getCenter().toUrlValue()}&zoom=${this.getMap().getZoom()}&size=${parseInt(width)}x${parseInt(height)}&scale=1&format=png&maptype=roadmap&key=AIzaSyA--TK3eFeoKElqmaynPIF9QvoK1-HnpXg`;
            }
            onRemove() {
                this.canvas.parentNode.removeChild(this.canvas);
                this.canvas = null;
            }
        }

        // Add the custom overlay to the map
        const overlay = new CanvasOverlay();
        overlay.setMap(map);
    }

    function Download(img) {
        // create a download link
        const link = document.createElement('a');
        link.download = 'image.png';
        link.href = img.src;

        // simulate a click on the link to download the image
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }

    let DownloadButton = document.getElementById("DownloadImageButton");
    DownloadButton.addEventListener("click", function() {
        wantsDownload = true;
        Draw();
      });


      function downloadObjectAsJson(exportObj, exportName){
        var dataStr = "data:text/json;charset=utf-8," + encodeURIComponent(JSON.stringify(exportObj));
        var downloadAnchorNode = document.createElement('a');
        downloadAnchorNode.setAttribute("href",     dataStr);
        downloadAnchorNode.setAttribute("download", exportName + ".json");
        document.body.appendChild(downloadAnchorNode); // required for firefox
        downloadAnchorNode.click();
        downloadAnchorNode.remove();
      }