import PropTypes from "prop-types";
import React, {
  createContext,
  useContext,
  useMemo,
  useState,
  useEffect,
  ReactNode
} from "react";
import UTIF from "utif";

// type ContextProps = {
//     children: ReactNode;
    
// }
const Context = createContext(
  // {pageNumber:0,
  //     setPageNumber:(s:number)=>{},
  //     pages: [],
  //     setPages:(s:number)=>{}
  //   }
  );

// While I probably don't need context for an app this small I like to set up ahead of time just in case and to avoid prop drilling.

export const TiffContext = ({ children,tiffurlx}) => {
  // The pages array is set on mount and used in SelectPage.js. This will likely have to change
  const [pages, setPages] = useState([]);

  // pageNumber is set in SelectPage.js and used in TiffDisplay.js
  const [pageNumber, setPageNumber] = useState(0);

  console.log(tiffurlx)
  const [tiffurl,setTiffUrl]=useState(tiffurlx.tiffurl);
  // UTIF to just return pages array
  useEffect(() => {
    const xhr = new XMLHttpRequest();
    xhr.open("GET", tiffurl );
    xhr.responseType = "arraybuffer";
    xhr.onload = (e) => {
      const ifds = UTIF.decode(e.target.response);

      const pages  = [];

      ifds.forEach((page, i) => pages.push({ id: i, number: i + 1 }));
      setPages(pages);
    };

    xhr.send();
  }, []);

  const value = useMemo(
    () => ({
      pageNumber,
      setPageNumber,
      pages,
      setPages,
      tiffurl,
      setTiffUrl
    }),
    [pageNumber, pages]
  );

  return <Context.Provider value={value}>{children}</Context.Provider>;
};

export const useTiffContext = () => useContext(Context);
TiffContext.propTypes = {
  children: PropTypes.node.isRequired,
};
