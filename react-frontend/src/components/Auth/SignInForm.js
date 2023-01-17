import React from "react";
import { useRef, useState } from "react";
import Card from "../ui/Card";
import classes from "../Form.module.css";
import { Link } from "react-router-dom";

function SignInForm() {
  const userNameInputRef = useRef();
  const passwordInputRef = useRef();


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
              required
              ref={userNameInputRef}
            />
          </div>
          <div className={classes.inputitem}>
            <label htmlFor="password">Password:</label>
          

            <input
              type="password"
              id="password"
              required
              ref={passwordInputRef}
            />
          </div>

        




          <div className={classes.actions}>
            {/* {error && <div style={{ color: 'red' }}>{error}</div>} */}
            <button type="submit">Login</button>
            <div className={classes.text}>
              Create a FREE Account,please
              <Link to="/register" className={classes.link}>
                Register
              </Link>
            </div>
          </div>
        </div>
      </form>
    </Card>
  );
}

export default SignInForm;
