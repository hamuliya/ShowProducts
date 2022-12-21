import React from "react";
import classes from "./PageContainer.module.css";

function PageContainer(props) {
  return( <main className={classes.pageContainer}>{props.children}</main>)
 
}

export default PageContainer;
