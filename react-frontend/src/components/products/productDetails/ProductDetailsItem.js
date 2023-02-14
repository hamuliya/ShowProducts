import React from 'react'
import { useState,useEffect } from 'react';
import { getPhoto } from '../../../services/FetchPhotos';
import { LazyLoadImage } from "react-lazy-load-image-component";

function ProductDetailsItem(props) {
    const [photo,setPhoto]=useState([]);
    const [loading,setLoading]=useState(false);
    const [error,setError]=useState(false);

    useEffect(()=>{showPhoto(props.id,props.image)},[]);

    async function showPhoto(productId, image) {
          const {data,error} = await getPhoto(productId,image,setLoading);
          if (error){setError(error)} 
          else{setPhoto(data); }
      }

  return (

    <>
    {loading && <div>Loading...</div>}
    {error && <div>error.message </div>}
    {photo &&
    <LazyLoadImage src={photo} alt={props.title} key={props.index}  height="8em" width="9em" effect="blur"/>
    }
    </>
  )
}

export default ProductDetailsItem