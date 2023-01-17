import React from "react";
import Card from "../ui/Card";
import classes from "../Form.module.css";
import { useRef, useState } from "react";

function RegisterForm() {
  const userNameInputRef = useRef();
  const passwordInputRef = useRef();
  const passwordconfirmInputRef = useRef();
  const firstNameInputRef=useRef();
  const lastNameInputRef=useRef();
  const emailAddressInputRef=useRef();

  const [error, setError] = useState(null);

  async function handleSubmit(e) {
    e.preventDefault();
    //   try {
    //     const enteredUserName = userNameInputRef.current.value;
    //     const enteredPassword = passwordInputRef.current.value;

    //     const response = await fetch('/api/authenticate', {
    //       method: 'POST',
    //       body: JSON.stringify({ username, password }),
    //       headers: { 'Content-Type': 'application/json' },
    //     });
    //     const data = await response.json();
    //     if (!response.ok) {
    //       throw new Error(data.message);
    //     }
    //     //onAuthenticate(data.token);
    //   } catch (err) {
    //     setError(err.message);
    //   }
  }

  return (
    <Card>
      <form className={classes.form} onSubmit={handleSubmit}>
        <div className={classes.inputarea}>
          <div className={classes.inputitem}>
            <label htmlFor="username">Username:</label>
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
            <label htmlFor="firstname">FirstName:</label>
            <input
              type="text"
              id="firstname"
              required
              ref={firstNameInputRef}
            />
          </div>

          <div className={classes.inputitem}>
            <label htmlFor="lastname">LastName:</label>
            <input
              type="text"
              id="lastname"
              required
              ref={lastNameInputRef}
            />
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
