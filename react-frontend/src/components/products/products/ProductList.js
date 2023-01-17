import React from "react";
import ProductItem from "./ProductItem";
import classes from "./ProductList.module.css";

function ProductList(props) {
  
  return (
    <>
      <div className={classes.itemlist}>
        {props.products.map((product) => (
          
          <ProductItem
            key={product.productId}
            id={product.productId}
            title={product.title}
            date={product.uploadDate}
            description={product.detail}
          />
        ))}
      </div>
    </>
  );
}

export default ProductList;
