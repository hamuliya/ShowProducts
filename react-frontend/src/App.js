import React from "react";
import {  Routes, Route } from "react-router-dom";
import Home from "./pages/Home";
import UploadProduct from "./pages/UploadProduct";

import ProductDetails from "./pages/ProductDetails";
import SignIn from "./pages/SignIn";
import Register from "./pages/Register";
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
            <Route path="/signIn" element={<SignIn />} />
            <Route path="/register" element={<Register />} />
          </Routes>
        </Layout>
      </div>
    </>
  );
}

export default App;
