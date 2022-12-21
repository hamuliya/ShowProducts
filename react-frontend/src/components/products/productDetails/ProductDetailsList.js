import React from "react";

import classes from "./ProductDetailsList.module.css";
import { GetPhoto } from "../../../services/FetchPhotos";
import { useState, useEffect } from "react";
import ProductDetailsItem from "./ProductDetailsItem";
import { LazyLoadImage } from "react-lazy-load-image-component";

import { FormatDate } from "../../../lib/Format";
import { UppcaseFirstLetter } from "../../../lib/Format";


function ProductDetailsList(props) {
  const [currentPhoto, setCurrentPhoto] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    getCurrentPhoto(props.id, props.images[currentIndex]);
  }, [currentIndex]);

  function getCurrentPhoto(productId, image) {
    const response = GetPhoto(productId, image);
    response.then((value) => {
      setCurrentPhoto(value);
    });
  }

  function goToPrevious() {
    const isFirstSlide = currentIndex === 0;
    const newIndex = isFirstSlide ? props.images.length - 1 : currentIndex - 1;
    setCurrentIndex(newIndex);
  }
  function goToNext() {
    const isLastSlide = currentIndex === props.images.length - 1;
    const newIndex = isLastSlide ? 0 : currentIndex + 1;
    setCurrentIndex(newIndex);
  }
  function goToSlide(slideIndex) {
    setCurrentIndex(slideIndex);
  }

  function showDialogHandler() {
    setIsOpen(!isOpen);
  }
  return (
    <>
      <div className={classes.grid}>
        <div className={classes.largeImage}>
          <figure>
            <LazyLoadImage
              src={currentPhoto}
              alt={props.title}
              effect="blur"
              onClick={showDialogHandler}
              className={classes.zoomIn}
            />

            {isOpen && (
              <dialog
                className={classes.dialog}
                open>
                <LazyLoadImage
                  src={currentPhoto}
                  alt={props.title}
                  effect="blur"
                  onClick={showDialogHandler}
                  className={classes.zoomOut}
                />
              </dialog>
            )}

            <figcaption>{UppcaseFirstLetter(props.title)}</figcaption>
          </figure>
        </div>

        <div className={classes.leftarrow} >
          <div onClick={goToPrevious}>
          ❰
          </div>
        </div>

        <div className={classes.smallAreaFrame}>
          <div
            className={classes.smallArea}
            style={{
              transform:` translateX(calc(-${currentIndex} * 9rem))`
            }}



            // style={{
            //   transform: `translateX(-${
            //     currentIndex * (100 / props.images.length)
            //   }%)`,
            // }}

            

          >
            {props.images.map((image, index) => (
              <div key={index} onClick={() => goToSlide(index)}>
                <ProductDetailsItem image={image} index={index} id={props.id} />
              </div>
            ))}
          </div>
        </div>

        <div className={classes.rightarrow} >
          <div onClick={goToNext}> ❱</div>
        </div>
        <div className={classes.content}>
          <div className={classes.date}>{FormatDate(props.date)}</div>
          <div className={classes.description}>
            {UppcaseFirstLetter(props.description)}
          </div>
        </div>
      </div>
    </>
  );
}

export default ProductDetailsList;
