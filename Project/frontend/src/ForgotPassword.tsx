import React, { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import "./RegisterPopup.css";

export const ForgotPassword: React.FC = () => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const location = useLocation();
    const { herstelCode } = location.state;
    const [code, setCode] = useState<string>("");


    const navigate = useNavigate();

    const handleBack = () => {
        navigate('/');
    };

    const handleChangePassword = async () => {
        if (!password) {
            alert("Please fill in all required fields.");
            return;
        }

        const userData = {
            email,
            password,
        };

        try {
            const response = await fetch('http://localhost:5001/api/password', {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userData),
            });

            if (response.ok) {
                alert("User created successfully!");
                setEmail("");
                setPassword("");

            } else {
                const errorData = await response.json();
                console.error('Error editing password:', errorData);
                alert(`Error: ${response.statusText}`);
            }
        } catch (error) {
            console.error('Network error:', error);
            alert("Failed to edit password. Please try again.");
        }
    };

    return (

        <div className="container">
            <button className="back-button" onClick={handleBack}>Back</button>
            <h1>Forgot Password</h1>
            <div className="form-container">
                <input
                    className="input"
                    type="number"
                    placeholder="enter Herstelcode"
                    value={code}
                    onChange={(e) => setCode(e.target.value)}
                />
                {herstelCode == code && <div>
                    <input
                        className="input"
                        type="email"
                        placeholder="Email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                    />
                    <input
                        className="input"
                        type="password"
                        placeholder="Password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                    />
                </div>}
                <button className="create-button" onClick={handleChangePassword}>
                    Change Password
                </button>
            </div>
        </div>
    );
};
