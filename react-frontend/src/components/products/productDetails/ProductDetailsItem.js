import React from 'react'
import { useState,useEffect } from 'react';
import { GetPhoto } from '../../../services/FetchPhotos';
import { LazyLoadImage } from "react-lazy-load-image-component";

function ProductDetailsItem(props) {
    const [photo,setPhoto]=useState([]);

    useEffect(()=>{showPhoto(props.id,props.image)},[]);

    function showPhoto(productId, image) {
          const response = GetPhoto(productId,image);
          response.then((value) => {
            setPhoto(value);
          });
      }

  return (
    <LazyLoadImage src={photo} alt={props.title} key={props.index}  height="8em" width="9em" effect="blur"/>
  )
}

export default ProductDetailsItem