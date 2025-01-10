import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import apiService from "../../services/apiService";
import "../../styles/Login.css";
import axios from "axios";
import { useContext } from "react";
import { UserContext } from "../../context/UserContext";
import { toast } from 'react-toastify';


const Login = () => {
    const [email, setEmail] = useState("");
    const { setUserRole } = useContext(UserContext);
    const [password, setPassword] = useState("");
    const [twoFactorCode, setTwoFactorCode] = useState("");
    const [userId, setUserId] = useState(null);
    const [errorMessage, setErrorMessage] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();

    const handleLogin = async () => {
        setIsLoading(true);
        setErrorMessage("");

        try {
            const payload = {
                email: email,
                password: password,
            };

            const response = await apiService.post("/Account/login", { body: payload });

            if (response.data && response.data.requiresTwoFactor) {
                setUserId(response.data.userId);
            } else {
                throw new Error("Er is een onverwachte fout opgetreden. 2FA is vereist.");
            }
        } catch (error) {
            console.error("Login error:", error.response || error);
            const message = error.response?.data || "Fout bij inloggen. Probeer opnieuw.";
            setErrorMessage(typeof message === "string" ? message : "Onbekende fout.");
        } finally {
            setIsLoading(false);
        }
    };


    const handleVerifyTwoFactor = async () => {
        setIsLoading(true);
        setErrorMessage("");

        try {
            const payload = {
                userId: userId,
                code: twoFactorCode,
            };

            const response = await axios.post("https://localhost:5033/api/Account/verify-2fa", payload, {
                withCredentials: true, // Ensures cookies are processed
            });

            console.log("2FA successful:", response.data);
            const userRole = response.data.role; // Get role from response

            setUserRole(userRole); // Update de context
            localStorage.setItem("role", userRole);; // Store role in localStorage for UI purposes

            toast.success("Succesvol ingelogd!");
            


            navigate("/");
        } catch (error) {
            console.error("2FA verification failed:", error.response || error);
            setErrorMessage("Verification failed. Please try again.");
        } finally {
            setIsLoading(false);
        }
    };



    return (
        <div className="login-container-wrapper">
            <div className="login-container">
                {!userId ? (
                    <div className="login-form">
                        <h2>Login</h2>
                        <input
                            type="email"
                            placeholder="E-mailadres"
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                            className="login-input"
                        />
                        <input
                            type="password"
                            placeholder="Wachtwoord"
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                            className="login-input"
                        />
                        <button onClick={handleLogin} disabled={isLoading} className="login-button">
                            {isLoading ? "Inloggen..." : "Login"}
                        </button>
                    </div>
                ) : (
                    <div className="two-factor-form">
                        <h2>Tweestapsverificatie</h2>
                        <input
                            type="text"
                            placeholder="Verificatiecode"
                            value={twoFactorCode}
                            onChange={(e) => setTwoFactorCode(e.target.value)}
                            className="login-input"
                        />
                        <button onClick={handleVerifyTwoFactor} disabled={isLoading} className="login-button">
                            {isLoading ? "Verifiëren..." : "Verifieer"}
                        </button>
                    </div>
                )}
                {errorMessage && <p className="error-message">{errorMessage}</p>}
            </div>
        </div>
    );
};

export default Login;
