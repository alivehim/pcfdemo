import { Card, CardBody, CardTitle } from "reactstrap";
import React from "react";
import Zoom from "./ZoomPanPinch";
import  {TiffContext}  from "./TiffContext";
import  {SelectPage}  from "./SelectPage";
export const Viewer = (tiffurl:any) => {

  return (

    // <TiffContext>
    //   <div>
    //     <Zoom />
    //   </div>
    // </TiffContext>

    <TiffContext tiffurlx={tiffurl}>
      <Card className="tiff-container d-flex justify-content-center text-center">
        <CardTitle className="card-head">React Tiff Viewer</CardTitle>
        <CardBody>
          {" "}
          <SelectPage />
          <Zoom   />
        </CardBody>
      </Card>
    </TiffContext>
  );
}

export default Viewer;