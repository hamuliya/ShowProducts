import axios from "axios";
import 'abortcontroller-polyfill/dist/polyfill-patch-fetch';




export async function getProducts(setLoading) {
  setLoading(true);
  const response =await axios.get("/Product");
  setLoading(false);
  return  response;  
}


export async function getProduct(id, setLoading) {
  setLoading(true);
  const controller = new AbortController();
  const signal = controller.signal;
  let product = null;
  let error = null;
  try {
    const response = await axios.get(`/Product/${id}`, { timeout: 10000 ,signal });
    if (response.status === 200) {
        product = response.data;
    } else {
        error = {
            message: 'There was an error processing your request',
            status: response.status,
        }
    }
  } catch (e) {
    if (e.name === 'AbortError') {
      // request was cancelled
    } else {
      error = {
        message: e.message,
        status: e.response?.status || 500
      }
    }
  }
  setLoading(false);
  return {product, error}
}








export async function deleteProduct(id) {
  await axios
    .delete(`/Product/${id}`)
    .then((res) => {
      console.log(res.data);
      alert("Product deleted!");
    })
    .catch((error) => {
      console.error("Error:", error);
    });
}




// export async function UpdateProduct({id,title,updateDate,detail}) {
//   await axios
//     .put(`/Product/${id}`, {
//       title: title,
//       updateDate:updateDate,
//       detail:detail,
//     })
//     .then((res) => {
//       console.log(res.data);
//       alert("Product updated!");
//     })
//     .then((result) => console.log("Success:", result))
//     .catch((error) => console.log("Error", error));
// }

// export async function InsertProduct(title, uploadDate, detail) {
//   const response = await axios.post("Product", {
//     title: title,
//     uploadDate: uploadDate,
//     detail: detail,
//   });


//   return response.data;
// }

export async function insertProduct({ title, uploadDate, detail }) {
  try {
    const { data } = await axios.post(`Product`, { title, uploadDate, detail });
    return data;
  } catch (error) {
    console.log(error);
    throw error;
  }
}




//const api = axios.create({
//    baseURL: 'http://localhost:5000/api',
//    headers: {
//        'Authorization': `Bearer ${localStorage.getItem('jwt')}`
//    }
//});

//api.get('/protected-route')
//    .then(response => {
//        // do something with the response
//    })
//    .catch(error => {
//        // handle the error
//    });