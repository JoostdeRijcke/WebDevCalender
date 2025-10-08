import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import './LoginPage.css';
import logo from './calender.png';

export const UserInput: React.FC = () => {
  const [username, setUsername] = useState<string>(''); // State for username
  const [password, setPassword] = useState<string>(''); // State for password
  const [showPassword, setShowPassword] = useState<boolean>(false); // State for toggling password visibility
  const [loginStatus, setLoginStatus] = useState<string | null>(null); // State for showing login status
  const navigate = useNavigate();
  const [herstelCode, setHerstelCode] = useState<string>("");
  const handleLogin = async () => {
    if (!username || !password) {
      alert('Please fill in both fields.');
      return;
    }

    const data = {
      Email: username,
      Password: password,
    };

    try {
      const response = await fetch('http://localhost:5001/api/Login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        credentials: 'include', // Include cookies
        body: JSON.stringify(data),
      });

      if (response.ok) {
        console.log('Login successful: ', response);
        navigate('/calender');
      } else {
        alert(response.statusText);
      }
    } catch (error) {
      alert('Er is iets misgegaan');
      console.error('Error logging in:', error);
    }
  };

  const forgotPassword = async () => {
    try {
      const response = await fetch('http://localhost:5001/api/ForgotPassword', {
        method: 'GET',
        credentials: 'include', // Include cookies
      });

      if (response.ok) {
        const data = await response.json();
        console.log(data);
        setHerstelCode(data);
      } else {
        alert(response.statusText);
      }
    } catch (error) {
      setLoginStatus('Error checking login status.');
      console.error('Error checking login status:', error);
    }

  };

  useEffect(() => {
    if (herstelCode) {
      console.log('Updated herstelCode:', herstelCode);
      navigate('/forgot', { state: { herstelCode } });
    }
  }, [herstelCode]);

  const togglePasswordVisibility = () => {
    setShowPassword((curState) => !curState);
  };

  return (
    <div>
      <header className="LoginPage-header">
        <img src={logo} alt="Logo" />
        <div style={{ marginTop: '7vmin' }}>
          <label htmlFor="user">Username: </label>
          <input
            id="user"
            type="text"
            value={username}
            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
              setUsername(event.target.value);
            }}
            placeholder="Enter your username"
          />
        </div>

        <div style={{ marginTop: '10px' }}>
          <label htmlFor="pass">Password: </label>
          <input
            id="pass"
            style={{ marginRight: '10px' }}
            type={showPassword ? 'text' : 'password'}
            value={password}
            onChange={(event: React.ChangeEvent<HTMLInputElement>) => {
              setPassword(event.target.value);
            }}
            placeholder="Enter your password"
          />
          <button onClick={togglePasswordVisibility}>show password</button>
        </div>

        <button
          onClick={handleLogin}
          style={{ marginTop: '10px', padding: '5px 10px' }}
        >
          Log In
        </button>

        <button
          onClick={forgotPassword}
          style={{
            marginTop: '10px',
            padding: '5px 10px',
            backgroundColor: '#007BFF',
            color: 'white',
            border: 'none',
            borderRadius: '5px',
          }}
        >
          Forgot Password
        </button>

      </header>
    </div>
  );
};
