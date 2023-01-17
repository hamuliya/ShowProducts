import React, { useState } from "react";
import { Link } from "react-router-dom";
import classes from "./Navbar.module.css";


function Navbar() {
  return (
    <nav className={classes.navbar}>
      <ul className={classes.menu}>
        <li className={classes.item}>
          <Link to="/" className={classes.link}>
            Home
          </Link>
        </li>
        <li className="item">
          <Link to="/uploadProduct" className={classes.link}>
            Upload Product
          </Link>
        </li>
      </ul>
      <Link to="/signIn" className={classes.btnlink}>
       <button className={classes.btn}>SIGN IN</button> 
       </Link>
    </nav>
  );
}

export default Navbar;
