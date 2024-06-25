
import React from "react"
import ImageZoom from "react-image-zooom";
import Zoom from 'react-medium-image-zoom'
import 'react-medium-image-zoom/dist/styles.css'

import { useEffect, useState, useCallback } from "react";

export const ImageViewer = ({ url, base64 }) => {

  const [imageurl, setImageUrl] = useState(url);
 
  useEffect(() => {

    if (url) {
      console.log(url)
      setImageUrl(url)
    }
  },
    [url])

  useEffect(() => {

    if (base64) {
      console.log(base64)
      setImageUrl(`data:image/png;base64,${base64}`)
    }
  },
    [base64])

  // return (<div style={{width:"1000px",height:"800px",display:"flex",overflow:"auto"}} >
  //     <img src={tiffurl}>
  //     </img>
  // </div>)



  return (
    // <div >
    //   <ImageZoom src={imageurl} alt="A image to apply the ImageZoom plugin" zoom="200"/>
    // </div>
    <div style={{ overflow: "auto", width: "100%", height: "100%" }}>
      <Zoom>
        <img
          alt="That Wanaka Tree, New Zealand by Laura Smetsers"
          src={imageurl}
          width="500"
        />
      </Zoom>
    </div>

  );
}

export default ImageViewer;