import React, { useRef, useState,useEffect } from "react";
import Card from "../../ui/Card";
import classes from "./UploadProductsForm.module.css";
import { InsertProduct } from "../../../services/FetchProducts";
import { UploadPhoto } from "../../../services/FetchPhotos";

export default function UploadProductsForm() {
  const titleInputRef = useRef();
  const dateInputRef = useRef();
  const descriptionInputRef = useRef();
  const photosInputRef = useRef();

  const [isSelected, setIsSelected] = useState(false);
  const [imgFiles, setImgFiles] = useState([]);
  const [urlFiles, setUrlFiles] = useState([]);

  const toDay = new Date().toISOString().substring(0, 10);

  useEffect(()=>console.log(imgFiles),[imgFiles]);

  function uploadPhotos(photoID, photos) {
    photos.map((photo) => {
      UploadPhoto(photoID, photo);
    });
  }

  function imgFileshandler(event) {
   
    if (event.target.files.length !== 0) {
      setUrlFiles([...urlFiles, URL.createObjectURL(event.target.files[0])]);
      setImgFiles([...imgFiles, event.target.files[0]]);
      setIsSelected(true);
    }
  }

 function removeFileHandler(file, index) {
    setUrlFiles((prevFiles) => {
      return prevFiles.filter((preFile) => preFile !== file);
    });

    const temp = [];
    imgFiles.map((imgFile, imgIndex) => {
      if (imgIndex !== index) {
        temp.push(imgFile);
      }
    });

    setImgFiles(temp);
   
  }



  async function submitHandler(event) {
    event.preventDefault();
    const enteredTitle = titleInputRef.current.value;
    const enteredDate = dateInputRef.current.value;
    const enteredDescription = descriptionInputRef.current.value;

    if (imgFiles.length > 0) {
      const photoID = await InsertProduct(
        enteredTitle,
        enteredDate,
        enteredDescription
      );
      uploadPhotos(photoID, imgFiles);
      event.target.reset();
      setUrlFiles([]);
      setImgFiles([]);
      setIsSelected(false);
      alert("Upload Success.");
    } else {
      alert("Please choose Photo.");
    }
  }

  return (
    <Card>
      <form className={classes.form} onSubmit={submitHandler}>
        <div className={classes.inputarea}>
          <div className={classes.inputitem}>
            <label htmlFor="title">Title</label>
            <input
              type="text"
              id="title"
              className="title"
              required
              ref={titleInputRef}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="description">Description</label>
            <textarea
              id="description"
              className="Description"
              required
              rows="5"
              ref={descriptionInputRef}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="date">Date</label>
            <input
              type="date"
              id="date"
              className="date"
              required
              ref={dateInputRef}
              defaultValue={toDay}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="photos">Photos</label>
            <input
              type="file"
              id="photos"
              className="photos"
              required
              ref={photosInputRef}
              onChange={imgFileshandler}
              accept="image/png, image/gif, image/jpeg"
           />
            {isSelected ? (
              <div className={classes.photolists}>
                <h2>Preview</h2>
                {urlFiles.map((file, index) => {
                  return (
                    <>
                      <span key={index}>
                        <img
                          src={file}
                          height="200"
                          width="200"
                          alt="med1"
                          key={index}
                        />

                        <input
                          type="button"
                          value="Delete"
                          onClick={() => removeFileHandler(file, index)}
                        />
                      </span>
                    </>
                  );
                })}
              </div>
            ) : (
              <p>Select a file to show details</p>
            )}
          </div>

          <div className={classes.actions}>
            <button type="submit">Upload</button>
          </div>
        </div>
      </form>
    </Card>
  );
}
