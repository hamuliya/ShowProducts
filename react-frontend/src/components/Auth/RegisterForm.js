import React from "react";
import Card from "../ui/Card";
import classes from "../Form.module.css";
import { useRef, useEffect, useState } from "react";
import { register } from "../../services/FethAuth";
import { toast } from "react-toastify";

function RegisterForm() {
  const userNameInputRef = useRef();
  const passwordInputRef = useRef();
  const passwordconfirmInputRef = useRef();
  const firstNameInputRef = useRef();
  const lastNameInputRef = useRef();
  const emailAddressInputRef = useRef();

  const [error, setError] = useState(null);

  useEffect(() => {
    if (error) {
      alert(error);
    }
    return () => {
      setError(null);
    };
  }, [error]);

  function checkPassword(password) {
    let output = true;
    if (password.length < 8 || password.length > 50) {
      setError("Password must be at least 8 characters long");
      output = false;
    } else if (!/[A-Z]/.test(password)) {
      setError("Password must include at least one capital letter");
      output = false;
    } else if (!/[!@#$%^&*(),.?":{}|<>]/.test(password)) {
      setError("Password must include at least one special character");
      output = false;
    }
    return output;
  }

  async function handleSubmit(e) {
   
    e.preventDefault();

    const enteredUserName = userNameInputRef.current.value;
    const enteredPassword = passwordInputRef.current.value;
    const enteredPasswordConfirm = passwordconfirmInputRef.current.value;
    const enteredFirstName = firstNameInputRef.current.value;
    const enteredLastName = lastNameInputRef.current.value;
    const enteredEmailAddress = emailAddressInputRef.current.value;
    
    if (enteredPassword !== enteredPasswordConfirm) {
      setError("Password is different from password confirmation.");
    } else if (enteredUserName.length < 3 || enteredUserName.length > 50) {
      setError("Username length should be between 4-50.");
    } else if (enteredFirstName > 50) {
      setError("First Name length should be less than 50.");
    } else if (enteredLastName > 50) {
      setError("Last Name length should be less than 50.");
    } else if (enteredEmailAddress > 100) {
      setError("Email Address length should be less than 100.");
    }else {  
      const passwordCheck = checkPassword(enteredPassword);
      if (!passwordCheck) {
        throw new Error("Password does not meet the requirements.");
      }
    }


    if (!error) {
      try {
      
        let error = await register({
          userName: enteredUserName,
          password: enteredPassword,
          firstName: enteredFirstName,
          lastName: enteredLastName,
          emailAddress: enteredEmailAddress,
        });
        if (error) {
          setError(error.response.data);
          console.error(error);
          toast.error("Error: " + error.message, {
            position: toast.POSITION.BOTTOM_CENTER,
          });
        } else {
          toast.success("Upload Success.", {
            position: toast.POSITION.BOTTOM_CENTER,
          });
          e.target.reset();
        }
      } catch (error) {
        console.log(error);
      }
    }
  }
  return (
    <Card>
      <form className={classes.form} onSubmit={handleSubmit}>
        <div className={classes.inputarea}>
          <div className={classes.inputitem}>
            <label htmlFor="username">User Name:</label>
            <input
              type="text"
              id="username"
              className="username"
              required
              ref={userNameInputRef}
            />
          </div>
          <div className={classes.inputitem}>
            <label htmlFor="password">Password:</label>
            <input
              type="password"
              id="password"
              className="password"
              required
              ref={passwordInputRef}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="confirm">Confirm:</label>
            <input
              type="password"
              id="passwordconfirm"
              required
              ref={passwordconfirmInputRef}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="firstname">First Name:</label>
            <input
              type="text"
              id="firstname"
              required
              ref={firstNameInputRef}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="lastname">Last Name:</label>
            <input type="text" id="lastname" required ref={lastNameInputRef} />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="emailaddress">Email Address:</label>
            <input
              type="email"
              id="emailaddress"
              required
              ref={emailAddressInputRef}
            />
          </div>

          <div className={classes.actions}>
            {/* {error && <div style={{ color: 'red' }}>{error}</div>} */}
            <button type="submit">Submit</button>
          </div>
        </div>
      </form>
    </Card>
  );
}

export default RegisterForm;
