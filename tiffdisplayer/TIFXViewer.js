import * as React from "react";
import { useEffect, useState } from "react";
import { Image, Layer, Stage } from "react-konva";
// import UTIF from "utif";
import Tiff from 'tiff'

const TIFXViewer = () => {

  const [image, setImage] = useState();

  useEffect(() => {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", "http://127.0.0.1:5500/flaniganResume.tiff");
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