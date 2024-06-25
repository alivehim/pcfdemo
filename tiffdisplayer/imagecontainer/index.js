
import ImageZoom from "react-image-zooom";
import Zoom from 'react-medium-image-zoom'
import 'react-medium-image-zoom/dist/styles.css'
import React, { createRef, useEffect, useState } from 'react';
import styles from './styles.module.css'
// const WIDTH = 466;
// const HEIGHT = 326;


const imgInfo = {
    lableBottom: "492",
    lableLeft: "342",
    lableRight: "353",
    lableTop: "470",
    position: "03",

}

export const ImageViewer2 = ({ url, base64 }) => {

    // const [imageurl, setImageUrl] = useState(url);

    let canvasWidth = 0
    let canvasHeight = 0
    // let [canvasWidth, setCanvasWidth] = useState(0);
    // let [canvasHeight, setCanvasHeight] = useState(0);

    const canvasRef = createRef();
    /** 记录鼠标是否按下 */
    const [mouseDowmFlag, setMouseDowmFlag] = useState(false);
    /** 记录鼠标按下的坐标 */
    const [mouseDowmPos, setMouseDowmPos] = useState({ x: 0, y: 0 });
    /** 记录这次平移之前的距离 */
    const [offsetDis, setOffsetDis] = useState({ left: 0, top: 0 });
    /** 图片节点 */
    const [imgElement, setImgElement] = useState(new Image());
    /** 展示的图片大小 */
    const [size, setSize] = useState({ width: 0, height: 0 });

    const initImg = (url) => {
        /** 初始化一个图片节点对象 */
        const img = new Image();
        img.onload = () => {
            const ctx = canvasRef.current?.getContext('2d');
            const { naturalWidth, height, naturalHeight } = img;
            // 缩放比例
            const imgScale = height / naturalHeight;
            // 图片设定的宽度
            const width = naturalWidth * imgScale;
            // 图片相对于父元素水平垂直居中的定位
            const left = 0;
            const top = 0;

            // setCanvasHeight(height)
            // setCanvasWidth(width)
            canvasRef.current.width = width;
            canvasRef.current.height = height;
            canvasWidth = width
            canvasHeight = height
            // 画出图片
            ctx.drawImage(img, left, top, width, height);
            // 画图标
            // const labelLeft = parseInt(`${imgInfo.lableLeft}`, 10) * imgScale;
            // const labelTop = parseInt(`${imgInfo.lableTop}`, 10) * imgScale;
            // ctx.strokeStyle = '#da2727';
            // ctx.strokeRect(labelLeft, labelTop, 28, 28);

            setSize({ width, height });

            setImgElement(img);
        }
        // img.height = HEIGHT;
        img.src = url;
    }

    useEffect(() => {

        if (url) {
            console.log(url)
            initImg(url)
        }
    },
        [url])

    useEffect(() => {

        if (base64 && base64 != 'val') {
            console.log(base64)

            initImg(`data:image/png;base64,${base64}`)
            // setImageUrl(`data:image/png;base64,${base64}`)
        }
    },
        [base64])

    /** 将浏览器坐标系转化成canvas坐标系 */
    const windowToCanvas = (canvas, x, y) => {
        var canvasBox = canvas.getBoundingClientRect();
        return {
            x: (x - canvasBox.left) * (canvas.width / canvasBox.width), // //对canvas元素大小与绘图表面大小不一致时进行缩放
            y: (y - canvasBox.top) * (canvas.height / canvasBox.height),
        };
    }

    /** 图片平移 */
    const handleMouseDown = (event) => {
        event.stopPropagation();
        event.preventDefault(); // 阻止浏览器默认行为，拖动会打开图片
        const { clientX, clientY } = event;
        // 相对于canvas坐标
        const canvas = canvasRef.current;
        const pos = windowToCanvas(canvas, clientX, clientY);
        canvas.style.cursor = 'move';
        setMouseDowmFlag(true); // 控制只有在鼠标按下后才会执行mousemove
        setMouseDowmPos({
            x: pos.x,
            y: pos.y,
        });
    };

    const handleMouseMove = (event) => {
        event.stopPropagation();
        
        // event.preventDefault();
        if (!mouseDowmFlag) return;
        const { clientX, clientY } = event;
        const canvas = canvasRef.current;
        // 相对于canvas坐标
        const pos = windowToCanvas(canvas, clientX, clientY)
        // 偏移量
        const diffX = pos.x - mouseDowmPos.x;
        const diffY = pos.y - mouseDowmPos.y;
        if ((diffX === 0 && diffY === 0)) return;
        // 坐标定位 = 上次定位 + 偏移量
        const offsetX = parseInt(`${diffX + offsetDis.left}`, 10);
        const offsetY = parseInt(`${diffY + offsetDis.top}`, 10);
        // 平移图片
        const ctx = canvas.getContext('2d');
        // 清空画布
        ctx.clearRect(0, 0, canvasRef.current.width, canvasRef.current.height);
        // 画出图片
        ctx.drawImage(imgElement, offsetX, offsetY, size.width, size.height);
        // 画图标
        // const { naturalWidth, height, naturalHeight } = imgElement;
        // const imgScale = height / naturalHeight; // 缩放比例
        // const totalZoomRateX = size.width / (naturalWidth * imgScale);
        // const totalZoomRateY = size.height / height;
        // const labelLeft = parseInt(`${imgInfo.lableLeft}`, 10) * imgScale * totalZoomRateX + offsetX;
        // const labelTop = parseInt(`${imgInfo.lableTop}`, 10) * imgScale * totalZoomRateY + offsetY;
        // ctx.strokeStyle = '#da2727';
        // ctx.strokeRect(labelLeft, labelTop, 28 * totalZoomRateX, 28 * totalZoomRateY);

        // 更新按下的坐标
        setMouseDowmPos({
            x: pos.x,
            y: pos.y,
        });
        // 更新上次坐标
        setOffsetDis({
            left: offsetX,
            top: offsetY,
        })
    };

    const handleMouseUp = (event) => {
        event.stopPropagation();
        event.preventDefault();
        const canvas = canvasRef.current;
        canvas.style.cursor = 'default';
        setMouseDowmFlag(false);
    };

    /** 滚动 */
    const handleWheelImage = (event) => {
        event.stopPropagation();
        event.preventDefault();
        const canvas = canvasRef.current;
        const ctx = canvas.getContext('2d');
        // 向上为负，向下为正
        const bigger = event.deltaY > 0 ? -1 : 1;
        // 放大比例
        const enlargeRate = 1.2;
        // 缩小比例
        const shrinkRate = 0.8;

        // 缩放比例
        const { height: initHeight, naturalHeight, naturalWidth } = imgElement;
        const imgScale = initHeight / naturalHeight;

        const rate = bigger > 0 ? enlargeRate : shrinkRate;
        const width = size.width * rate;
        const height = size.height * rate;

        // 清空画布
        ctx.clearRect(0, 0, canvasRef.current.width, canvasRef.current.height);
        ctx.drawImage(imgElement, offsetDis.left, offsetDis.top, width, height);

        // 画图标
        const totalZoomRateX = width / (naturalWidth * imgScale);
        const totalZoomRateY = height / initHeight;
        const labelLeft = parseInt(`${imgInfo.lableLeft}`, 10) * imgScale * totalZoomRateX + offsetDis.left;
        const labelTop = parseInt(`${imgInfo.lableTop}`, 10) * imgScale * totalZoomRateY + offsetDis.top;
        ctx.strokeStyle = '#da2727';
        ctx.strokeRect(labelLeft, labelTop, 28 * totalZoomRateX, 28 * totalZoomRateY);

        setSize({ width, height });
        return false;
    };

    return (
        // <div >
        //   <ImageZoom src={imageurl} alt="A image to apply the ImageZoom plugin" zoom="200"/>
        // </div>
        <div style={{ overflow: "auto", width: "100%", height: "100%" }}>
            <canvas
                ref={canvasRef}

                onWheel={handleWheelImage}
                onMouseDown={handleMouseDown}
                onMouseMove={handleMouseMove}
                onMouseUp={handleMouseUp}
            ></canvas>
        </div>

        // <div className={styles.container}>

        //     <div className={styles.arrow}>
        //         <div
        //             id='tiff-inner-container'
        //             className={styles.inner}
        //         >
        //             <canvas
        //                 ref={canvasRef}
        //                 onWheel={handleWheelImage}
        //                 onMouseDown={handleMouseDown}
        //                 onMouseMove={handleMouseMove}
        //                 onMouseUp={handleMouseUp}
        //             ></canvas>
        //         </div>


        //     </div>

        // </div>

    );
}

export default ImageViewer2;