import React from 'react'
import ProductDetailsList from '../components/products/productDetails/ProductDetailsList';
import { useLocation } from 'react-router-dom';



function ProductDetails() {
  const location = useLocation();
  const { id,title,images,date,description } = location.state;

  
    return (
       <ProductDetailsList id={id} title={title} images={images} date={date} description={description}/>
  )
}

export default ProductDetails