import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import "./RegisterPopup.css";

export const RegisterPopup: React.FC = () => {
    const [firstName, setFirstName] = useState("");
    const [lastName, setLastName] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [recurringDays, setRecurringDays] = useState("");

    const navigate = useNavigate();

    const handleBack = () => {
        navigate('/calender');
    };

    const handleCreateUser = async () => {
        if (!firstName || !lastName || !email || !password) {
            alert("Please fill in all required fields.");
            return;
        }

        const userData = {
            firstName,
            lastName,
            email,
            password,
            recurringDays,
        };

        try {
            const response = await fetch('http://localhost:5001/api/register', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(userData),
            });

            if (response.ok) {
                alert("User created successfully!");
                setFirstName("");
                setLastName("");
                setEmail("");
                setPassword("");
                setRecurringDays("");
            } else {
                const errorData = await response.json();
                console.error('Error creating user:', errorData);
                alert(`Error: ${response.statusText}`);
            }
        } catch (error) {
            console.error('Network error:', error);
            alert("Failed to create user. Please try again.");
        }
    };

    return (
        <div className="container">
            <button className="back-button" onClick={handleBack}>Back</button>
            <h1>Create User</h1>
            <div className="form-container">
                <input
                    className="input"
                    type="text"
                    placeholder="First Name"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                />
                <input
                    className="input"
                    type="text"
                    placeholder="Last Name"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                />
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
                <input
                    className="input"
                    type="number"
                    max={7}
                    placeholder="1-7"
                    value={recurringDays}
                    onChange={(e) => setRecurringDays(e.target.value)}
                />
                <button className="create-button" onClick={handleCreateUser}>
                    Create User
                </button>
            </div>
        </div>
    );
};
