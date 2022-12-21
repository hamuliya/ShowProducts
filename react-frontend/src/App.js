import React from "react";
import {  Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import UploadProduct from "./pages/UploadProduct";

import ProductDetails from "./pages/ProductDetails";
import Layout from "./layout/Layout";






function App () {
  return (
    <>
      <div>
        <Layout>
          <Routes>
            <Route path="/" element={<Home />} />
            <Route path="/uploadProduct" element={<UploadProduct />} />
            <Route path="/productDetails" element={<ProductDetails />} />
          </Routes>
        </Layout>
      </div>
    </>
  );
}

export default App;
