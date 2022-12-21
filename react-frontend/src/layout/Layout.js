import React from "react";
import PageContainer from "./PageContainer";
import Navbar from "./Navbar";
import Header from "./Header";
import classes from "./Layout.module.css";
import Footer from "./Footer";

function Layout(props) {
  return (
    <div className={classes.layout}>
      <Header className={classes.header}/>
      <Navbar className={classes.navbar}/>
      <PageContainer className={classes.pagecontainer}>{props.children}</PageContainer>
      <Footer className={classes.footer}/>
    </div>
  );
}

export default Layout;
