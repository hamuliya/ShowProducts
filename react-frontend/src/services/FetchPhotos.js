import axios from "axios";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";

export async function uploadPhoto(photoID, photo) {
  const bodyFormData = new FormData();
  bodyFormData.append("PhotoID", photoID);
  bodyFormData.append("Photo", photo);

  try {
    const response = await axios.post("/Photo/UploadPhoto", bodyFormData, {
      headers: { "Content-Type": "multipart/form-data" },
      timeout: 10000, // 10 seconds
    });

    if (response.status >= 200 && response.status < 300) {
      return response.data;
    } else {
      throw new Error(`Request failed with status code: ${response.status}`);
    }
  } catch (error) {
    console.error(error);
    throw error;
  }
}

export async function getPhotos(photoId, setLoading) {
  setLoading(true);
  const controller = new AbortController();
  const signal = controller.signal;
  let data = null;
  let error = null;
  try {
    const response = await axios.get(`/Photo/GetPhotos/${photoId}`, { signal });
    if (response.status === 200) {
      data = response.data;
    } else {
      error = {
        message: "There was an error processing your request",
        status: response.status,
      };
    }
  } catch (e) {
    if (e.name === "AbortError") {
      // request was cancelled
    } else {
      error = {
        message: e.message,
        status: e.response?.status || 500,
      };
    }
  }
  setLoading(false);
  return { data, error };
}



export async function getPhoto(photoId, photoName, setLoading) {
  setLoading(true);
  const controller = new AbortController();
  const signal = controller.signal;
  let data = null;
  let error = null;
  try {
    const response = await axios.get(`/Photo/GetPhoto/${photoId}/${photoName}`,{ signal });

   
    if (response.status === 200) {
      data = response.request.responseURL;
    } else {
      error = {
        message: "There was an error processing your request",
        status: response.status,
      };
    }
  } catch (e) {
    if (e.name === "AbortError") {
      // request was cancelled
    } else {
      error = {
        message: e.message,
        status: e.response?.status || 500,
      };
    }
  }
  setLoading(false);
  return { data, error };
}
