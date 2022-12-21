import classes from './Header.module.css'
import React from 'react'
import { Link } from 'react-router-dom'

function Header() {
  return (
    <div className={classes.header}>
      <Link to="/" className={classes.logo}>
        Elle
      </Link>
    </div>
  )
}

export default Header