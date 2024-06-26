import React, { forwardRef, useEffect, createRef, useState } from 'react'
import styles from './styles.module.css'
import UTIF from 'utif'
import axios from 'axios'
import i18n from 'i18next'
import { useTranslation, initReactI18next } from 'react-i18next'
import PropTypes from 'prop-types'

i18n.use(initReactI18next).init({
  resources: {
    en: {
      translation: {
        Next: 'Next',
        Previous: 'Previous',
        'Page of total': 'Page {{page}} of {{total}}'
      }
    },
    tr: {
      translation: {
        Next: 'Sonraki',
        Previous: 'Önceki',
        'Page of total': '{{ page }}. sayfa / {{ total }}'
      }
    },
    de: {
      translation: {
        Next: 'Nächste',
        Previous: 'Vorherige',
        'Page of total': 'Seite {{page}} von {{total}}'
      }
    },
    fr: {
      translation: {
        Next: 'Suivant',
        Previous: 'Précédent',
        'Page of total': 'Page {{page}} sur {{total}}'
      }
    },
    es: {
      translation: {
        Next: 'Siguiente',
        Previous: 'Anterior',
        'Page of total': 'Página {{page}} de {{total}}'
      }
    },
    ja: {
      translation: {
        Next: '次へ',
        Previous: '前へ',
        'Page of total': '{{page}} / {{total}} ページ'
      }
    },
    zh: {
      translation: {
        Next: '下一页',
        Previous: '上一页',
        'Page of total': '第 {{page}} 页，共 {{total}} 页'
      }
    },
    ru: {
      translation: {
        Next: 'Следующий',
        Previous: 'Предыдущий',
        'Page of total': 'Страница {{page}} из {{total}}'
      }
    },
    ar: {
      translation: {
        Next: 'التالى',
        Previous: 'سابق',
        'Page of total': 'صفحة {{page}} من {{total}}'
      }
    },
    hi: {
      translation: {
        Next: 'अगला',
        Previous: 'पिछला',
        'Page of total': 'पृष्ठ {{page}} का {{total}}'
      }
    }
  },
  lng: 'tr',
  fallbackLng: 'en',

  interpolation: {
    escapeValue: false
  }
})

export const TIFFViewer = function TiffFileViewer(
  {
    tiff,
    lang = 'en',
    paginate = 'bottom',
    buttonColor = '#141414',
    printable = false,
    ...rest
  }
) {
  const { t, i18n } = useTranslation()

  const [size, setSize] = useState({ width: 0, height: 0 });
  const [canvasWidth, setCanvasWidth] = useState(0);
  const [canvasHeight, setCanvasHeight] = useState(0);
  const [mouseDowmFlag, setMouseDowmFlag] = useState(false);
  /** 记录鼠标按下的坐标 */
  const [mouseDowmPos, setMouseDowmPos] = useState({ x: 0, y: 0 });

  const [offsetDis, setOffsetDis] = useState({ left: 0, top: 0 });

  // console.log("x" + tiff)
  // states
  const [_tiff, setTiff] = React.useState(tiff)
  const [, setTiffs] = React.useState([])
  const [pages, setPages] = React.useState([])
  const [page, setPage] = React.useState(0)
  const [pageCount, setPageCount] = React.useState(0)

  // refs
  const canvasRef = React.useRef(null)
  const canvasXRef = React.useRef(null)
  const btnPrintRef = React.useRef(null)
  const paginateLTRRef = React.useRef(null)
  const paginateBottomRef = React.useRef(null)

  const [buffer, setBuffer] = useState([])

  const [imgdatas, setImageData] = useState([])

  if (tiff != _tiff) {
    setTiff(tiff);
  }


  const loadimagedata = (buffer1) => {

    var ifds = UTIF.decode(buffer1)

    const _imgdatas = ifds.map(function (ifd, index) {
      UTIF.decodeImage(buffer1, ifd)
      var rgba = UTIF.toRGBA8(ifd)
      var canvas = document.createElement('canvas')
      canvas.width = ifd.width
      canvas.height = ifd.height
      var ctx = canvas.getContext('2d')
      var img = ctx.createImageData(ifd.width, ifd.height)
      img.data.set(rgba)
      return img
    })

    setImageData(_imgdatas)

    setPageCount(ifds.length)
    return _imgdatas;
  }

  const readerImage = ( _imgdatas) => {

  

    if(!_imgdatas)
      return ;
    
 
    var canvas = canvasXRef.current

    canvas.width = _imgdatas[page].width
    canvas.height = _imgdatas[page].height


    // console.log(`write width ${ifds[0].width} and ${ifds[0].height} `)
    setCanvasWidth(_imgdatas[page].width)
    setCanvasHeight(_imgdatas[page].height)




    var ctx = canvas.getContext('2d')
    ctx.clearRect(0, 0,  _imgdatas[page].width , _imgdatas[page].height);
    // var img = ctx.createImageData( _imgdatas[page].width,  _imgdatas[page].height)
   
    ctx.putImageData(_imgdatas[page], 0, 0)

    // setPages([...pages, 1]);
    // setPages([...pages, 1]);

  }

  function imgLoaded(buffer1) {

    setBuffer(buffer1)
    return loadimagedata(buffer1);

    // var ifds = UTIF.decode(buffer)

    // const _tiffs = ifds.map(function (ifd, index) {


    //   var ifd = ifds[page]
    //   UTIF.decodeImage(buffer, ifd)
    //   var rgba = UTIF.toRGBA8(ifd)
    //   var canvas = document.createElement('canvas')
    //   canvas.width = ifd.width
    //   canvas.height = ifd.height


    //   console.log(`write width ${ifds[0].width} and ${ifds[0].height} `)
    //   setCanvasWidth( ifd.width)
    //   setCanvasHeight (ifd.height)

    // //   canvas.onwheel = (event) => {
    // //     console.log(event)
    // //     event.stopPropagation();

    // //     const canvas = event.target;
    // //     const ctx = canvas.getContext('2d');
    // //     // 向上为负，向下为正
    // //     const bigger = event.deltaY > 0 ? -1 : 1;
    // //     // 放大比例
    // //     const enlargeRate = 1.2;
    // //     // 缩小比例
    // //     const shrinkRate = 0.8;

    // //     const rate = bigger > 0 ? enlargeRate : shrinkRate;

    // //     const width = canvasWidth * rate;
    // //     const height = canvasHeight * rate;
    // //     console.log(width)
    // //     console.log(height)
    // //     canvasWidth = width
    // //     canvasHeight = height

    // //     ctx.clearRect(0, 0, ifd.width, ifd.height);

    // //     // ctx.drawImage(imgElement, offsetDis.left, offsetDis.top, width, height);
    // //     var img = ctx.createImageData(ifd.width, ifd.height)
    // //     img.data.set(rgba)
    // //     // const { height: initHeight, naturalHeight, naturalWidth } = img;

    // //     resizeImageData(img, width, height).then(data => {

    // //       // ctx.putImageData(data, 0, 0)
    // //       ctx.putImageData(data, offsetDis.left, offsetDis.top)

    // //       console.log(`set width:${width}`)
    // //       canvasWidth = width
    // //       canvasHeight = height
    // //     })

    // //     return false

    // //   }

    // //   canvas.onmousedown = (event) => {
    // //     event.stopPropagation();
    // //     event.preventDefault(); // 阻止浏览器默认行为，拖动会打开图片
    // //     const { clientX, clientY } = event;
    // //     // 相对于canvas坐标
    // //     const canvas = event.target;
    // //     const pos = windowToCanvas(canvas, clientX, clientY);
    // //     canvas.style.cursor = 'move';
    // //     // setMouseDowmFlag(true); // 控制只有在鼠标按下后才会执行mousemove

    // //     mouseDowmFlag=true;
    // //     // setMouseDowmPos({
    // //     //   x: pos.x,
    // //     //   y: pos.y,
    // //     // });

    // //     mouseDowmPos = {
    // //       x: pos.x,
    // //       y: pos.y,
    // //     }
    // //   }

    // //   canvas.onmousemove = (event) => {
    // //     event.stopPropagation();
    // //     event.preventDefault();
    // //     if (!mouseDowmFlag) return;
    // //     const { clientX, clientY } = event;
    // //     const canvas = event.target
    // //     // 相对于canvas坐标
    // //     const pos = windowToCanvas(canvas, clientX, clientY)
    // //     // 偏移量
    // //     const diffX = pos.x - mouseDowmPos.x;
    // //     const diffY = pos.y - mouseDowmPos.y;
    // //     if ((diffX === 0 && diffY === 0)) return;
    // //     // 坐标定位 = 上次定位 + 偏移量
    // //     const offsetX = parseInt(`${diffX + offsetDis.left}`, 10);
    // //     const offsetY = parseInt(`${diffY + offsetDis.top}`, 10);
    // //     // 平移图片
    // //     const ctx = canvas.getContext('2d');

    // //      ctx.clearRect(0, 0, ifd.width, ifd.height);

    // //     // ctx.drawImage(imgElement, offsetDis.left, offsetDis.top, width, height);
    // //     var img = ctx.createImageData(ifd.width, ifd.height)
    // //     img.data.set(rgba)
    // //     // const { height: initHeight, naturalHeight, naturalWidth } = img;

    // //     resizeImageData(img, canvasWidth, canvasHeight).then(data => {

    // //       ctx.putImageData(data, offsetX, offsetY )


    // //     })


    // //     // 更新按下的坐标
    // //     setMouseDowmPos({
    // //       x: pos.x,
    // //       y: pos.y,
    // //     });

    // //       mouseDowmPos = {
    // //       x: pos.x,
    // //       y: pos.y,
    // //     }

    // //     // 更新上次坐标
    // //     setOffsetDis({
    // //       left: offsetX,
    // //       top: offsetY,
    // //     })

    // //     offsetDis={
    // //       left: offsetX,
    // //       top: offsetY,
    // //     }
    // //   }

    // //   canvas.onmouseup = (event)=>{
    // //     event.stopPropagation();
    // // event.preventDefault();
    // // const canvas = event.target  ;
    // // canvas.style.cursor = 'default';
    // // setMouseDowmFlag(false);
    // //  mouseDowmFlag=false;
    // //   }



    //   var ctx = canvas.getContext('2d')
    //   var img = ctx.createImageData(ifd.width, ifd.height)
    //   img.data.set(rgba)
    //   ctx.putImageData(img, 0, 0)
    //   // if (index === 0)
    //   //   document.getElementById('tiff-inner-container').appendChild(canvas)

    //   return canvas
    // })



    // setPages(_tiffs)
    // setTiffs(_tiffs)
  }

  const windowToCanvas = (canvas, x, y) => {
    var canvasBox = canvas.getBoundingClientRect();
    return {
      x: (x - canvasBox.left) * (canvas.width / canvasBox.width), // //对canvas元素大小与绘图表面大小不一致时进行缩放
      y: (y - canvasBox.top) * (canvas.height / canvasBox.height),
    };
  }

  async function resizeImageData(imageData, width, height) {
    const resizeWidth = width >> 0
    const resizeHeight = height >> 0
    const ibm = await window.createImageBitmap(imageData, 0, 0, imageData.width, imageData.height, {
      resizeWidth,
      resizeHeight
    })
    const canvas = document.createElement('canvas')
    canvas.width = resizeWidth
    canvas.height = resizeHeight
    const ctx = canvas.getContext('2d', {
      willReadFrequently: true
    })
    // 不注释这一行, 得到的图像就会有缺失
    // ctx.scale(resizeWidth / imageData.width, resizeHeight / imageData.height)
    ctx.drawImage(ibm, 0, 0)
    return ctx.getImageData(0, 0, resizeWidth, resizeHeight)
  }



  async function displayTIFF(tiffUrl) {
    if (tiffUrl) {

      if (tiffUrl.startsWith("http")) {
        const response = await axios.get(tiffUrl, {
          responseType: 'arraybuffer'
        })
         var img = imgLoaded(response.data)
        readerImage(img)
      } else {
        const b = Uint8Array.from(atob(tiffUrl), (c) => c.charCodeAt(0));
        // console.log(tiffBase64);
        // const b = base642UINT8Array(tiffUrl);
      
       var img1 =  imgLoaded(b)
        readerImage(img1)
      }
    }

  }

  function displayBase64TIFF(tiffBase64) {

    if (tiffBase64) {
      // const b = Uint8Array.from(atob(tiffBase64), (c) => c.charCodeAt(0));
      console.log(tiffBase64);
      const b = base642UINT8Array(tiffBase64);
      imgLoaded(b)
    }

  }

  const base642UINT8Array = (tiffBase64) => {
    const binaryString = atob(tiffBase64)
    const bytes = new Uint8Array(binaryString.length)
    for (let i = 0; i < binaryString.length; i++) {
      bytes[i] = binaryString.charCodeAt(i);
    }
  }

  const handlePreviousClick = () => {
    if (page > 0) {
      setPage(page - 1)
    }
  }

  const handleNextClick = () => {
    if (page < pageCount - 1) {
      setPage(page + 1)
    }
  }

  const handlePrintClick = () => {
    try {
      if (printable) btnPrintRef.current.style.visibility = 'hidden'

      if (paginateLTRRef.current)
        paginateLTRRef.current.style.visibility = 'hidden'

      if (paginateBottomRef.current)
        paginateBottomRef.current.style.visibility = 'hidden'

      // all page window.print
      if (pages.length > 1) {
        pages.forEach((page, index) => {
          if (index > 0) {
            canvasRef.current.style.display = 'block'
            canvasRef.current.appendChild(page)
          }
        })
        // print
        window.print()

        // remove all pages
        pages.forEach((page, index) => {
          if (index > 0) {
            canvasRef.current.removeChild(page)
          } else {
            canvasRef.current.style.display = 'flex'
          }
        })
      } else {
        window.print()
      }
    } catch (error) {
      console.error('Error')
    } finally {
      if (printable) btnPrintRef.current.style.visibility = 'visible'

      if (paginateLTRRef.current)
        paginateLTRRef.current.style.visibility = 'visible'

      if (paginateBottomRef.current) {
        paginateBottomRef.current.style.visibility = 'visible'
      }
    }
  }

  useEffect(() => {
    displayTIFF(_tiff)
  }, [_tiff])



  useEffect(() => {
    // if (pages.length > 0) {
    //   canvasRef.current.innerHTML = ''
    //   canvasRef.current.appendChild(pages[page])
    // }

    if (buffer && buffer.length != 0) {

      console.log(page)
      console.log('page changed')
      readerImage(imgdatas)
    }
  }, [page, pages])

  useEffect(() => {
    i18n.changeLanguage(lang)
  }, [lang])

  // ref all page print
  // React.useImperativeHandle(ref, () => ({
  //   context: () => {
  //     pages.forEach((page, index) => {
  //       if (index > 0) {
  //         canvasRef.current.style.display = 'block'
  //         canvasRef.current.appendChild(page)
  //       }
  //     })
  //     return canvasRef.current
  //   }
  // }))

  const handleWheelImage = (event) => {
    console.log(event)
    event.stopPropagation();
    event.nativeEvent.stopImmediatePropagation()
    // event.preventDefault();

    var canvas = canvasXRef.current



    const ctx = canvas.getContext('2d');
    // 向上为负，向下为正
    const bigger = event.deltaY > 0 ? -1 : 1;
    // 放大比例
    const enlargeRate = 1.2;
    // 缩小比例
    const shrinkRate = 0.8;

    const rate = bigger > 0 ? enlargeRate : shrinkRate;

    const width = canvasWidth * rate;
    const height = canvasHeight * rate;
    console.log(width)
    console.log(height)
    setCanvasWidth(width)
    setCanvasHeight(height)

    // var ifds = UTIF.decode(buffer)
    // var ifd = ifds[page]
    // UTIF.decodeImage(buffer, ifd)
    // var rgba = UTIF.toRGBA8(ifd)

    ctx.clearRect(0, 0, canvas.width, canvas.height)
    // ctx.clearRect(0, 0, ifd.width, ifd.height);

    // ctx.drawImage(imgElement, offsetDis.left, offsetDis.top, width, height);
    // var img = ctx.createImageData(ifd.width, ifd.height)
    // img.data.set(rgba)
    // const { height: initHeight, naturalHeight, naturalWidth } = img;

    resizeImageData(imgdatas[page], width, height).then(data => {

      // ctx.putImageData(data, 0, 0)
      ctx.putImageData(data, offsetDis.left, offsetDis.top)

      console.log(`set width:${width}`)
      // canvasWidth = width
      // canvasHeight = height
    })

    return false

  }

  const handleMouseDown = (event) => {
    event.stopPropagation();
    event.preventDefault(); // 阻止浏览器默认行为，拖动会打开图片
    const { clientX, clientY } = event;
    // 相对于canvas坐标
    const canvas = event.target;
    const pos = windowToCanvas(canvas, clientX, clientY);
    canvas.style.cursor = 'move';
    setMouseDowmFlag(true); // 控制只有在鼠标按下后才会执行mousemove

    setMouseDowmPos({
      x: pos.x,
      y: pos.y,
    });


  }

  const handleMouseMove = (event) => {

    event.stopPropagation();
    event.preventDefault();

    // var ifds = UTIF.decode(buffer)
    // var ifd = ifds[page]
    // UTIF.decodeImage(buffer, ifd)
    // var rgba = UTIF.toRGBA8(ifd)

    if (!mouseDowmFlag) return;
    const { clientX, clientY } = event;
    const canvas = event.target
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

    ctx.clearRect(0, 0, canvas.width, canvas.height);

    // ctx.drawImage(imgElement, offsetDis.left, offsetDis.top, width, height);
    // var img = ctx.createImageData(ifd.width, ifd.height)
    // img.data.set(rgba)
    // const { height: initHeight, naturalHeight, naturalWidth } = img;

    resizeImageData(imgdatas[page], canvasWidth, canvasHeight).then(data => {

      ctx.putImageData(data, offsetX, offsetY)


    })


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


  }

  const handleMouseUp = (event) => {
    event.stopPropagation();
    event.preventDefault();
    const canvas = event.target;
    canvas.style.cursor = 'default';
    setMouseDowmFlag(false);
  }


  return (
    <div className={styles.container} id='tiff-container'   {...rest}>
      {printable && (
        <button
          id='btn-print'
          onClick={handlePrintClick}
          ref={btnPrintRef}
          className={styles.btnPrint}
          type='button'
        >
          <svg
            xmlns='http://www.w3.org/2000/svg'
            fill='none'
            viewBox='0 0 24 24'
            strokeWidth={1.5}
            stroke='currentColor'
            className='w-6 h-6'
          >
            <path
              strokeLinecap='round'
              strokeLinejoin='round'
              d='M6.72 13.829c-.24.03-.48.062-.72.096m.72-.096a42.415 42.415 0 0110.56 0m-10.56 0L6.34 18m10.94-4.171c.24.03.48.062.72.096m-.72-.096L17.66 18m0 0l.229 2.523a1.125 1.125 0 01-1.12 1.227H7.231c-.662 0-1.18-.568-1.12-1.227L6.34 18m11.318 0h1.091A2.25 2.25 0 0021 15.75V9.456c0-1.081-.768-2.015-1.837-2.175a48.055 48.055 0 00-1.913-.247M6.34 18H5.25A2.25 2.25 0 013 15.75V9.456c0-1.081.768-2.015 1.837-2.175a48.041 48.041 0 011.913-.247m10.5 0a48.536 48.536 0 00-10.5 0m10.5 0V3.375c0-.621-.504-1.125-1.125-1.125h-8.25c-.621 0-1.125.504-1.125 1.125v3.659M18 10.5h.008v.008H18V10.5zm-3 0h.008v.008H15V10.5z'
            />
          </svg>
        </button>
      )}
      <div className={styles.arrow}>
        <div
          id='tiff-inner-container'
          className={styles.inner}
          ref={canvasRef}
        >
          <canvas
            ref={canvasXRef}

            onWheel={handleWheelImage}
            onMouseDown={handleMouseDown}
            onMouseMove={handleMouseMove}
            onMouseUp={handleMouseUp}
          ></canvas>

        </div>

        {paginate === 'ltr' && pageCount > 1 && (
          <div className={styles.absolute} id='absolute' ref={paginateLTRRef}>
            <button
              style={{ backgroundColor: buttonColor }}
              disabled={page === 0}
              onClick={handlePreviousClick}
              className={styles.button}
              type='button'
            >
              {t('<')}
            </button>{' '}
            <button
              style={{ backgroundColor: buttonColor }}
              disabled={page === pageCount - 1}
              onClick={handleNextClick}
              className={styles.button}
              type='button'
            >
              {t('>')}
            </button>
          </div>
        )}
      </div>

      {paginate === 'bottom' && pageCount > 1 && (
        <div id='footer' ref={paginateBottomRef}>
          <button
            style={{ backgroundColor: buttonColor }}
            disabled={page === 0}
            onClick={handlePreviousClick}
            className={styles.button}
            type='button'
          >
            {t('Previous')}
          </button>
          <span className={styles.span}>
            {t('Page of total', { page: page + 1, total: pageCount })}
          </span>
          <button
            style={{ backgroundColor: buttonColor }}
            disabled={page === pageCount}
            onClick={handleNextClick}
            className={styles.button}
            type='button'
          >
            {t('Next')}
          </button>
        </div>
      )}
    </div>
  )
}

TIFFViewer.propTypes = {
  tiff: PropTypes.string.isRequired,
  tiffbase64: PropTypes.string.isRequired,
  lang: PropTypes.string,
  paginate: PropTypes.string,
  buttonColor: PropTypes.string,
  printable: PropTypes.bool
}
