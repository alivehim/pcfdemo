import * as React from "react";
import { useEffect, useState } from "react";
import { Image, Layer, Stage } from "react-konva";
import UTIF from "utif";
import { useTiffContext } from "./Components/TiffContext";

const TIFFViewer = (page:any) => {

  const [image, setImage] = useState<HTMLCanvasElement >();
const pages = useTiffContext();
  useEffect(() => {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", pages.tiffurl);
    xhr.responseType = "arraybuffer";
    xhr.onload = (e: any) => {
      const ifds = UTIF.decode(e?.target?.response);

      const pageOfTiff = ifds[page.index];
      UTIF.decodeImage(e?.target?.response, pageOfTiff);
      const rgba = UTIF.toRGBA8(pageOfTiff);

      const imageWidth = pageOfTiff.width;
      const imageHeight = pageOfTiff.height;

      const cnv = document.createElement("canvas");
      cnv.width = imageWidth;
      cnv.height = imageHeight;
 
      const ctx = cnv.getContext("2d");
      if (ctx) {
        const imageData = ctx.createImageData(imageWidth, imageHeight);
        for (let i = 0; i < rgba.length; i++) {
          imageData.data[i] = rgba[i];
        }
        ctx.putImageData(imageData, 0, 0);
        setImage(cnv);
        console.log(cnv);
      }


    };

    xhr.send();
  }, [page.index]);
  return (
    <Stage
      width={1200}
      height={800}
      style={{ overflow:"auto",border: "1px solid #000"}}
      className={"zoom-stage"}
    >
      <Layer>
        <Image  style={{ display: "block"}} image={image} />
      </Layer>
    </Stage>
  )
}

export default TIFFViewer;