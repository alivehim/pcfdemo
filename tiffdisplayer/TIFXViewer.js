import * as React from "react";
import { useEffect, useState } from "react";
import { Image, Layer, Stage } from "react-konva";
// import UTIF from "utif";
import Tiff from 'tiff'

const TIFXViewer = (paramers) => {

  const [image, setImage] = useState();

  useEffect(() => {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", paramers.tiffurl);
    xhr.responseType = "arraybuffer";
    xhr.onload = (e) => {
     
        const arrayBuffer= e.target.response;

        Tiff.initialize({ TOTAL_MEMORY:16777216*10 });

        let tiff= new Tiff({buffer:arrayBuffer});

        var dataUri = tiff.toDataURL();

        document.getElementById("img").src= dataUri;
        


    };

    xhr.send();
  }, []);

  return (
     <div>
        <img id="img"></img>
     </div>
  )
}

export default TIFXViewer;