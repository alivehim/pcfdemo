// import { TIFFViewer } from 'react-tiff'
import { TIFFViewer } from './tiffcontainer/index'
import { useEffect, useState, useCallback } from "react";
import * as React from "react";
import Icon, { ActionType } from './tiffcontainer/Icon';
import Toolbar,{ defaultToolbars } from './tiffcontainer/Toolbar' 
import   { ImageDecorator, ToolbarConfig } from './tiffcontainer/Props';
import { TransformWrapper, TransformComponent } from "react-zoom-pan-pinch";

export interface ViewerCoreState {
  visible?: boolean;
  visibleStart?: boolean;
  transitionEnd?: boolean;
  activeIndex: number;
  width?: number;
  height?: number;
  top?: number;
  left?: number;
  rotate?: number;
  imageWidth: number;
  imageHeight: number;
  scaleX?: number;
  scaleY?: number;
  loading?: boolean;
  loadFailed?: boolean;
  startLoading: boolean;
}
const ACTION_TYPES = {
  setVisible: 'setVisible',
  setActiveIndex: 'setActiveIndex',
  update: 'update',
  clear: 'clear',
};
//  function useForceUpdate() {
//     const [value, setState] = useState(true);
//     return () => setState(!value);
// }

const TIFXViewer = ({ url, base64 }) => {

  const [tiffurl, setTIFFImage] = useState();
  // const [base64String, setBase64String] = useState();
  // const handleForceupdateMethod = useForceUpdate();

  // const [, updateState] = useState();
  // const handleForceupdateMethod = useCallback(() => updateState({}), []);


  // if (tiffurl != url) {
  //   setTIFFImage(url)
  //   console.log("new:" + url)
  //   // handleForceupdateMethod();
  // }

   const initialState: ViewerCoreState = {
    visible: false,
    visibleStart: false,
    transitionEnd: false,
    activeIndex: 0,
    width: 0,
    height: 0,
    top: 15,
    left: 0,
    rotate: 0,
    imageWidth: 0,
    imageHeight: 0,
    scaleX: 0,
    scaleY: 0,
    loading: false,
    loadFailed: false,
    startLoading: false,
  };

function reducer(s: ViewerCoreState, action): typeof initialState {
    switch (action.type) {
      case ACTION_TYPES.setVisible:
        return {
          ...s,
          visible: action.payload.visible,
        };
      case ACTION_TYPES.setActiveIndex:
        return {
          ...s,
          activeIndex: action.payload.index,
          startLoading: true,
        };
      case ACTION_TYPES.update:
        return {
          ...s,
          ...action.payload,
        };
      case ACTION_TYPES.clear:
        return {
          ...s,
          width: 0,
          height: 0,
          scaleX: 0,
          scaleY: 0,
          rotate: 1,
          imageWidth: 0,
          imageHeight: 0,
          loadFailed: false,
          top: 0,
          left: 0,
          loading: false,
        };
      default:
        break;
    }
    return s;
  }
    const [ state, dispatch ] = React.useReducer<(s: any, a: any) => ViewerCoreState>(reducer, initialState);


  const prefixCls = 'react-viewer';
  const zIndex = 1000;
  const attribute=true;
   const zoomable = true
   const rotatable = true
  const  scalable = true
  const changeable = true
  const downloadable=true
  const noImgDetails=true
  const customToolbar = (toolbars) => toolbars
  const activeImg: ImageDecorator = {
    src: '',
    alt: '',
  };

  function handleDefaultAction(type: ActionType) {
    switch (type) {
      // case ActionType.prev:
      //   handleChangeImg(state.activeIndex - 1);
      //   break;
      // case ActionType.next:
      //   handleChangeImg(state.activeIndex + 1);
      //   break;
      case ActionType.zoomIn:
        // let imgCenterXY = getImageCenterXY();
        // handleZoom(imgCenterXY.x, imgCenterXY.y, 1, zoomSpeed);
        break;
      case ActionType.zoomOut:
        // let imgCenterXY2 = getImageCenterXY();
        // handleZoom(imgCenterXY2.x, imgCenterXY2.y, -1, zoomSpeed);
        break;
      // case ActionType.rotateLeft:
      //   handleRotate();
      //   break;
      // case ActionType.rotateRight:
      //   handleRotate(true);
      //   break;
      // case ActionType.reset:
      //   loadImg(state.activeIndex, true);
      //   break;
      // case ActionType.scaleX:
      //   handleScaleX(-1);
      //   break;
      // case ActionType.scaleY:
      //   handleScaleY(-1);
      //   break;
      // case ActionType.download:
      //   handleDownload();
      //   break;
      default:
        break;
    }
  }

 

   function handleAction(config: ToolbarConfig) {
    handleDefaultAction(config.actionType);

    // if (config.onClick) {
    //   const activeImage = getActiveImage();
    //   config.onClick(activeImage);
    // }
  }

  useEffect(() => {

    if (url) {
      // console.log(url)
      setTIFFImage(url)
    }
  },
    [url])

    useEffect(() => {

    if (base64 && base64!='val') {
      // console.log(base64)
      setTIFFImage(base64)
    }
  },
    [base64])

  return (
    // <div>{tiffurl}</div>

    <div style={{ overflow: "auto", width: "100%", height: "100%" }}>

<div className={`${prefixCls}-footer`} style={{ zIndex: zIndex + 5 }}>
  <Toolbar
              prefixCls={prefixCls}
              onAction={handleAction}
              alt={activeImg.alt}
              width={state.imageWidth}
              height={state.imageHeight}
              attribute={attribute}
              zoomable={zoomable}
              rotatable={rotatable}
              scalable={scalable}
              changeable={changeable}
              downloadable={downloadable}
              noImgDetails={noImgDetails}
              toolbars={customToolbar(defaultToolbars)}
              activeIndex={state.activeIndex}
              count={1}
              showTotal={true}
              totalName={"11111"}
            />
</div>
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