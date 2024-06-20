
import React from "react"
import ImageZoom from "react-image-zooom";
import Zoom from 'react-medium-image-zoom'
import 'react-medium-image-zoom/dist/styles.css'
export const ImageViewer = ({tiffurl}) => {

    // return (<div style={{width:"1000px",height:"800px",display:"flex",overflow:"auto"}} >
    //     <img src={tiffurl}>
    //     </img>
    // </div>)

    return (
    // <div>
    //   <ImageZoom src={tiffurl} alt="A image to apply the ImageZoom plugin" zoom="200"/>
    // </div>
    <Zoom>
    <img
      alt="That Wanaka Tree, New Zealand by Laura Smetsers"
      src={tiffurl}
      width="500"
    />
  </Zoom>
  );
}

export default ImageViewer;