import React from "react";
import Card from "../../ui/Card";
import classes from "./ProductItem.module.css";
import { Link } from "react-router-dom";
import { GetPhoto } from "../../../services/FetchPhotos";
import { GetPhotos } from "../../../services/FetchPhotos";
import { useEffect } from "react";
import { useState } from "react";
import { FormatDate, UppcaseFirstLetter } from "../../../lib/Format";
import { LazyLoadImage } from "react-lazy-load-image-component";

function ProductItem(props) {
  const [image, setImage] = useState("");
  const [photos, setPhotos] = useState([]);


  useEffect(() => getAllPhotos(props.id), []);

  function getAllPhotos(productId) {
    const response = GetPhotos(productId);
    response.then((value) => {
      if (value.length !== undefined) {
        setPhotos(value);
        getFirstPhoto(productId, value[0]);
      }
    });
  }

  function getFirstPhoto(productId, firstPhotoName) {
    const response = GetPhoto(productId, firstPhotoName);
    response.then((value) => {
      setImage(value);
    });
  }



  return (
    <div className={classes.item} id={props.id}>
      <Card>
        <div className={classes.image}>
          <div className={classes.back}>
            <LazyLoadImage
              src={image}
              alt={props.id}
              effect="blur"
              height="15em"
              width="16em"
            />

          </div>

          <div className={classes.front}>
            <div className={classes.favour}>
              <i class="fa fa-heart" aria-hidden="true"></i>
            </div>
            <div className={classes.search}>
              <Link
                to="/productDetails"
                state={{
                  id: props.id,
                  title: props.title,
                  images: photos,
                  date: props.date,
                  description: props.description,
                }}
              >
                <div className={classes.searchlogo}>
                  <i class="fa fa-search-plus" aria-hidden="true"></i>
                </div>
              </Link>
              view more
            </div>
          </div>
        </div>

        <div className={classes.content}>
          <div>{FormatDate(props.date)}</div>
          <div>{UppcaseFirstLetter(props.title)}</div>
        </div>
      </Card>
    </div>
  );
}

export default ProductItem;
