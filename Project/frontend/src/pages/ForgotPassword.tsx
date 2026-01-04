import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "../styling/RegisterPopup.css";

export const ForgotPassword: React.FC = () => {
    const [email, setEmail] = useState<string>("");
    const [code, setCode] = useState<string>("");
    const [password, setPassword] = useState<string>("");

    const [codeSent, setCodeSent] = useState<boolean>(false);
    const [codeVerified, setCodeVerified] = useState<boolean>(false);
    const [error, setError] = useState<string>("");

    const navigate = useNavigate();

    const handleBack = () => navigate("/");

    const handleSendCode = async (): Promise<void> => {
        setError("");

        const res = await fetch("http://localhost:5001/api/generatecode", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email }),
        });

        if (res.ok) setCodeSent(true);
        else setError("Failed to send reset code.");
    };

    const handleVerifyCode = async (): Promise<void> => {
        const res = await fetch("http://localhost:5001/api/checkcode", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ code, email }),
        });

        if (res.ok) setCodeVerified(true);
        else setError("Invalid or expired code.");
    };

    const handleChangePassword = async (): Promise<void> => {
        const res = await fetch("http://localhost:5001/api/password", {
            method: "PUT",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, password }),
        });

        if (res.ok) {
            alert("Password changed successfully!");
            navigate("/login");
        } else setError("Failed to change password.");
    };

    return (
        <div className="container">
            <button className="back-button" onClick={handleBack}>Back</button>
            <h1>Forgot Password</h1>

            <div className="form-container">
                <input
                    className="input"
                    type="email"
                    placeholder="Email"
                    value={email}
                    disabled={codeSent}
                    onChange={(e) => setEmail(e.target.value)}
                />

                {!codeSent && (
                    <button onClick={handleSendCode}>Send Code</button>
                )}

                {codeSent && (
                    <>
                        <input
                            className="input"
                            type="number"
                            placeholder="Enter reset code"
                            value={code}
                            disabled={codeVerified}
                            onChange={(e) => setCode(e.target.value)}
                        />

                        {!codeVerified && (
                            <button onClick={handleVerifyCode}>Verify Code</button>
                        )}
                    </>
                )}

                {codeVerified && (
                    <>
                        <input
                            className="input"
                            type="password"
                            placeholder="New password"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />

                        <button className="create-button" onClick={handleChangePassword}>
                            Change Password
                        </button>
                    </>
                )}

                {error && <p className="error">{error}</p>}
            </div>
        </div>
    );
};
