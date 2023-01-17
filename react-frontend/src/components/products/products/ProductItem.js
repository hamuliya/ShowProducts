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
  const [photos, setPhotos] = useState(null);
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    getAllPhotos(props.id);
    return () => {}
  }, [props.id]);

  async function getAllPhotos(productId) {
    const {data,error} = await GetPhotos(productId,setLoading);
    if (error) {setError(error);}
    else{
      setPhotos(data);
      getFirstPhoto(productId, data[0]);
    }
  }


  async function getFirstPhoto(productId, firstPhotoName) {
    setLoading(false);
    const {data,error} =await GetPhoto(productId, firstPhotoName,setLoading);
    if (error) {setError(error);}
    else{
      setImage(data);
    } 
  }

  return (
    <>
    {loading && <div>Loading...</div>}
    {error && <div>{error.message}</div>}
    {photos &&
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
    </div>}
    </>
  );
 
}

export default ProductItem;
