import axios from "axios";


export async function UploadPhoto(photoID, photo) {
    var bodyFormData = new FormData();
    bodyFormData.append("PhotoID", photoID);
    bodyFormData.append("Photo", photo);
    await axios({
      method: "post",
      url: "/Photo/UploadPhoto",
      data: bodyFormData,
      headers: { "Content-Type": "multipart/form-data" },
    })
      .then((response) => {
        //handle success
        console.log(response);
        return response;
      })
      .catch((error) => {
        //handle error
        console.log(error);
        return error;
      });
  }
  
  
  
  export async function GetPhotos(photoId){
    const response= await axios.get(`/Photo/GetPhotos/${photoId}`);
    return response.data.value;
   };

   export async function GetPhoto(photoId,photoName){
    const response= await axios.get(`/Photo/GetPhoto/${photoId}/${photoName}`);
    return response.request.responseURL;
    //return response.data;
   };
