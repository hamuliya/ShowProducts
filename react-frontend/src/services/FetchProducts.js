import axios from "axios";




export async function GetProducts() {
  const response =await axios.get("/Product");
  return  response;  
}

export async function GetProduct(id) {
  await axios
    .get(`/Product/${id}`)
    .then((res) => {
      const Products = res.data;
      console.log(res.data);
      return Products;
    })
    .catch((error) => {
      console.error("Error:", error);
    });
}






export async function DeleteProduct(id) {
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




export async function UpdateProduct({id,title,updateDate,detail}) {
  await axios
    .put(`/Product/${id}`, {
      title: title,
      updateDate:updateDate,
      detail:detail,
    })
    .then((res) => {
      console.log(res.data);
      alert("Product updated!");
    })
    .then((result) => console.log("Success:", result))
    .catch((error) => console.log("Error", error));
}

export async function InsertProduct(title, uploadDate, detail) {
  const response = await axios.post("Product", {
    title: title,
    uploadDate: uploadDate,
    detail: detail,
  });


  return response.data.value;
}

