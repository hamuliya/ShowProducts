import React from "react";
import classes from "./Modal.module.css";

function Modal({ title, message, setOpenModal }) {
  return (
    <div className={classes.modalBackground}>
      <div className={classes.modalContainer}>
        <div className={classes.titleCloseBtn}>
          <button
            onClick={() => {
              setOpenModal(false);
            }}
          >
            X
          </button>
        </div>
        <div className={classes.title}>
          <h1>{title}</h1>
        </div>
        <div className={classes.body}>
          <p>{message}</p>
        </div>
        <div className={classes.footer}>
          <button
            onClick={() => {
              setOpenModal(false);
            }}
            className={classes.cancelBtn}
          >
            Close
          </button>
          {/* <button>Continue</button> */}
        </div>
      </div>
    </div>
  );
}

export default Modal;
