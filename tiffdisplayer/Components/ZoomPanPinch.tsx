import React from "react";

import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";
import TiffDisplay from "../TIFFViewer";
import { useTiffContext } from "./TiffContext";

export const Zoom = (tiffurl:any) => {
  const { pageNumber } = useTiffContext();

  return (
        <TiffDisplay index={pageNumber} />
  );
};

export default Zoom;
