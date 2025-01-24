import React, { useState, useContext, useEffect} from "react";
import { useNavigate, Link } from "react-router-dom";
import axios from "axios";
import { UserContext } from "../../context/UserContext";
import { toast } from 'react-toastify';
import "../../styles/Login.css";

const Login = () => {
    const [email, setEmail] = useState("");
    const { setUserRole } = useContext(UserContext);
    const [password, setPassword] = useState("");
    const [twoFactorCode, setTwoFactorCode] = useState(Array(6).fill(""));
    const [userId, setUserId] = useState(null);
    const [showPassword, setShowPassword] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const navigate = useNavigate();


    useEffect(() => {
        const handleKeyPress = (event) => {
            if (event.key === 'Enter') {
                event.preventDefault();
                if (!userId) {
                    handleLogin(event);
                }
            }
        };


        window.addEventListener('keydown', handleKeyPress);

        return () => {
            window.removeEventListener('keydown', handleKeyPress);
        };
    }, [email, password, userId]);



    useEffect(() => {
        const allFieldsFilled = twoFactorCode.every(code => code.trim() !== '');
        if (allFieldsFilled) {
            handleSubmit(new Event('submit'));
        }
    }, [twoFactorCode]); 



    const handleLogin = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");
        try {
            const payload = { email, password };
            const response = await axios.post("https://localhost:5033/api/Account/login", payload);
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



    const handleSubmit = async (event) => {
        event.preventDefault();
        setIsLoading(true);
        setErrorMessage("");
        try {

            const code = twoFactorCode.join('');
            const payload = { userId, code };
            const response = await axios.post("https://localhost:5033/api/Account/verify-2fa", payload, {
                withCredentials: true,
            });

            console.log("2FA successful:", response.data);

            const userRole = response.data.role;

            setUserRole(userRole);
            localStorage.setItem("role", userRole);
            toast.success("Succesvol ingelogd!");

            navigate("/");
        } catch (error) {
            console.error("2FA verification failed:", error.response || error);
            setErrorMessage("Verification failed. Please try again.");
        } finally {
            setIsLoading(false);
        }
    };



    const handleKeyDown = (index, event) => {
        if (event.key === "Backspace") {
            if (twoFactorCode[index] === "") {
                const prevFieldIndex = index - 1;
                if (prevFieldIndex >= 0) {
                    const prevField = document.getElementById(`code-${prevFieldIndex}`);
                    if (prevField) {
                        prevField.focus();
                        const newCode = [...twoFactorCode];
                        newCode[prevFieldIndex] = ""; 
                        setTwoFactorCode(newCode);
                    }
                }
            } else {

                const newCode = [...twoFactorCode];
                newCode[index] = '';
                setTwoFactorCode(newCode);
            }
        } else if (event.key === "ArrowLeft" && index > 0) {
            const prevField = document.getElementById(`code-${index - 1}`);
            prevField && prevField.focus();
        } else if (event.key === "ArrowRight" && index < twoFactorCode.length - 1) {
            const nextField = document.getElementById(`code-${index + 1}`);
            nextField && nextField.focus();
        }
    };


    const handleCodeChange = (index, event) => {
        const newCodes = [...twoFactorCode];
        newCodes[index] = event.target.value;
        setTwoFactorCode(newCodes);

        if (event.target.value && index < twoFactorCode.length - 1) {
            document.getElementById(`code-${index + 1}`).focus();
        }
    };



    const handleKeyPress = (event) => {
        if (event.key === 'Enter') {
            event.preventDefault();
            handleLogin(event);
        }
    };


    return (
        <div className="login-container-wrapper">
            <div className="login-container">
                {!userId ? (
                    <form onSubmit={handleLogin} className="login-form">
                        <h2>Login</h2>
                        <input
                            type="email"
                            placeholder="E-mailadres"
                            value={email}
                            onInput={(e) => setEmail(e.target.value)}
                            className="login-input"
                            onKeyDown={handleKeyPress} 
                        />
                        <div className="password-input-wrapper">
                            <input
                                type={showPassword ? "text" : "password"} 
                                placeholder="Wachtwoord"
                                value={password}
                                onInput={(e) => setPassword(e.target.value)}
                                className="login-input"
                                onKeyDown={handleKeyPress}
                            />
                            <span
                                className="password-toggle-icon"
                                onClick={() => setShowPassword(!showPassword)}
                            >
                                {showPassword ? "👁️" : "🕶️"}
                            </span>
                        </div>
                        <div className="login-links">
                            <Link to="/wachtwoord-vergeten">Wachtwoord Vergeten?</Link>
                        </div>
                        <button type="submit" disabled={isLoading}>
                            {isLoading ? "Inloggen..." : "Login"}
                        </button>
                    </form>
                ) : (
                    <form onSubmit={handleSubmit} className="two-factor-form">
                        <h2>Tweestapsverificatie</h2>
                        <div className="code-input-container">
                            {twoFactorCode.map((code, index) => (
                                <input
                                    key={index}
                                    id={`code-${index}`}
                                    type="text"
                                    maxLength="1"
                                    value={code}
                                    onChange={(e) => handleCodeChange(index, e)}
                                    onKeyDown={(e) => handleKeyDown(index, e)}
                                    className="code-input"
                                    autoComplete="off"
                                />
                            ))}
                            </div>
                            <div className="login-links">
                                <Link to="/2fa-reset">2FA Reset</Link>
                            </div>
                            <button type="submit" disabled={isLoading} className="login-button" style={{ display: "none" }}>
                                Verifieer
                            </button>
                    </form>
                )}
                {errorMessage && <p className="error-message">{errorMessage}</p>}
            </div>
        </div>
    );
};

export default Login;




