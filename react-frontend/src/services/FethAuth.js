import axios from "axios";
import "abortcontroller-polyfill/dist/polyfill-patch-fetch";



  
export async function register({ userName, password, firstName, lastName, emailAddress }) {
    const current = new Date();
    const year = current.getFullYear();
    let month = current.getMonth() + 1;
    month = month.toString().padStart(2, '0');
    let day = current.getDate();
    day = day.toString().padStart(2, '0');
    const createDate = `${year}-${month}-${day}`;

    try {
        const response= await axios.post("/Auth/register", {
          userName,
          password,
          emailAddress,
          firstName,
          lastName,
          createDate,
        })
        console.log(response.data);
       
        
      } catch (error) {
        return error;
      
      }
  }

export async function login({userName,password}){
   try 
   {
     const response= await axios.post("/Auth/login",{userName,password})
     console.log(response.data);
     return response;
   }catch (error) {
    return error;
  
  }
}
 


