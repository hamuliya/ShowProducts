import React, { useRef, useState,useEffect } from "react";
import Card from "../../ui/Card";
import classes from "../../Form.module.css";
import { insertProduct } from "../../../services/FetchProducts";
import { uploadPhoto } from "../../../services/FetchPhotos";
import { toast } from 'react-toastify';
import Modal from "../../ui/Modal";




export default function UploadProductsForm() {
  const titleInputRef = useRef();
  const dateInputRef = useRef();
  const descriptionInputRef = useRef();
  const photosInputRef = useRef();

  const [isSelected, setIsSelected] = useState(false);
  const [imgFiles, setImgFiles] = useState([]);
  const [urlFiles, setUrlFiles] = useState([]);
 

  const [modalOpen, setModalOpen] = useState(false);
  const [title,setTitle]=useState(null);
  const [errorMessage,setErrorMessage]=useState(null);
 

  const toDay = new Date().toISOString().substring(0, 10);

  const MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB
  const ALLOWED_FILE_TYPES = ['image/jpg', 'image/jpeg', 'image/png', 'image/gif'];
  const FILE_TYPES="image/jpg, image/jpeg, image/png, image/gif";


  useEffect(()=>console.log(imgFiles),[imgFiles]);

  function uploadPhotos(photoID, photos) {
    photos.map((photo) => {
      uploadPhoto(photoID, photo);
    });
  }

  function imgFileshandler(event) {
    const selectedFile = event.target.files[0];
    if (selectedFile)
    {
      if (selectedFile.size > MAX_FILE_SIZE) {
      
        setErrorMessage(`File size must be less than ${MAX_FILE_SIZE / 1024 / 1024}MB`);
        setModalOpen(true);
        return;
    }

      if (!ALLOWED_FILE_TYPES.includes(selectedFile.type)) {
      setErrorMessage("Invalid file type. Only jpg, jpeg, png, gif are allowed.");
      setModalOpen(true);
      return;
    }
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
      await insertProduct({title:enteredTitle,uploadDate:enteredDate,detail:enteredDescription})
      .then((photoID) => {
        uploadPhotos(photoID, imgFiles);
        event.target.reset();
        setUrlFiles([]);
        setImgFiles([]);
        setIsSelected(false);
        toast.success('Upload Success.', {
          position: toast.POSITION.BOTTOM_CENTER,
      });

      })
      .catch((error) => {
        toast.error("Error: " + error.message,{position:toast.POSITION.BOTTOM_CENTER});
      });
  }
  else
  {
    setErrorMessage("Please choose photos");
    setModalOpen(true);
    return;
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
              accept={FILE_TYPES}

              
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
        {modalOpen && <Modal title={title} message={errorMessage} setOpenModal={setModalOpen} />}
      </form>
   
    </Card>
  );
}
