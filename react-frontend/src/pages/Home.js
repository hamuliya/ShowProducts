import React from "react";
import ProductList from "../components/products/products/ProductList";
import { GetProducts } from "../services/FetchProducts";
import { useEffect, useState } from "react";

function Home() { 
  const [products,setProducts]=useState([]);
  const [loading,setLoading]=useState(false);
  useEffect(() => {
    const response = GetProducts(setLoading);
     response.then((value)=>{
       setProducts(value.data);
     })
  }, []);
  return (
    <>
      <ProductList products={products}/>
    </>
  );
}

export default Home;
