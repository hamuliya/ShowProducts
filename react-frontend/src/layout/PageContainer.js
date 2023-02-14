import React from "react";
import classes from "./PageContainer.module.css";
import { ToastContainer } from "react-toastify";
import 'react-toastify/dist/ReactToastify.css';


function PageContainer(props) {
 return( 

 
  <main className={classes.pageContainer}>
    <ToastContainer autoClose={1000} />
    {props.children}
   
  </main>)
 
}

export default PageContainer;
