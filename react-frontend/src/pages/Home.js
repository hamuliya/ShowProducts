import React from "react";
import ProductList from "../components/products/products/ProductList";
import { GetProducts } from "../services/FetchProducts";
import { useEffect, useState } from "react";

function Home() { 
  const [products,setProducts]=useState([]);
  useEffect(() => {
    const response = GetProducts();
     response.then((value)=>{
       setProducts(value.data.value);
     })
  }, []);
  return (
    <>
      <ProductList products={products}/>
    </>
  );
}

export default Home;
