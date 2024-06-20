// import { TIFFViewer } from 'react-tiff'
import { TIFFViewer } from './tiffcontainer/index'
import { useEffect, useState, useCallback } from "react";
import * as React from "react";

import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";


//  function useForceUpdate() {
//     const [value, setState] = useState(true);
//     return () => setState(!value);
// }

const TIFXViewer = ({ url }) => {

  const [tiffurl, setTIFFImage] = useState();
  // const handleForceupdateMethod = useForceUpdate();

  // const [, updateState] = useState();
  // const handleForceupdateMethod = useCallback(() => updateState({}), []);


  if (tiffurl != url) {
    setTIFFImage(url)
    console.log("new:" + url)
    // handleForceupdateMethod();
  }



  return (
    // <div>{tiffurl}</div>
    <div style={{ overflow: "auto", width: "100%", height: "100%" }}>
      <TIFFViewer
        tiff={tiffurl}
        lang='en' // en | de | fr | es | tr | ja | zh | ru | ar | hi
        paginate='ltr' // bottom | ltr
        buttonColor='#141414'
        printable
      />
    </div>
  )
}

export default TIFXViewer;